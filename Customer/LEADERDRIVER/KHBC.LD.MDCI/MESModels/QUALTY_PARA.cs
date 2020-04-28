using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("QUALTY_PARA")]
	public class TB_QUALTY_PARA : BaseEntity
	{
        /// <summary>
        /// Desc:物料编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:物料版本
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_VER {get;set;}

        /// <summary>
        /// Desc:工序编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_CODE {get;set;}

        /// <summary>
        /// Desc:工艺路线编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ROUTE_CODE {get;set;}

        /// <summary>
        /// Desc:工艺路线版本号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string ROUTE_VER {get;set;}

        /// <summary>
        /// Desc:质检编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string QUALITY_C {get;set;}

        /// <summary>
        /// Desc:质检版本
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string QUALITY_VER {get;set;}

        /// <summary>
        /// Desc:检验步骤
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string INSPECT_STEP {get;set;}

        /// <summary>
        /// Desc:检测项目
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string INSPECTION_ITEM {get;set;}

        /// <summary>
        /// Desc:参数名称(检测内容)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PARA_NAME {get;set;}

        /// <summary>
        /// Desc:目标值(判定标准)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string T_VALUE {get;set;}

        /// <summary>
        /// Desc:控制上限
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string UPPER_CL {get;set;}

        /// <summary>
        /// Desc:控制下限
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LOWER_CL {get;set;}

        /// <summary>
        /// Desc:上公差
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOL_UP {get;set;}

        /// <summary>
        /// Desc:下公差
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOL_DOWN {get;set;}

        /// <summary>
        /// Desc:附件路径
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string OP_ROUTE {get;set;}

        /// <summary>
        /// Desc:检验方法
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string INSPECT_METHOD {get;set;}

        /// <summary>
        /// Desc:检验工具
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string INSPECT_TOOL {get;set;}

        /// <summary>
        /// Desc:检验频率(由PLM写入如：3PCS/首末件/夹/机1PCS/1H/夹/机)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string INSPECT_FREQUENCY {get;set;}

        /// <summary>
        /// Desc:抽样方案(由PLM写入，如：X/每夹/机)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string SAMPLING_PLAN {get;set;}

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string REMARKS {get;set;}

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
