using KHBC.Core;
using System.Collections.Generic;
using KHBC.LD.MDCI.MesModels;
using System;

namespace KHBC.LD.MDCI
{
    public class MesDbController
    {
        public static void Add(MessageDataObject mdo)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (var d in mdo.Data)
            {
                list.Add(d.Value);
            }
            MdciService.MesRepository.DbContext.Insertable(list).AS($"{mdo.DestModule}").ExecuteCommand();
        }

        public static void Update(MessageDataObject mdo)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (var d in mdo.Data)
            {
                MdciService.MesRepository.DbContext.Updateable(d.Value).AS($"{mdo.DestModule}").ExecuteCommand();
            }
        }

        public static void AddRobotState() 
        {
            var data = new TB_EQUIP_STATE()
            {
                EQUIP_CODE = "",
                EQUIP_CODE_CNC = "",
                EQUIP_STATE = "",
                STATE_VALUE = 0.0M,
                SWTICH_TIME = DateTime.Now,
                CREATE_USER = "",
                FLAG = 1            
            };
            MdciService.MesRepository.DbContext.Insertable(data).ExecuteCommand();

            //var entityList = new List<Dictionary<string, object>>();
            //var entity = new Dictionary<string, object>
            //{
            //    ["EQUIP_CODE"] = "",
            //    ["EQUIP_CODE_CNC"] = "",
            //    ["EQUIP_STATE"] = "",
            //    ["STATE_VALUE"] = 0.0F,
            //    ["SWTICH_TIME"] = "",
            //    ["FLAG"] =1
            //};
            //entityList.Add(entity);
            //MdciService.MesRepository.DbContext.Insertable(entityList).AS($"EQUIP_STATE").ExecuteCommand();
        }
    }
}
