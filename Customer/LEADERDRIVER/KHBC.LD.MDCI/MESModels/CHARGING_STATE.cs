using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("CHARGING_STATE")]
	public class TB_CHARGING_STATE : BaseEntity
	{
        /// <summary>
        /// Desc:产线编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LINE {get;set;}

        /// <summary>
        /// Desc:号位From(5)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string NO_FROM {get;set;}

        /// <summary>
        /// Desc:料箱号1
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_ID1 {get;set;}

        /// <summary>
        /// Desc:料箱号2
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_ID2 {get;set;}

        /// <summary>
        /// Desc:料箱号3
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BTRAY_ID3 {get;set;}

        /// <summary>
        /// Desc:数量
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? QTY {get;set;}

        /// <summary>
        /// Desc:物料号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MRL_CODE {get;set;}

        /// <summary>
        /// Desc:读取标识(0-未读取/1-已读取)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? IS_READ {get;set;}

        /// <summary>
        /// Desc:数据写入方
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string CREATE_USER {get;set;}

        /// <summary>
        /// Desc:上下料标识(0-上料请求/1-下料请求/2-空箱请求)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? DISPATCH_STATE {get;set;}

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
