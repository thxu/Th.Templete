using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Th.Templete.Domain.Demo;
using Th.Templete.Domain.IRepositories;
using Th.Templete.Domain.IRepositories.SqlSugarBase;

namespace Th.Templete.Application.Demo.Impl
{
    public class Demo : IDemo
    {
        private readonly IDemoRepository _demoDb = null;
        private readonly ISqlSugarUnitOfWork _unitOfWork = null;

        public Demo(IDemoRepository demoDb, ISqlSugarUnitOfWork unitOfWork)
        {
            _demoDb = demoDb;
            _unitOfWork = unitOfWork;
        }

        public async Task<DemoEntity> GetFirst(int id)
        {
            return await _demoDb.QueryFirst(n => n.Id == 1);
        }

        public bool TransactionDemo(int someParam)
        {
            _unitOfWork.BeginTran();
            try
            {
                // do something

                if (true)
                {
                    // if error is true
                    _unitOfWork.RollbackTran();
                }

                _unitOfWork.CommitTran();
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
            }

            return false;
        }
    }
}
