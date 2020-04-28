using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("TOOL_INFO")]
	public class TB_TOOL_INFO : BaseEntity
	{
        /// <summary>
        /// Desc:物料编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:物料名称
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_NAME {get;set;}

        /// <summary>
        /// Desc:机台号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:刀具名称
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string TOOL_NAME {get;set;}

        /// <summary>
        /// Desc:刀柄型号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string TOOL_SHANK_CODE {get;set;}

        /// <summary>
        /// Desc:刀片型号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string TOOL_CODE {get;set;}

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
