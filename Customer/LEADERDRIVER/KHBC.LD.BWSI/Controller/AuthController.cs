using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KHBC.Core.Extend;
using KHBC.Core;
using Nancy;

namespace KHBC.LD.BWSI
{
    public class AuthController : IApiController
    {
        public string Name => "auth";

        public Result DoHttpWork(Request req)
        {
            var action = req.Query["action"]?.ToString().ToLower();
            if (action == "login")
            {
                return DoLogin(req);
            }
            else if (action == "logout")
            {
                return DoLogout(req);
            }

            return Result.Fail($"不支持该操作: {action}");
        }

        private Result DoLogin(Nancy.Request req)
        {
            return Result.Success("{UserName:'ldm', passwd:'1111'}");
        }

        private Result DoLogout(Nancy.Request req)
        {
            return Result.Success("");
        }
    }
}
