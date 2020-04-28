using System;
using System.Collections.Generic;
using System.Text;
using KHBC.Core;
using SqlSugar;

namespace KHBC.DataAccess
{
    internal class DbContext
    {
        private DbConfModel DbConf;
        public SqlSugarClient Db { get; private set; }
        
        public Dictionary<string, int> DbTypeDict = new Dictionary<string, int>()
        {
            {"MYSQL", 0 },
            {"SQLSERVER ", 1},
            {"SQLITE", 2},
            {"ORACLE", 3},
            {"POSTGRESQL", 4}
        };

        public DbContext(DbConfModel conf)
        {
            DbConf = conf;
            GetConn();
        }

        private SqlSugarClient GetConn()
        {
            var dt = DbConf.DbType;
            if (!DbTypeDict.ContainsKey(dt.ToUpper()))
            {
                DBConf.WriteLog($"错误的数据库类型: {dt}");
            }

            var conn = new ConnectionConfig()
            {
                DbType = (DbType)DbTypeDict[dt.ToUpper()],
                ConnectionString = DbConf.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsShardSameThread = false,
                IsAutoCloseConnection = true
            };
            Db = new SqlSugarClient(conn);
            Db.Ado.CommandTimeOut = DbConf.CommandTimeOut;//设置超时时间

            Db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            {
#if DEBUG
                DBConf.WriteLog($"{sql}{Environment.NewLine} {GetString(pars)}{Environment.NewLine}[耗时 {Db.Ado.SqlExecutionTime.TotalSeconds * 1000}s ]");
#endif
                //sql执行超过3000ms  则记录
                if (Db.Ado.SqlExecutionTime.TotalSeconds > 3000)
                {
                    DBConf.WriteLog($"{sql}{Environment.NewLine} {GetString(pars)}{Environment.NewLine}[耗时 {Db.Ado.SqlExecutionTime.TotalSeconds * 1000}s ]");
                }
            };
            Db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {
                //var p = sql + "\r\n" + Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
            };
            Db.Aop.OnError = (ex) =>//执行SQL 错误事件
            {
                var pars = (ex.Parametres as SugarParameter[]) ?? new SugarParameter[] { };
                DBConf.WriteErrorLog($"msg:{ex.Message} sql:{ex.Sql}{Environment.NewLine}{GetString(pars)}");
            };

            Db.Aop.OnDiffLogEvent = it =>
            {
                var editBeforeData = it.BeforeData;
                var editAfterData = it.AfterData;
                var sql = it.Sql;
                var parameter = it.Parameters;
                var data = it.BusinessData;
                var time = it.Time;
                var diffType = it.DiffType;//枚举值 insert 、update 和 delete 用来作业务区分
            };
            Db.Aop.OnExecutingChangeSql = (sql, pars) => //SQL执行前 可以修改SQL
            {
                return new KeyValuePair<string, SugarParameter[]>(sql, pars);
            };

            return Db;
        }

        private string GetString(SugarParameter[] pars)
        {
            StringBuilder sbr = new StringBuilder();
            foreach (var i in pars)
            {
                sbr.Append($"{i.ParameterName}:{i.Value.ObjToString()},");
            }
            return sbr.ToString();
        }

    }
}

