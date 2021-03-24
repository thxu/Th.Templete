using System;
using System.Collections.Generic;
using System.Text;
using Th.Templete.Domain.Demo;
using Th.Templete.Domain.IRepositories;
using Th.Templete.Domain.IRepositories.SqlSugarBase;
using Th.Templete.Repositories.SqlSugarBase;

namespace Th.Templete.Repositories
{
    public class DemoRepository : SqlSugarBaseRepository<DemoEntity>, IDemoRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        public DemoRepository(ISqlSugarUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
