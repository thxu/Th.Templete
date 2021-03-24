using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Th.Templete.Domain.Demo;

namespace Th.Templete.Application.Demo
{
    public interface IDemo
    {
        Task<DemoEntity> GetFirst(int id);

        bool TransactionDemo(int someParam);
    }
}
