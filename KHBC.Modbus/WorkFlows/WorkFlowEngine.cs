using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using KHBC.Core.BaseModels;
//using KHBC.Const.Enums;
using KHBC.Core.Extend;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using KHBC.Core;
using KHBC.Modbus.Enums;
using CSRedis;
using KHBC.Core.Log;

namespace KHBC.Modbus.WorkFlows
{
    /// <summary>
    /// 工作流引擎
    /// </summary>
    public class WorkFlowEngine : BaseService
    {
        public string ModuleName { get; set; } = "WFE";
        public sealed override string ServiceName { get; set; }
        private CSRedisClient RedisClientLocal;
        private CSRedisClient RedisClientRemote;

        public readonly ConcurrentQueue<FlowBlock> BlockQueue = new ConcurrentQueue<FlowBlock>();//流程队列
        public readonly ConcurrentQueue<Step> StepQueue = new ConcurrentQueue<Step>();//步骤队列
        public static event Action<string> Loged;
        public static event Action<string> ErrorLoged;
        public readonly WorkFlow _workFlow;//对应的工作流
        private readonly ModBusBase _modBusClient;//对应的modbus客户端单例
        public readonly Dictionary<string, PlcAddress> AddressDict;

        private readonly Stopwatch _watchSetp = new Stopwatch();//步骤计时
        private FlowBlock _curBlock;//当前模块

        public WorkFlowEngine(WorkFlow work, ModBusBase modBus, Dictionary<string, PlcAddress> dict)
        {
            _workFlow = work;
            _modBusClient = modBus;
            AddressDict = dict;
            ServiceName = $"{ModuleName}:{work.Name}";
        }

        public void EnqueueBlock(FlowBlock block)
        {
            if (block.IsRunning)
            {
                //如果当前模块正在执行则略过
                OnLoged($"[block] isRuuning");
                return;
            }
            //开始执行模块
            BlockQueue.Enqueue(block);
        }

        public override void ThreadWork()
        {
            try
            {
                RedisClientLocal = new CSRedisClient(SysConf.Main.RedisLocal.ConnectStrings);
                Logger.Main.Info($"[{ServiceName}]初始化LOCAL REDIS CLIENT成功: \"{SysConf.Main.RedisLocal.ConnectStrings}\"");
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{ServiceName}]初始化LOCAL REDIS CLIENT失败: \"{SysConf.Main.RedisLocal.ConnectStrings}\", {ex.Message}");
            }

            try
            {
                RedisClientRemote = new CSRedisClient(SysConf.Main.RedisRemote.ConnectStrings);
                Logger.Main.Info($"[{ServiceName}]初始化REMOTE REDIS CLIENT成功: \"{SysConf.Main.RedisRemote.ConnectStrings}\"");
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[{ServiceName}]初始化REMOTE REDIS CLIENT失败: {ex.Message}");
            }

