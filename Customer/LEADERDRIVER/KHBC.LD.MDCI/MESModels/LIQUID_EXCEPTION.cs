using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.MDCI.MesModels
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("LIQUID_EXCEPTION")]
	public class TB_LIQUID_EXCEPTION : BaseEntity
	{
        /// <summary>
        /// Desc:供液系统号
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string SYS_NO {get;set;}

        /// <summary>
        /// Desc:一号净水过滤网堵塞
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PIP_NO1 {get;set;}

        /// <summary>
        /// Desc:二号净水过滤网堵塞
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PIP_NO2 {get;set;}

        /// <summary>
        /// Desc:纸带冲刷机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string TAPE_EXCEPTION {get;set;}

        /// <summary>
        /// Desc:抽污水电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PUMP_EXCEPTION1 {get;set;}

        /// <summary>
        /// Desc:抽净水电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string PUMP_EXCEPTION2 {get;set;}

        /// <summary>
        /// Desc:毛刷电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string BRUSH_EXCEPTION {get;set;}

        /// <summary>
        /// Desc:纸带过滤机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string FILTER_EXCEPTION1 {get;set;}

        /// <summary>
        /// Desc:二级过滤泵异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string FILTER_EXCEPTION2 {get;set;}

        /// <summary>
        /// Desc:油水分离机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string SEPERATE_EXCEPTION {get;set;}

        /// <summary>
        /// Desc:抽原液电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string STOSTE_EXCEPTION {get;set;}

        /// <summary>
        /// Desc:一号供液电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LIQUID_EXCEPTION1 {get;set;}

        /// <summary>
        /// Desc:二号供液电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string LIQUID_EXCEPTION2 {get;set;}

        /// <summary>
        /// Desc:搅拌电机异常
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string MIX_EXCEPTION {get;set;}

        /// <summary>
        /// Desc:备用
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string UDA1 {get;set;}

        /// <summary>
        /// Desc:备用
        /// Default:
        /// Nullable:True
        /// </summary>        
        public string UDA2 {get;set;}

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
