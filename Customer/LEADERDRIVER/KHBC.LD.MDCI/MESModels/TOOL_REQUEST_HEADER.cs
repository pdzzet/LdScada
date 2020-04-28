using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("TOOL_REQUEST_HEADER")]
	public class TB_TOOL_REQUEST_HEADER : BaseEntity
	{
        /// <summary>
        /// Desc:换刀请求单号(线体号+机台号+时间戳)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string REQ_SN {get;set;}

        /// <summary>
        /// Desc:工位派工单号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string DISPATCH_CODE {get;set;}

        /// <summary>
        /// Desc:物料编号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MATERIAL_CODE {get;set;}

        /// <summary>
        /// Desc:机台号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_CODE {get;set;}

        /// <summary>
        /// Desc:换刀原因(1-寿命到期，2-异常换刀，3-清线换刀)备注：清线换刀，不需要给出换刀明细
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? CHANGE_REASON {get;set;}

        /// <summary>
        /// Desc:请求状态(1-请求/2-出库/3-换刀完成)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? STATE {get;set;}

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
