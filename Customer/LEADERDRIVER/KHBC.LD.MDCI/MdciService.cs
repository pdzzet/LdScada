using KHBC.Core;
using KHBC.Core.BaseModels;
using KHBC.Core.Log;
using KHBC.DataAccess;
using KHBC.LD.MDCI.MesModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHBC.LD.MDCI
{
    public class MdciService : BaseService
    {
        public sealed override string ServiceName { get; set; }
        public static Repository MesRepository;

        public MdciService()
        {
            ServiceName = $"{SysConf.ModuleName}";
            MesRepository = new Repository(SysConf.Main.DbMES);
            TimeSpan = 1000;
        }

        protected override void DoWork()
        {
            //执行查询写入队列
            QureyOrder();
            // 数据库操作
            DoMesDbAction();
        }

        #region MES数据库操作
        private void DoMesDbAction()
        {
            try
            {
                var obj = Pop<MessageDataObject>(MdciKeyConf.CmdQueue);
                if (obj != null)
                {
                    var actionName = obj.ActionName?.ToUpper();

                    switch (actionName)
                    {
                        case "ADD":
                            MesDbController.Add(obj);
                            break;
                        case "UPDATE":
                            MesDbController.Update(obj);
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion


        #region MES数据查询
        /// <summary>
        /// 根据子级工单号，查询所有工序信息
        /// </summary>
        /// <param name="workOeder"></param>
        /// <returns></returns>
        private List<TB_MES_DISPATCH> QueryOpCodeByWorkOrder(string workOeder)
        {
            return MesRepository.GetList<TB_MES_DISPATCH>(w => w.IS_READ == 0 && w.WORK_CODE_1 == workOeder);
        }

        /// <summary>
        /// List去重
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<string> ListDistinct(List<TB_MES_DISPATCH> list)
        {
            List<string> result = new List<string>();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!result.Contains(list[i].WORK_CODE_1))
                        result.Add(list[i].WORK_CODE_1);
                }
            }
            return result;
        }
        #endregion

        #region 派工单
        private void QureyOrder()
        {
            //获取未获取的工单信息
            //var list = _mesRepository.GetList<TB_MES_DISPATCH>("select * from mes_dispatch");
            var list = MesRepository.GetList<TB_MES_DISPATCH>(w => w.IS_READ == 0);
            //去重后数据保存
            List<string> result = new List<string>();
            result = ListDistinct(list);
            //相同派工单所有工序的集合
            List<TB_MES_DISPATCH> listDispatch = new List<TB_MES_DISPATCH>();
            Dictionary<string, List<TB_MES_DISPATCH>> dicDispatch = new Dictionary<string, List<TB_MES_DISPATCH>>();
            if (!result.Any())
                return;
            foreach (var obj in result)
            {
                if (!dicDispatch.ContainsKey(obj))
                {
                    listDispatch = QueryOpCodeByWorkOrder(obj);
                    dicDispatch.Add(obj, listDispatch);
                }
            }
            if (!list.Any())
                return;
            foreach (KeyValuePair<string, List<TB_MES_DISPATCH>> kvp in dicDispatch)
            {
                var key = $"LD:{kvp.Value[0].LINE}:MCIM:{kvp.Key}";
                Set(key, kvp.Value);
                //当前订单产品的物料号
                Set($"LD:{kvp.Value[0].LINE}:MCIM:MATERIAL_CODE", kvp.Value[0].MATERIAL_CODE);
            }
            var ids = list.Select(x => x.ORDER_CODE);
            //更新为已获取
            MesRepository.Update<TB_MES_DISPATCH>(u => new TB_MES_DISPATCH { IS_READ = 1 }, w => ids.Contains(w.WORK_CODE_1));
            Logger.Main.Info($"已读订单：{string.Join(",", ids)}");
        }
        #endregion
    }
}
