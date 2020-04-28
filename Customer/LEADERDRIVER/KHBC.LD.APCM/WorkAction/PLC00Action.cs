using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KHBC.Core;
using KHBC.Core.Extend;
using KHBC.Core.Log;
using KHBC.DataAccess;
using KHBC.LD.APCM.BEntity;
using KHBC.LD.DTO;
using KHBC.LD.MDCI.MESModels;
using KHBC.Modbus;
using KHBC.Modbus.WorkFlows;

namespace KHBC.LD.APCM
{
    public class PLC00Action : IActionHandle
    {
        private Repository _mesRepository;
        //获取派工单信息；做线体准备工作
        public Result GetOrderEvent(ActionArgs actionArgs)
        {
            //下发工序，CNC程序等给线体机台
            SynCncOpInfo();
            Constant.OrderLine = ModBusDataFactory.OrderLine;
            if (Constant.Package == -1)
            {
                int package = int.Parse(actionArgs.RedisClientLocal.Get($"LD:{Constant.OrderLine}:APCM:PACKAGE"));
                Constant.Package = package;
            }
            //获取2号位置数量
            if (Constant.p2Num == -1)
            {
                Constant.p2Num = int.Parse(actionArgs.RedisClientLocal.Get($"LD:{Constant.OrderLine}:APCM:P2NUM"));
            }
            //获取4号位置数量
            if (Constant.p4Num == -1)
            {
                Constant.p4Num = int.Parse(actionArgs.RedisClientLocal.Get($"LD:{Constant.OrderLine}:APCM:P4NUM"));
            }
            //查找当前工作流引擎
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            if (Constant.Package > 25)
            {
                //触发空箱任务模块
                FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC01SetNullTaskB");
                eng?.EnqueueBlock(block);
            }
            else
            {
                //触发正常上料模块
                FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC01SetAvgCallB");
                eng?.EnqueueBlock(block);
            }
            Logger.Device.Info($" invoke EmptyAgvCallEvent 获取派工单成功,并判断下发什么任务");
            return Result.Success();
        }

        private void SynCncOpInfo()
        {
            List<Tuple<string, string, OpInfo>> opInfos = new List<Tuple<string, string, OpInfo>>();
            List<Tuple<string, string, NcInfo>> cncInfos = new List<Tuple<string, string, NcInfo>>();
            foreach (RMesDispatch dispatch in ModBusDataFactory.dispatrchs.OrderBy(v => v.OP_CODE))
            {
                _mesRepository = new Repository(SysConf.Main.DbMES);
                TB_ROUTE_OP rOuteOp = _mesRepository.Single<TB_ROUTE_OP>(v => v.OP_CODE == dispatch.OP_CODE);
                OpInfo opInfo = new OpInfo()
                {
                    OpSeq = rOuteOp.OP_SEQ,
                    OpCode = rOuteOp.OP_CODE,
                    OpDoc = dispatch.OP_DOC,
                    OpDocPath = dispatch.OP_DOC_PATH,
                    OpDocVer = dispatch.OP_DOC_VER
                };
                NcInfo ncInfo = new NcInfo()
                {
                    NcId = dispatch.NC_ID,
                    NcPath = dispatch.NC_PATH,
                    NcVer = dispatch.NC_VER
                };
                opInfos.Add(new Tuple<string, string, OpInfo>(dispatch.LINE, dispatch.EQUIP_CODE, opInfo));
                cncInfos.Add(new Tuple<string, string, NcInfo>(dispatch.LINE, dispatch.EQUIP_CODE, ncInfo));
            }
            //按设备编号正向排序，得出工序
            opInfos = opInfos.OrderBy(v => v.Item2).ToList();
            byte[] cncOp = new byte[12];
            int i = 0;
            foreach (Tuple<string, string, OpInfo> tuple in opInfos)
            {
                i++;
                cncOp[i] = Convert.ToByte(tuple.Item3.OpSeq); 
            }
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            //触发更新CnCOp模块
            FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC00CncOpB");
            block.Steps[0].Value = Encoding.ASCII.GetString(cncOp);
            eng?.EnqueueBlock(block); 
        }

        //请求产生抽检料
        public Result SpotReqCheckTaskEvent(ActionArgs actionArgs)
        {
            //查找当前工作流引擎
            var eng = ModBusDataFactory.workFlowEngines.FirstOrDefault(x => x.ModuleName == "WFE");
            FlowBlock block = eng?._workFlow.Blocks.FirstOrDefault(v => v.Name == "PLC00SpotReqCheckTaskB");
            if (block != null)
            {
                eng?.EnqueueBlock(block);
            }
            Logger.Device.Info($" invoke SpotReqCheckTaskEvent 请求产生抽检料执行完毕");
            return Result.Success();
        }
        //尾巴特殊放行指令
        public Result EndSpeTaskEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke EndSpeTaskEvent 尾巴特殊放行指令执行完毕");
            return Result.Success();
        }
        //机台OP更新完毕
        public Result CncOpUpDoneEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke CncOpUpDoneEvent 机台OP更新完毕");
            return Result.Success();
        }
        //抽检任务已经产生
        public Result SpotCheckDoneEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke SpotCheckDoneEvent 抽检任务已经产生");
            return Result.Success();
        }

        //加工位置序列号读取完毕
        public Result LineOpInfo1ReadEvent(ActionArgs actionArgs)
        {
            //存放对应的结果到本地
            Logger.Device.Info($" invoke LineOpInfo1ReadEvent 加工位置序列号读取完毕");
            return Result.Success();
        }
        //备用位置序列号读取完毕
        public Result LineOpInfo2ReadEvent(ActionArgs actionArgs)
        {
            //存放对应的结果到本地
            Logger.Device.Info($" invoke LineOpInfo2ReadEvent 备用位置序列号读取完毕");
            return Result.Success();
        }
        //呼叫人工取抽检料
        public Result SpotCallEvent(ActionArgs actionArgs)
        {
            Logger.Device.Info($" invoke SpotCallEvent 呼叫人工取抽检料");
            return Result.Success();
        }
    }
}
