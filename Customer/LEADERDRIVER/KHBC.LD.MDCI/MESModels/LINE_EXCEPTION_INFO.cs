using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("LINE_EXCEPTION_INFO")]
	public class TB_LINE_EXCEPTION_INFO : BaseEntity
	{
        /// <summary>
        /// Desc:产线(生产线名称)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE {get;set;}

        /// <summary>
        /// Desc:异常发生时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? ABNORMALITY_TIME {get;set;}

        /// <summary>
        /// Desc:异常说明
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string INSTRUCTION {get;set;}

        /// <summary>
        /// Desc:设备编码(CNC机台设备编码)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIPMENT_CODE {get;set;}

        /// <summary>
        /// Desc:异常来源(异常发生的来源(如:堆垛机、输送线，CNC机台))
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string SOURCE {get;set;}

        /// <summary>
        /// Desc:数据写入方
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_USER {get;set;}

        /// <summary>
        /// Desc:报警解除时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? ABNORMALITY_ENDTIME {get;set;}

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
