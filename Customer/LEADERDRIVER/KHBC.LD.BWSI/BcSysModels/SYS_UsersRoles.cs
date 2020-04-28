using System;
using System.Linq;
using System.Text;
using KHBC.Core;

namespace KHBC.LD.BWSI.BcSysModels
{
    ///<summary>
    ///
    ///</summary>
    [SqlSugar.SugarTable("SYS_UsersRoles")]
    public class TBL_SYS_UsersRoles : BaseEntity
	{
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long UserId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long RoleId {get;set;}

    }
}
