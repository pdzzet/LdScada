using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("PROD_REPORT")]
	public class TB_PROD_REPORT : BaseEntity
	{
        /// <summary>
        /// Desc:机台号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:生产派工单
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ASSIGN_CODE {get;set;}

        /// <summary>
        /// Desc:产品编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:工序编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_CODE {get;set;}

        /// <summary>
        /// Desc:产品序列号/批次号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PRODUCT_SN {get;set;}

        /// <summary>
        /// Desc:批次号(新增，由SCADA通过序列号匹配料箱明细表给出)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BATCH_NO {get;set;}

        /// <summary>
        /// Desc:报工数
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? QTY {get;set;}

        /// <summary>
        /// Desc:开工时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? START_TIME {get;set;}

        /// <summary>
        /// Desc:完工时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? END_TIME {get;set;}

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