            //while (true)
            //{
            //    if (State == ServiceState.Suspend)
            //        re.WaitOne();
            //    else if (State == ServiceState.Abort)
            //        break;
            //    DoWork();
            //}
            //OnLoged($"[{Thread.CurrentThread}] exit");
        }

        public override void Start()
        {
            try
            {
                _modBusClient.Open();
                OnLoged($"modbus:{_modBusClient.Name} open success! MultiThread is {_workFlow.MultiThread}");
            }
            catch (Exception e)
            {
                OnErrorLoged($"modbus:{_modBusClient.Name} open fail", e);
                return;
            }
            base.Start();
        }
        protected override void DoWork()
        {
            if (!_modBusClient.IsConnected)
            {
                OnLoged($"[{_workFlow.PlcDeviceName}] has disconnected  and  reconnect after 5s  ");
                _modBusClient.Open();
                if (_modBusClient.IsConnected)
                    OnLoged($"[{_workFlow.PlcDeviceName}] has reconnected ");
                else
                    Thread.Sleep(5 * 1000);
            }
            //队列有任务执行任务
            else if (StepQueue.TryDequeue(out Step step))
            {
                if (_workFlow.MultiThread)
                    ExecStepMutiThread(step);
                else
                {
                    ExecStepSingleThread(step);
                    if (!step.Result.IsSuccess)
                    {
                        OnLoged($"[{_curBlock?.Name}][{step.Seq}]fail and  block exit ");
                        //清空队列
                        while (StepQueue.TryDequeue(out Step stepclear))
                        {
                        }
                    }
                }
            }
            //队列有无任务 执行模块
            else if (BlockQueue.TryDequeue(out FlowBlock block))
            {
                OnLoged($"[{block.Name}] enter");
                if (_workFlow.MultiThread)
                    ExecBlockMutiThread(block);
                else
                    ExecBlockSingleThread(block);
            }
            else //无任务无模块执行监听
                ExecListen();
        }


        /// <summary>
        /// 单线程处理模块,每次将所有step都放入队列执行
        /// </summary>
        /// <param name="block"></param>
        private void ExecBlockSingleThread(FlowBlock block)
        {
            _curBlock = block;
            if (_curBlock.Steps != null)
            {
                int seq = 1;
                foreach (var item in _curBlock.Steps)
                {
                    item.Seq = seq++;
                    StepQueue.Enqueue(item);
                }
            }
        }
        /// <summary>
        /// 单线程执行整个步骤，每次执行完plc 再执行action
        /// </summary>
        /// <param name="step"></param>
        private void ExecStepSingleThread(Step step)
        {
            if (step.Type != ActionTypes.None)
            {
                if (!AddressDict.ContainsKey(step.PlcAddressName))
                {
                    OnLoged($"[{_curBlock?.Name}][{step.Seq}]not found PlcAddressName {step.PlcAddressName}");
                    step.Result = Result.Fail();
                    return;
                }
                _watchSetp.Restart();
                ExecPlcStep(step);
                _watchSetp.Stop();
                OnLoged($"[{_curBlock?.Name}][{step.Seq}][{step.PlcAddressName}]{step.Result.IsSuccess} {step.Result.Msg} 耗时{_watchSetp.ElapsedMilliseconds}ms");
            }
            if (step.Result.IsSuccess && !step.ActionName.IsNullOrWhiteSpace())
            {
                _watchSetp.Restart();
                //方法的执行结果不影响步骤
                ActionArgs actionArgs = new ActionArgs
                {
                    WorkFlowName = _workFlow.Name,
                    BlockName = _curBlock.Name,
                    StepSeq = step.Seq,
                    StepResult = step.Result,
                    RedisClientLocal = RedisClientLocal,
                    RedisClientRemote = RedisClientRemote
                };
                var res = ActionProx(step.ActionName, _workFlow.ActionTimeOut, actionArgs);
                _watchSetp.Stop();
                OnLoged($"[{_curBlock?.Name}][{step.Seq}][{step.ActionName}]{res.IsSuccess} {res.Msg} 耗时{_watchSetp.ElapsedMilliseconds}ms");
            }
        }

        /// <summary>
        /// 多线程处理模块,每个模块单独线程处理任务，单个的把任务放入step队列
        /// </summary>
        /// <param name="block"></param>
        private void ExecBlockMutiThread(FlowBlock block)
        {
            block.IsRunning = true;
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                block.ExecEvent.Set();//默认打开
                if (block.Steps != null)
                {
                    int seq = 1;
                    var watch = new Stopwatch();
                    foreach (var step in block.Steps)
                    {

                        step.Seq = seq++;
                        step.Perform = null;
                        if (step.Type != ActionTypes.None)
                        {
                            if (!AddressDict.ContainsKey(step.PlcAddressName))
                            {
                                OnLoged($"[{block.Name}][{step.Seq}]not found PlcAddressName {step.PlcAddressName}");
                                step.Result = Result.Fail();
                                break;
                            }
                            watch.Restart();
                            step.Perform = () => block.ExecEvent.Set();
                            //放入plc执行队列
                            StepQueue.Enqueue(step);
                            //等待放行信号
                            block.ExecEvent.WaitOne();
                            watch.Stop();
                            OnLoged($"[{block.Name}][{step.Seq}][{step.PlcAddressName}]{step.Result.IsSuccess} {step.Result.Msg} 耗时{watch.ElapsedMilliseconds}ms");
                        }
                        if (step.Result.IsSuccess)
                        {
                            if (step.Result.IsSuccess && !step.ActionName.IsNullOrWhiteSpace())
                            {
                                _watchSetp.Restart();
                                //方法的执行结果不影响步骤
                                ActionArgs actionArgs = new ActionArgs
                                {
                                    WorkFlowName = _workFlow.Name,
                                    BlockName = _curBlock.Name,
                                    StepSeq = step.Seq,
                                    StepResult = step.Result,
                                    RedisClientLocal = RedisClientLocal,
                                    RedisClientRemote = RedisClientRemote
                                };
                                var res = ActionProx(step.ActionName, _workFlow.ActionTimeOut, actionArgs);
                                _watchSetp.Stop();
                                OnLoged($"[{block.Name}][{step.Seq}][{step.ActionName}]{res.IsSuccess} {res.Msg} 耗时{_watchSetp.ElapsedMilliseconds}ms");
                            }
                        }
                    }
                }
                OnLoged($"[{block.Name}]exit");
                block.IsRunning = false;
            });
        }

        /// <summary>
        /// 多线程执行整个步骤，只执行plc处理，并需要调用执行完成
        /// </summary>
        /// <param name="step"></param>
        private void ExecStepMutiThread(Step step)
        {
            if (!AddressDict.ContainsKey(step.PlcAddressName))
            {
                OnLoged($"[{_curBlock?.Name}][{step.Seq}]not found PlcAddressName {step.PlcAddressName}");
                step.Perform?.Invoke();
                return;
            }
            ExecPlcStep(step);
            step.Perform?.Invoke();
        }

        /// <summary>
        /// 执行plc步骤
        /// </summary>
        private void ExecPlcStep(Step step)
        {
            try
            {
                var plcAddress = AddressDict[step.PlcAddressName];
                var time = step.TimeOut == 0 ? _workFlow.PLCTimeOut : step.TimeOut;
                if (step.Type == ActionTypes.Read || step.Type == ActionTypes.Compare)
                {
                    GetByteValue(plcAddress, step);
                    if (step.Result.IsSuccess)
                    {
                        if (step.Type == ActionTypes.Compare)
                        {
                            if (step.Value != step.Result.Data.ToString())
                            {
                                step.Result = Result.Fail("读取值与比较值不符");
                                return;
                            }
                        }
                    }
                }
                else if (step.Type == ActionTypes.Write)
                {
                    SetByteValue(plcAddress, step);
                }
                else if (step.Type == ActionTypes.Wait)
                {
                    if (step.Value.IsNullOrWhiteSpace())
                    {
                        step.Result = Result.Fail("未设置value值");
                        return;
                    }
                    var token = new CancellationTokenSource(time * 1000);
                    var res = this.InvokeActionWithTimeOut(
                        t =>
                        {
                            while (!t.IsCancellationRequested)
                            {
                                GetByteValue(plcAddress, step);
                                if (step.Result.Data.ToString() == step.Value)
                                {
                                    step.Result = Result.Success();
                                    break;
                                }
                            }
                        }, token);
                    if (!res)
                    {
                        step.Result = Result.Fail("Action Fail");
                    }
                }
            }
            catch (Exception e)
            {
                OnErrorLoged($"ExecPlcStep方法异常 step {step.PlcAddressName} ", e);
                step.Result = Result.Fail(e.Message);
            }
        }


        /// <summary>
        /// 执行监听
        /// </summary>
        private void ExecListen()
        {
            var list = _workFlow.Listens ?? new List<PlcListen>();
            foreach (var listen in list)
            {
                if (listen.Items?.Count == 0 || listen.Items == null)
                {
                    listen.Items = new List<Item> { new Item { Index = 0, OnBlockName = listen.OnBlockName, OffBlockName = listen.OffBlockName } };
                }
                if (!AddressDict.ContainsKey(listen.StartAddressName))
                    continue;
                var address = AddressDict[listen.StartAddressName];
                //信号位
                var result = _modBusClient.ReadInt16(address.Address, (ushort)listen.Len);
                if (result.IsSuccess)
                {
                    foreach (var item in listen.Items)
                    {
                        if (item.Index < 0 && item.Index < listen.Len)
                        {
                            OnLoged($"Listen {listen.Name} index must >=0 and < len ");
                            continue;
                        }
                        if (result.Data[item.Index].ToString() != item.Value)
                        {
                            item.Value = result.Data[item.Index].ToString();
                            var blockname = item.Value == "0" ? item.OffBlockName : item.OnBlockName;
                            var block = _workFlow.Blocks.FirstOrDefault(x => x.Name == blockname);
                            if (block == null)
                            {
                                OnLoged($"Listen not found block");
                            }
                            else
                            {
                                //如果block状态由本身维持则无需克隆
                                //   EnqueueBlock(block.CloneEx());
                                OnLoged($"Listen:{listen.Name} has Has triggered");
                                EnqueueBlock(block);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 方法代理
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timeout"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private Result ActionProx(string action, int timeout, ActionArgs input)
        {
            try
            {
                var actionName = action.ToUpper();
                if (ModBusDataFactory.ActionDict.ContainsKey(actionName))
                {
                    return this.InvokeActionWithTimeOut(() =>
                     {
                         Thread.CurrentThread.IsBackground = true;
                         var handlerName = ModBusDataFactory.ActionDict[actionName];
                         var method = ModBusDataFactory.MethdDict[actionName];
                         var handle = Common.GetInstance<IActionHandle>(handlerName);
                         var output = (Result)method.Invoke(handle, new object[] { input });
                         return output;
                     }, timeout);
                }
                else
                    return Result.Fail($"不存在方法{action}");
            }
            catch (Exception e)
            {
                return Result.Fail($"方法{action},异常：{e.Message}");
            }
        }

        /// <summary>
        /// 指定时间内执行函数
        /// </summary>
        /// <param name="func"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public Result InvokeActionWithTimeOut(Func<Result> func, int timeout)
        {
            try
            {
                var t = new Task<Result>(func);
                t.Start();
                if (timeout > 0)
                {
                    if (t.Wait(timeout * 1000))
                        return t.Result;
                    return Result.Fail("方法超时");
                }
                t.Wait();
                return t.Result;
            }
            catch (Exception e)
            {
                OnErrorLoged("InvokeActionWithTimeOut方法异常 ", e);
                return Result.Fail($"InvokeActionWithTimeOut方法异常：{e.Message}");
            }
        }

        /// <summary>
        /// 指定时间内执行函数
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public bool InvokeActionWithTimeOut(Action<CancellationTokenSource> action, CancellationTokenSource cancel)
        {
            try
            {
                var t = new Task(() => { action(cancel); });
                t.Start();
                t.Wait();
                return !cancel.IsCancellationRequested;
            }
            catch (Exception e)
            {
                OnErrorLoged("InvokeActionWithTimeOut方法异常 ", e);
                return false;
            }
        }


        public void OnLoged(string msg)
        {
            Loged?.Invoke($"[{ServiceName}]{msg}");
        }


        public void OnErrorLoged(string str, Exception obj)
        {
            ErrorLoged?.Invoke($"[{ServiceName}]{str}error:{obj.Message}");
        }

        public void GetByteValue(PlcAddress plcAddress, Step step)
        {
            try
            {
                var res = _modBusClient.Read(plcAddress.Address, (ushort)plcAddress.Len);
                if (!res.IsSuccess)
                    step.Result = Result.Fail("plc操作失败");
                switch (plcAddress.DataType)
                {
                    case PlcDataType.BIT:
                        step.Result = Result.Success(BitConverter.ToInt16(res.Data, 0));
                        break;
                    case PlcDataType.WORD:
                        step.Result = Result.Success(Encoding.ASCII.GetString(res.Data));
                        break;
                    case PlcDataType.DWORD:
                        step.Result = Result.Success(Encoding.Unicode.GetString(res.Data));
                        break;
                    case PlcDataType.FLOAT:
                        step.Result = Result.Success(BitConverter.ToSingle(res.Data, 0));
                        break;
                }
            }
            catch (Exception e)
            {
                OnErrorLoged(e.Message, e);
            }
        }


        public void SetByteValue(PlcAddress plcAddress, Step step)
        {
            try
            {
                var data = new byte[] { };
                switch (plcAddress.DataType)
                {
                    case PlcDataType.BIT:
                        data = BitConverter.GetBytes(short.Parse(step.Value));
                        break;
                    case PlcDataType.WORD:
                        data = Encoding.ASCII.GetBytes(step.Value);
                        break;
                    case PlcDataType.DWORD:
                        data = Encoding.Unicode.GetBytes(step.Value);
                        break;
                    case PlcDataType.FLOAT:
                        data = BitConverter.GetBytes(float.Parse(step.Value));
                        break;
                }
                step.Result = _modBusClient.Write(plcAddress.Address, data);
            }
            catch (Exception e)
            {
                OnErrorLoged(e.Message, e);
            }

        }
    }

    public class ActionArgs
    {
        public string WorkFlowName;
        public string BlockName;
        public int StepSeq;
        public Result StepResult;
        public CSRedisClient RedisClientLocal;
        public CSRedisClient RedisClientRemote;
    }
}
