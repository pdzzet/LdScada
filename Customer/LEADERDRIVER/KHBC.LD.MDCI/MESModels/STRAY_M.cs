using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("STRAY_M")]
	public class TB_STRAY_M : BaseEntity
	{
        /// <summary>
        /// Desc:料盘号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string STRAY_ID {get;set;}

        /// <summary>
        /// Desc:料盘类型
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string STRAY_TYPE {get;set;}

        /// <summary>
        /// Desc:物料编号
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
        /// Desc:机台号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:产线编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE {get;set;}

        /// <summary>
        /// Desc:产品序列号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PRODUCT_SN {get;set;}

        /// <summary>
        /// Desc:位置顺序号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string SEQ_NO {get;set;}

        /// <summary>
        /// Desc:质量情况
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? QUALITY_STATE {get;set;}

        /// <summary>
        /// Desc:是否空料盘
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? IS_EMPTY {get;set;}

        /// <summary>
        /// Desc:是否完工
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? IS_DONE {get;set;}

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
