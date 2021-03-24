using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;
using Th.Templete.Domain.IRepositories.SqlSugarBase;

namespace Th.Templete.Repositories.SqlSugarBase
{
    /// <summary>
    /// SqlSugar事务单元
    /// </summary>
    public class SqlSugarUnitOfWork : ISqlSugarUnitOfWork
    {
        /// <summary>
        /// SqlSugar客户端
        /// </summary>
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        public SqlSugarUnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;

            //_sqlSugarClient.Aop.OnLogExecuting = (sql, parameters) =>
            //{
            //    Console.WriteLine($"sql={sql},param={GetParas(parameters)}");
            //};
        }

        /// <summary>
        /// 解析sql参数
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        private string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}{Environment.NewLine}";
            }

            return key;
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        public ISqlSugarClient GetDbClient()
        {
            return _sqlSugarClient;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTran()
        {
            _sqlSugarClient.Ado.BeginTran();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran()
        {
            try
            {
                _sqlSugarClient.Ado.CommitTran();
            }
            catch (Exception e)
            {
                _sqlSugarClient.Ado.RollbackTran();
                throw e;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTran()
        {
            _sqlSugarClient.Ado.RollbackTran();
        }
    }
}
