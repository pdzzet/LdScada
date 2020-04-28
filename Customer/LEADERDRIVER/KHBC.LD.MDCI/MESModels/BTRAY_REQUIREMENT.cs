using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("BTRAY_REQUIREMENT")]
	public class TB_BTRAY_REQUIREMENT : BaseEntity
	{
        /// <summary>
        /// Desc:料箱号2(下料提供)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_ID1 {get;set;}

        /// <summary>
        /// Desc:料箱号2(下料提供)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_ID2 {get;set;}

        /// <summary>
        /// Desc:料箱号3(下料提供)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_ID3 {get;set;}

        /// <summary>
        /// Desc:料箱类型1(OK/NG)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_TYPE1 {get;set;}

        /// <summary>
        /// Desc:料箱类型2(OK/NG)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_TYPE2 {get;set;}

        /// <summary>
        /// Desc:料箱类型3(OK/NG)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_TYPE3 {get;set;}

        /// <summary>
        /// Desc:产线From
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE_FROM {get;set;}

        /// <summary>
        /// Desc:号位From
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string NO_FROM {get;set;}

        /// <summary>
        /// Desc:产线To(用于在制品在产线间的转运)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE_TO {get;set;}

        /// <summary>
        /// Desc:号位To
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string NO_TO {get;set;}

        /// <summary>
        /// Desc:订单号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ORDER_CODE {get;set;}

        /// <summary>
        /// Desc:工单号(ERP提供)
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
        /// Desc:派工单号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ASSIGN_CODE {get;set;}

        /// <summary>
        /// Desc:物料编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:物料状态(1-原材料/2-半成品/3-成品/4-在制品/5-空箱)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_STATE {get;set;}

        /// <summary>
        /// Desc:批次号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BATCH_NO {get;set;}

        /// <summary>
        /// Desc:数量(0， ERP出库数量(标示0))
        /// Default:
        /// Nullable:True
        /// </summary>        
        public long? STRAY_QTY {get;set;}

        /// <summary>
        /// Desc:配送类型(0-空箱出库/1-空箱入库/2-原材料出库/3-原材料退库/4-在制转运(线-线)/5-成品入库(满箱/工单完工))
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string B_STATE {get;set;}

        /// <summary>
        /// Desc:工艺路线(工艺路线信息)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ROUTE_INFO {get;set;}

        /// <summary>
        /// Desc:是否质检(0-不检测/1-需检测)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string QUALITY_STATE {get;set;}

        /// <summary>
        /// Desc:是否紧急出库(0-不紧急出库/1-紧急出库)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EMERGENCY_STATE {get;set;}

        /// <summary>
        /// Desc:标示(1-未开始/2-配送中/3-配送完成/4-异常结束)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string A_STATE {get;set;}

        /// <summary>
        /// Desc:数据写入方
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_USER {get;set;}

        /// <summary>
        /// Desc:异常原因
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EXCEPTION {get;set;}

        /// <summary>
        /// Desc:配送开始时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? S_TIME {get;set;}

        /// <summary>
        /// Desc:配送完成时间
        /// Default:
        /// Nullable:True
        /// </summary>        
        public DateTime? E_TIME {get;set;}

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
