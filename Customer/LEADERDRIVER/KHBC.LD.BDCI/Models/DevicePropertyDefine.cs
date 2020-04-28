using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.BDCI.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DEV_PROPDEF")]
	public class DevicePropertyDefine : BaseEntity
	{
        /// <summary>
        /// Desc:Id
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
        public int Id {get;set;}

        /// <summary>
        /// Desc:模块名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string ModuleName {get;set;}

        /// <summary>
        /// Desc:属性简称
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string Name {get;set;}

        /// <summary>
        /// Desc:属性全名
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string FullName {get;set;}

	}
}
