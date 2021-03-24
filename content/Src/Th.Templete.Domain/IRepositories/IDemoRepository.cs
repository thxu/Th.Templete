using System;
using System.Collections.Generic;
using System.Text;
using Th.Templete.Domain.Demo;
using Th.Templete.Domain.IRepositories.SqlSugarBase;

namespace Th.Templete.Domain.IRepositories
{
    public interface IDemoRepository : ISqlSugarBaseRepository<DemoEntity>
    {
    }
}
