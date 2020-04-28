using KHBC.Core;
using KHBC.Core.Device;
using KHBC.LD.APCM.BEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHBC.LD.APCM.Workflow
{
    public class ApcmWorkflowP05 : ApcmWorkflowService
    {
        public ApcmWorkflowP05(DeviceInfo devInfo)
        {
            DevInfo = devInfo;
            ServiceName = $"{SysConf.ModuleName}:WF_P05";

            #region 五号位事件
            RegEvent(ApcmDevcieConfig.EVENT_P5_BENSCADAREADSPE, OnEvent_P5_bEnScadaReadSpe);
            RegEvent(ApcmDevcieConfig.EVENT_P5_SPETASKDONE, OnEvent_P5_SpeTaskDone);
            RegEvent(ApcmDevcieConfig.EVENT_P5_BENSCADAREAD, OnEvent_P5_bEnScadaRead);
            RegEvent(ApcmDevcieConfig.ACTION_P5_CENTINFO, OnEvent_P5_CentInfoReadDone);
            #endregion
        }

        #region 五号位事件
        public void OnEvent_P5_bEnScadaReadSpe(ApcmEventArgs evt)
        {
            Read(ApcmDevcieConfig.ACTION_P5_CENTINFO);
        }

        public void OnEvent_P5_SpeTaskDone(ApcmEventArgs evt)
        {

        }
        public void OnEvent_P5_bEnScadaRead(ApcmEventArgs evt)
        {
            Read(ApcmDevcieConfig.ACTION_P5_CENTINFO);

        }
        public void OnEvent_P5_CentInfoReadDone(ApcmEventArgs evt)
        {
            P5CentInfoList p5CentInfoList = new P5CentInfoList();
            p5CentInfoList = (P5CentInfoList)StructTransform.BytesToStruct((byte[])(((Dictionary<string, object>)evt.Data)[ApcmDevcieConfig.ACTION_P5_CENTINFO]), p5CentInfoList.GetType());
            Set($"{ApcmKeyConf.DataCollection}:{ApcmDevcieConfig.ACTION_P5_CENTINFO}:DATA", p5CentInfoList);
            //生成批次号
            p5CentInfoList = CreateBatch(p5CentInfoList);
            Write(ApcmDevcieConfig.ACTION_P5_CENTINFO, StructTransform.StructToBytes(p5CentInfoList));
            Write(ApcmDevcieConfig.ACTION_P5_BSCADAWRITEDONE, 1);
            bool IsLast = JudgeIsLast(p5CentInfoList);
            if (!IsLast)
            {
                Write(ApcmDevcieConfig.ACTION_P5_SPETASK, 1);
            }
            
        }
        private bool JudgeIsLast(P5CentInfoList list)
        {
            //TODO 判断是否需要特殊放行
            foreach (P5CentInfo centInfo in list.p5CentInfos)
            {
                if (String.IsNullOrEmpty(centInfo.BatchCode))
                {
                    return false;
                }
            }
            return true;
        }

        private P5CentInfoList CreateBatch(P5CentInfoList centInfoList)
        {
            //TODO 生成新的批次号
            String BatchCode = "Z19001 - DR"+ DateTime.Now.Month + DateTime.Now.Day;
            ;
            if (!string.IsNullOrEmpty(centInfoList.p5CentInfos[0].BatchCode))
            {
                centInfoList.p5CentInfos[0].BatchCode = BatchCode;
            }
            if (!string.IsNullOrEmpty(centInfoList.p5CentInfos[1].BatchCode))
            {
                centInfoList.p5CentInfos[1].BatchCode = BatchCode;
            }
            if (!string.IsNullOrEmpty(centInfoList.p5CentInfos[2].BatchCode))
            {
                centInfoList.p5CentInfos[2].BatchCode = BatchCode;
            }
            return centInfoList;
        }
        #endregion
    }
}
