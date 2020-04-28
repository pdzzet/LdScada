using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("QUALITY_RESULT")]
	public class TB_QUALITY_RESULT : BaseEntity
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
        /// Desc:检测任务编号(QC+工单号+流水(6位))
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string QUALITY_CODE {get;set;}

        /// <summary>
        /// Desc:产线编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE {get;set;}

        /// <summary>
        /// Desc:物料编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:工序编码(工序交替进行)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_CODE {get;set;}

        /// <summary>
        /// Desc:产品序列号(MESNULL 检测机写入)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PRODUCT_SN {get;set;}

        /// <summary>
        /// Desc:是否合格(MES NULL 检测机写入1-合格OK/0-不合格NG)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? IS_QUALIFIED {get;set;}

        /// <summary>
        /// Desc:检测时间(检测完成时间)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? CHECK_TIME {get;set;}

        /// <summary>
        /// Desc:检测类型(抽检、在制品检、线头成品检等)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CHECK_TYPE {get;set;}

        /// <summary>
        /// Desc:检测状态(1-未检测/2-待检测/3-检测中/4-检测完成/5-料箱放回)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CHECK_STATE {get;set;}

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
