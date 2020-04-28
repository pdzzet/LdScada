using KHBC.Core;
using KHBC.LD.BDCI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.BDCI
{
    public class DeviceDataInfoController
    {
        /// <summary>
        /// Add deviceinfo
        /// </summary>
        /// <param name="mdo"></param>
        public static void Add(MessageDataObject mdo)
        {
            var entityList = new List<Dictionary<string, object>>();

            foreach (var d in mdo.Data)
            {
                foreach (var p in d.Value)
                {
                    int propertyId = 0;

                    if (!BdciService.DevicePropertyDefineMap.ContainsKey(mdo.ModuleName))
                    {
                        BdciService.DevicePropertyDefineMap[mdo.ModuleName] = new Dictionary<string, DevicePropertyDefine>();
                    }

                    if (BdciService.DevicePropertyDefineMap[mdo.ModuleName].ContainsKey(p.Key))
                    {
                        propertyId = BdciService.DevicePropertyDefineMap[mdo.ModuleName][p.Key].Id;
                    }
                    else
                    {
                        var dp = new DevicePropertyDefine
                        {
                            ModuleName = mdo.ModuleName,
                            Name = p.Key
                        };

                        propertyId = BdciService.BcRepository.DbContext.Insertable<DevicePropertyDefine>(dp).ExecuteReturnEntity().Id;
                        BdciService.DevicePropertyDefineMap[mdo.ModuleName][p.Key] = new DevicePropertyDefine
                        {
                            Id = propertyId,
                            ModuleName = mdo.DestModule,
                            Name = p.Key
                        };
                    }

                    var entity = new Dictionary<string, object>
                    {
                        ["CreateTime"] = mdo.CreateTime,
                        ["PlantId"] = mdo.PlantId,
                        ["AssemblyLineId"] = mdo.AssemblyLineId,
                        ["DeviceId"] = d.Key,
                        ["PropertyId"] = propertyId,
                        ["PropertyValue"] = p.Value
                    };

                    entityList.Add(entity);
                }
            }

            BdciService.BcRepository.DbContext.Insertable(entityList).AS($"DEV_{mdo.DestModule}").ExecuteCommand();
        }
        /// <summary>
        /// Update deviceinfo
        /// </summary>
        /// <param name="mdo"></param>
        public static void Update(MessageDataObject mdo)
        {
            var entityList = new List<Dictionary<string, object>>();

            foreach (var d in mdo.Data)
            {
                foreach (var p in d.Value)
                {
                    int propertyId = 0;

                    if (!BdciService.DevicePropertyDefineMap.ContainsKey(mdo.ModuleName))
                    {
                        BdciService.DevicePropertyDefineMap[mdo.ModuleName] = new Dictionary<string, DevicePropertyDefine>();
                    }

                    if (BdciService.DevicePropertyDefineMap[mdo.ModuleName].ContainsKey(p.Key))
                    {
                        propertyId = BdciService.DevicePropertyDefineMap[mdo.ModuleName][p.Key].Id;
                    }
                    else
                    {
                        var dp = new DevicePropertyDefine
                        {
                            ModuleName = mdo.ModuleName,
                            Name = p.Key
                        };

                        propertyId = BdciService.BcRepository.DbContext.Insertable<DevicePropertyDefine>(dp).ExecuteReturnEntity().Id;
                        BdciService.DevicePropertyDefineMap[mdo.ModuleName][p.Key] = new DevicePropertyDefine
                        {
                            Id = propertyId,
                            ModuleName = mdo.DestModule,
                            Name = p.Key
                        };
                    }

                    var entity = new Dictionary<string, object>
                    {
                        ["CreateTime"] = mdo.CreateTime,
                        ["PlantId"] = mdo.PlantId,
                        ["AssemblyLineId"] = mdo.AssemblyLineId,
                        ["DeviceId"] = d.Key,
                        ["PropertyId"] = propertyId,
                        ["PropertyValue"] = p.Value
                    };

                    entityList.Add(entity);
                }
            }

            //BdciService.BcRepository.DbContext.Insertable(entityList).AS($"DEV_{mdo.DestModule}").ExecuteCommand();
            BdciService.BcRepository.DbContext.Updateable(entityList).AS($"REL_{mdo.DestModule}").ExecuteCommand();
        }
    }
}
