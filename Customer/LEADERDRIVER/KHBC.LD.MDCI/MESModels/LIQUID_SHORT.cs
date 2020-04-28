using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("LIQUID_SHORT")]
	public class TB_LIQUID_SHORT : BaseEntity
	{
        /// <summary>
        /// Desc:CNC号(Scada写入)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CNC_ID {get;set;}

        /// <summary>
        /// Desc:切削液类型(Scada写入)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CUT_FLUID_TYPE {get;set;}

        /// <summary>
        /// Desc:是否缺液(Scada写入)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string IS_LACK {get;set;}

        /// <summary>
        /// Desc:是否回抽
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string IS_BACK {get;set;}

        /// <summary>
        /// Desc:回抽液类型
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BACK_LIQUID_TYPE {get;set;}

        /// <summary>
        /// Desc:切削液溢出报警
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string IS_OVERFLOW {get;set;}

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
