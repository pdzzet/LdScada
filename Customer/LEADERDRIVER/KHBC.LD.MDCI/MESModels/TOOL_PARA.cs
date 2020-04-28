using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("TOOL_PARA")]
	public class TB_TOOL_PARA : BaseEntity
	{
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
        /// Desc:刀具RFID编码
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string TOOL_RFID {get;set;}

        /// <summary>
        /// Desc:机台刀具编号(刀具在对应的机台里的T Number)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string EQUIP_TOOL_CODE {get;set;}

        /// <summary>
        /// Desc:刀具加工次数(默认0，累计产品加工次数)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOOL__PROC_TIMES {get;set;}

        /// <summary>
        /// Desc:刀具加工时长(默认0，累计加工时间(以秒为单位的数值))
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOOL__TOTAL_TIME {get;set;}

        /// <summary>
        /// Desc:刀具补刀次数(默认0，累计补刀次数)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? TOOL__ADJUST_TIMES {get;set;}

        /// <summary>
        /// Desc:外形H
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? SHAPE_H {get;set;}

        /// <summary>
        /// Desc:外形D
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? SHAPE_D {get;set;}

        /// <summary>
        /// Desc:外形R角
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? R_ANGLE {get;set;}

        /// <summary>
        /// Desc:外形X(0，针对车床刀具有效)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? COMPE_X {get;set;}

        /// <summary>
        /// Desc:外形Z(0，针对车床刀具有效)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? COMPE_Z {get;set;}

        /// <summary>
        /// Desc:象限P(针对车床刀具有效)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public decimal? COMPE_P {get;set;}

        /// <summary>
        /// Desc:刀片状态(1-在线/2-离线)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? STATE {get;set;}

        /// <summary>
        /// Desc:换刀原因(1-OK/2-断刀/3-寿命周期结束/4-清线)
        /// Default:
        /// Nullable:True
        /// </summary>        
        public short? REASON {get;set;}

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
