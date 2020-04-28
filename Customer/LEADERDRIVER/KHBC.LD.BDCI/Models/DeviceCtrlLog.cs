using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.BDCI.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DEV_CLOG")]
	public class DeviceCtrlLog : BaseEntity
	{
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
        public int Id {get;set;}

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string PlantId {get;set;}

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string AssemblyLineId {get;set;}

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string DeviceId {get;set;}

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>        
        public DateTime CreateTime {get;set;}

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string Command {get;set;}

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public int? CmdResultId {get;set;}

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string CmdResultString {get;set;}

	}
}
