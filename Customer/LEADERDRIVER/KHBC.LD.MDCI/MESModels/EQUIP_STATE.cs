using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("EQUIP_STATE")]
	public class TB_EQUIP_STATE : BaseEntity
	{
        /// <summary>
        /// Desc:设备号(机台、PLC等)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:SN号(新增(CNC))
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE_CNC {get;set;}

        /// <summary>
        /// Desc:设备状态(开机、停机、运行、故障、故障解除，可扩展；注：STATE_VALUE为3时，写入具体的故障描述；)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_STATE {get;set;}

        /// <summary>
        /// Desc:设备状态值(浮点0:Waiting/-1:OffLine/1:Running/3:Stop)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? STATE_VALUE {get;set;}

        /// <summary>
        /// Desc:切换时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? SWTICH_TIME {get;set;}

        /// <summary>
        /// Desc:数据写入方
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_USER {get;set;}

        /// <summary>
        /// Desc:操作标示:1-新增;2-修改;3-删除;默认1
        /// Default:
        /// Nullable:False
        /// </summary>        
        public short FLAG {get;set;}

        /// <summary>
        /// Desc:Oracle自动生成
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey=true)]
        public long SN {get;set;}

        /// <summary>
        /// Desc:日期:Oracle自动生成
        /// Default:
        /// Nullable:False
        /// </summary>        
        public string MARK_DATE {get;set;}

        /// <summary>
        /// Desc:时间戳:Oracle自动生成
        /// Default:
        /// Nullable:False
        /// </summary>        
        public DateTime MARK_TIME {get;set;}

	}
}
