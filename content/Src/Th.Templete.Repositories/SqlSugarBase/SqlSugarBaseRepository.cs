using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Th.Templete.Domain.IRepositories.SqlSugarBase;

namespace Th.Templete.Repositories.SqlSugarBase
{
    /// <summary>
    /// SqlSugar仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SqlSugarBaseRepository<TEntity> : ISqlSugarBaseRepository<TEntity> where TEntity : class, new()
    {
        internal readonly ISqlSugarClient _sqlSugarClient;
        internal readonly ISqlSugarUnitOfWork _unitOfWork;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SqlSugarBaseRepository(ISqlSugarUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sqlSugarClient = _unitOfWork.GetDbClient();
        }

        /// <summary>
        /// 添加表名映射
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="tableName"></param>
        public void AddMappingTable(string modelName, string tableName)
        {
            _sqlSugarClient.MappingTables.Add(modelName, tableName);
        }

        #region Query
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="objId">主键id</param>
        /// <returns>实体</returns>
        public async Task<TEntity> QueryById(object objId)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .In(objId)
                .FirstAsync();
        }

        /// <summary>
        /// 根据主键集合查询
        /// </summary>
        /// <param name="objIds">主键集合</param>
        /// <returns>实体集合</returns>
        public async Task<TResult> QueryById<TResult>(object objId, Expression<Func<TEntity, TResult>> selectExpression)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .In(objId)
                .Select(selectExpression)
                .FirstAsync();
        }

        /// <summary>
        /// 查询第一个实体
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>实体</returns>
        public async Task<TEntity> QueryFirst(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .FirstAsync();
        }

        /// <summary>
        /// 查询第一个实体数据
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>实体</returns>
        public async Task<TResult> QueryFirst<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .Select(selectExpression)
                .FirstAsync();
        }

        /// <summary>
        /// 查询第一个实体数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否顺序排序，默认为true，即按顺序查询</param>
        /// <returns>实体数据</returns>
        public async Task<TEntity> QueryFirst(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc)
                .FirstAsync();
        }

        /// <summary>
        /// 查询第一个实体数据
        /// </summary>
        /// <param name="strWhere">sql语句中的Where语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>实体数据</returns>
        public async Task<TEntity> QueryFirst(string strWhere, SugarParameter[] parameters = null)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(!string.IsNullOrWhiteSpace(strWhere), strWhere, parameters)
                .FirstAsync();
        }

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>实体数据集合</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .ToListAsync();
        }

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>对象集合</returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .Select(selectExpression)
                .ToListAsync();
        }

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>对象集合</returns>
        public async Task<List<TResult>> QueryWhereIF<T1, TResult>(List<Dictionary<bool, Expression<Func<T1, bool>>>> whereExpression, Expression<Func<T1, TResult>> selectExpression)
        {
            var data = _sqlSugarClient.Queryable<T1>();
            if (whereExpression != null)
            {
                foreach (var item in whereExpression)

                    foreach (var temp in item)
                    {
                        data.WhereIF(temp.Key, temp.Value);
                    }
            }
            var dataRes = data.Select(selectExpression).ToListAsync();

            return await dataRes;
        }

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>对象集合</returns>
        public int QueryCount(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .Count();
        }
        /// <summary>
        /// 根据表达式判断数据是否存在
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns></returns>
        public async Task<bool> Any(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
               .AnyAsync(whereExpression);

        }
        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否顺序排序，默认为true，即按顺序查询</param>
        /// <returns>实体数据集合</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc)
                .ToListAsync();
        }

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <param name="strWhere">sql语句中的Where语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>实体数据集合</returns>
        public async Task<List<TEntity>> Query(string strWhere, SugarParameter[] parameters = null)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(!string.IsNullOrWhiteSpace(strWhere), strWhere, parameters)
                .ToListAsync();
        }

        public async Task<List<TResult>> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new()
        {
            if (whereLambda == null)
            {
                return await _sqlSugarClient.Queryable(joinExpression)
                    .Select(selectExpression)
                    .ToListAsync();
            }
            return await _sqlSugarClient.Queryable(joinExpression)
                .Where(whereLambda)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<List<TResult>> QueryMuchByPaging<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression, RefAsync<int> totalCount,
            int pageIndex = 0, int pageSize = 10, Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new()
        {
            if (whereLambda == null)
            {
                return await _sqlSugarClient.Queryable(joinExpression)
                    .Select(selectExpression)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            return await _sqlSugarClient.Queryable(joinExpression)
                .Where(whereLambda)
                .Select(selectExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
        }

        public async Task<List<TResult>> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
        {
            if (whereLambda == null)
            {
                return await _sqlSugarClient.Queryable(joinExpression)
                    .Select(selectExpression)
                    .ToListAsync();
            }
            return await _sqlSugarClient.Queryable(joinExpression)
                .Where(whereLambda)
                .Select(selectExpression)
                .ToListAsync();
        }
        public int QueryMuchCount<T1, T2>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
        {
            if (whereLambda == null)
            {
                return _sqlSugarClient.Queryable(joinExpression)
                    .Count();
            }
            return _sqlSugarClient.Queryable(joinExpression)
                .Where(whereLambda)
               .Count();
        }


        public async Task<List<TResult>> QueryMuchByPaging<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, RefAsync<int> totalCount,
            int pageIndex = 0, int pageSize = 10, Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
        {
            if (whereLambda == null)
            {
                return await _sqlSugarClient.Queryable(joinExpression)
                    .Select(selectExpression)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            return await _sqlSugarClient.Queryable(joinExpression)
                .Where(whereLambda)
                .Select(selectExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
        }

        /// <summary>
        /// 根据主键集合查询
        /// </summary>
        /// <param name="objIds">主键集合</param>
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> QueryByIds(IList<object> objIds)
        {
            return await _sqlSugarClient.Queryable<TEntity>()
                .In(objIds)
                .ToListAsync();
        }

        public Task<List<TEntity>> QueryByPaging(Expression<Func<TEntity, bool>> whereExpression, RefAsync<int> totalCount, int pageIndex = 0, int pageSize = 10)
        {
            return _sqlSugarClient.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
        }

        public Task<List<TResult>> QueryByPaging<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression, RefAsync<int> totalCount, int pageIndex = 0, int pageSize = 10)
        {
            return _sqlSugarClient.Queryable<TEntity>()
               .WhereIF(whereExpression != null, whereExpression)
               .Select(selectExpression)
               .ToPageListAsync(pageIndex, pageSize, totalCount);
        }
        #endregion

        #region Add
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        public async Task<int> Add(TEntity entity)
        {
            return await _sqlSugarClient.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加数据并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>主键</returns>
        public async Task<int> AddAndReturnId(TEntity entity)
        {
            return await _sqlSugarClient.Insertable(entity).ExecuteReturnIdentityAsync();
        }
        /// <summary>
        /// 添加数据并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>主键</returns>
        public async Task<TEntity> AddAndReturnTEntity(TEntity entity)
        {
            return await _sqlSugarClient.Insertable(entity).ExecuteReturnEntityAsync();
        }

        /// <summary>
        /// 添加实体指定列的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="insertColumns">要添加的列</param>
        /// <returns>受影响行数</returns>
        public async Task<int> AddWithColumns(TEntity entity, Expression<Func<TEntity, object>> insertColumns)
        {
            return await _sqlSugarClient.Insertable(entity).InsertColumns(insertColumns).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加实体指定列的数据 并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="insertColumns">要添加的列</param>
        /// <returns>主键id</returns>
        public async Task<int> AddWithColumnsAndReturnId(TEntity entity, Expression<Func<TEntity, object>> insertColumns)
        {
            return await _sqlSugarClient.Insertable(entity).InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 添加实体除了指定列的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">要忽略的列</param>
        /// <returns>受影响行数</returns>
        public async Task<int> AddWithoutColumns(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await _sqlSugarClient.Insertable(entity).IgnoreColumns(ignoreColumns).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加实体除了指定列的数据 并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">要忽略的列</param>
        /// <returns>主键id</returns>
        public async Task<int> AddWithoutColumnsAndReturnId(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await _sqlSugarClient.Insertable(entity).IgnoreColumns(ignoreColumns).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响行数</returns>
        public async Task<int> Add(List<TEntity> entities)
        {
            return await _sqlSugarClient.Insertable(entities).ExecuteCommandAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// 根据实体更新（主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>更新结果</returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await _sqlSugarClient.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 更新数据（主键加锁）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateWithUpdLock(TEntity entity)
        {
            return await _sqlSugarClient.Updateable(entity)
                       .With(SqlWith.UpdLock)
                       .ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 根据实体更新（主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="strSqlWhere">sql语句中的Where语句</param>
        /// <returns>更新结果</returns>
        public async Task<bool> Update(TEntity entity, string strSqlWhere)
        {
            return await _sqlSugarClient.Updateable(entity)
                       .Where(strSqlWhere)
                       .ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 只更新实体指定列（实体主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="updateColumns">要更新的列</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateWithColumns(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            return await _sqlSugarClient.Updateable(entity)
                       .UpdateColumns(updateColumns)
                       .ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 忽略指定列更新实体（实体主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">要忽略的列</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateWithoutColumns(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await _sqlSugarClient.Updateable(entity)
                       .IgnoreColumns(ignoreColumns)
                       .ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="updateColumns">要更新的列</param>
        /// <param name="whereColumns">条件表达式</param>
        /// <returns>更新结果</returns>
        public async Task<bool> Update(Expression<Func<TEntity, TEntity>> updateColumns, Expression<Func<TEntity, bool>> whereColumns)
        {
            return await _sqlSugarClient.Updateable<TEntity>()
                       .SetColumns(updateColumns)
                       .Where(whereColumns)
                       .ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>更新结果</returns>
        public async Task<bool> Update(string strSql, SugarParameter[] parameters = null)
        {
            return await _sqlSugarClient.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }
        #endregion
    }
}
