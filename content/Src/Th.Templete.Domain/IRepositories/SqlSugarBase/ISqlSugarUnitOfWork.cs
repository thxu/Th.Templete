using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace Th.Templete.Domain.IRepositories.SqlSugarBase
{
    /// <summary>
    /// 事务单元接口
    /// </summary>
    public interface ISqlSugarUnitOfWork
    {
        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        ISqlSugarClient GetDbClient();

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();
    }
}
