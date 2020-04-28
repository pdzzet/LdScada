using System;
using System.Linq;
using System.Text;
using KHBC.Core;

namespace KHBC.LD.BWSI.BcSysModels
{
    ///<summary>
    ///
    ///</summary>
    [SqlSugar.SugarTable("SYS_Permission")]
    public class TBL_SYS_Permission : BaseEntity
	{
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long Id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Alias {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Pid {get;set;}

    }
}
