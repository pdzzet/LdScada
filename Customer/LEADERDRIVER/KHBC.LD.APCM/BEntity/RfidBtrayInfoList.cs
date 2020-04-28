using System.Collections.Generic;

namespace KHBC.LD.APCM.BEntity
{
    internal class RfidBtrayInfoList
    {
        public string BtrayId1 { get; set; }
        public string BtrayId2 { get; set; } 
        public string BtrayId3 { get; set; }
        public List<RfidBtaryInfo> RfidBtaryInfos { get; set; } = new List<RfidBtaryInfo>();
    }
}
