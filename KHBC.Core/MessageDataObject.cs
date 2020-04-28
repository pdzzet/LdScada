using System;
using System.Collections.Generic;

namespace KHBC.Core
{
    public class MessageDataObject
    {
        public string ModuleName { get; set; } = "";
        public string ServiceName { get; set; } = "";
        public string DestModule { get; set; } = "";
        public string ActionName { get; set; } = "";

        public string PlantId { get; set; } = "";
        public string AssemblyLineId { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public Dictionary<string, Dictionary<string, object>> Data { get; set; } = new Dictionary<string, Dictionary<string, object>>();
    }
}
