using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace KHBC.LD.BDCI.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DEV_FCCM")]
	public class FccmDataInfo : BaseEntity
	{
        /// <summary>
        /// Desc:Id
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
        public int Id {get;set;}

        /// <summary>
        /// Desc:创建时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public DateTime? CreateTime {get;set;}

        /// <summary>
        /// Desc:车间ID
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string PlantId {get;set;}

        /// <summary>
        /// Desc:产线ID
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string AssemblyLineId {get;set;}

        /// <summary>
        /// Desc:设备ID
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string DeviceId {get;set;}

        /// <summary>
        /// Desc:属性ID
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public int? PropertyId {get;set;}

        /// <summary>
        /// Desc:属性值
        /// Default:NULL
        /// Nullable:True
        /// </summary>        
        public string PropertyValue {get;set;}

	}
}
