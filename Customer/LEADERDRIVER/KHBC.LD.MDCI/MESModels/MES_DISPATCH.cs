using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("MES_DISPATCH")]
	public class TB_MES_DISPATCH : BaseEntity
	{
        /// <summary>
        /// Desc:生产订单(ERP订单号)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ORDER_CODE {get;set;}

        /// <summary>
        /// Desc:ERP工单号(直接由ERP产出)
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
        /// Desc:工位派工单号(查询条件)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string DISPATCH_CODE {get;set;}

        /// <summary>
        /// Desc:首单(0-否/1-是)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string FIRST_ORDER {get;set;}

        /// <summary>
        /// Desc:尾单(0-否/1-是)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LAST_ORDER {get;set;}

        /// <summary>
        /// Desc:物料编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:生产数量
        /// Default:
        /// Nullable:True
        /// </summary>        
        public long? QTY {get;set;}

        /// <summary>
        /// Desc:计划开始时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? PLAN_START_DATE {get;set;}

        /// <summary>
        /// Desc:计划完成时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? PLAN_END_DATE {get;set;}

        /// <summary>
        /// Desc:NC编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string NC_ID {get;set;}

        /// <summary>
        /// Desc:NC版本号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string NC_VER {get;set;}

        /// <summary>
        /// Desc:作业指导书
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_DOC {get;set;}

        /// <summary>
        /// Desc:作业指导版本号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_DOC_VER {get;set;}

        /// <summary>
        /// Desc:NC路径(NC_PATH)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string NC_PATH {get;set;}

        /// <summary>
        /// Desc:作业指导书路径(OP_DOC_PATH)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_DOC_PATH {get;set;}

        /// <summary>
        /// Desc:工序编码
        /// Default:
        /// Nullable:False
        /// </summary>        
        public string OP_CODE {get;set;}

        /// <summary>
        /// Desc:设备编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:产线编号(线体编号，不提供)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EPLINE_CODE {get;set;}

        /// <summary>
        /// Desc:父级派工单号(取消)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ASSIGN_CODE_1 {get;set;}

        /// <summary>
        /// Desc:数据写入方
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_USER {get;set;}

        /// <summary>
        /// Desc:读取标识(0-未读取/1-已读取)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? IS_READ {get;set;}

        /// <summary>
        /// Desc:线体
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE {get;set;}

        /// <summary>
        /// Desc:工单状态(0-待生产，1-在制，2-完工，3-已入库， 4-计划变更冻结，5-设计变更冻结,6-一般质量异常冻结，7-严重质量异常冻结,8-异常完工，9-异常入库,10-手动撤销)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? STATE {get;set;}

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
