using System;
using System.Linq;
using System.Text;
using KHBC.Core;

namespace KHBC.LD.BWSI.BcSysModels
{
    ///<summary>
    ///
    ///</summary>
    [SqlSugar.SugarTable("SYS_RolesMenus")]
    public class TBL_SYS_RolesMenus : BaseEntity
	{
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long menu_id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long role_id {get;set;}

    }
}
