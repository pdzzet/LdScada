using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("QUALTY_RESULT_INFO")]
	public class TB_QUALTY_RESULT_INFO : BaseEntity
	{
        /// <summary>
        /// Desc:检测设备编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:工单号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string WORK_CODE {get;set;}

        /// <summary>
        /// Desc:子级工单号(MES拆单产生的子级工单号)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string WORK_CODE_1 {get;set;}

        /// <summary>
        /// Desc:检测任务编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string QUALITY_CODE {get;set;}

        /// <summary>
        /// Desc:工序编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_CODE {get;set;}

        /// <summary>
        /// Desc:参数编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PARA_CODE {get;set;}

        /// <summary>
        /// Desc:参数名称
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PARA_NAME {get;set;}

        /// <summary>
        /// Desc:参数值
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? PARA_VALUE {get;set;}

        /// <summary>
        /// Desc:检测值
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? INSP_VALUE {get;set;}

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? CREATE_DATE {get;set;}

        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_ID {get;set;}

        /// <summary>
        /// Desc:数据写入方
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_USER {get;set;}

        /// <summary>
        /// Desc:上偏差
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOL_UP {get;set;}

        /// <summary>
        /// Desc:下偏差
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOL_DOWN {get;set;}

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
