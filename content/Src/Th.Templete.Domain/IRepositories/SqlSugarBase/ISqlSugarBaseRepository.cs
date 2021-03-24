using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Th.Templete.Domain.IRepositories.SqlSugarBase
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISqlSugarBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加表名映射
        /// </summary>
        /// <param name="modelName">实体名</param>
        /// <param name="tableName">表名</param>
        void AddMappingTable(string modelName, string tableName);

        #region Query
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="objId">主键id</param>
        /// <returns>实体</returns>
        Task<TEntity> QueryById(object objId);

        /// <summary>
        /// 根据主键集合查询
        /// </summary>
        /// <param name="objIds">主键集合</param>
        /// <returns>实体集合</returns>
        Task<List<TEntity>> QueryByIds(IList<object> objIds);

        /// <summary>
        /// 根据主键查询指定列数据
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="objId">主键id</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>返回对象</returns>
        Task<TResult> QueryById<TResult>(object objId, Expression<Func<TEntity, TResult>> selectExpression);

        /// <summary>
        /// 查询第一个实体
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>实体</returns>
        Task<TEntity> QueryFirst(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 查询第一个实体数据
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>实体</returns>
        Task<TResult> QueryFirst<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression);

        /// <summary>
        /// 查询第一个实体数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否顺序排序，默认为true，即按顺序查询</param>
        /// <returns>实体数据</returns>
        Task<TEntity> QueryFirst(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        /// <summary>
        /// 查询第一个实体数据
        /// </summary>
        /// <param name="strWhere">sql语句中的Where语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>实体数据</returns>
        Task<TEntity> QueryFirst(string strWhere, SugarParameter[] parameters = null);

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>实体数据集合</returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>对象集合</returns>
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression);

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>对象集合</returns>
        Task<List<TResult>> QueryWhereIF<T1, TResult>(List<Dictionary<bool, Expression<Func<T1, bool>>>> whereExpression, Expression<Func<T1, TResult>> selectExpression);

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <typeparam name="TResult">查询表达式中返回的对象</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="selectExpression">查询表达式</param>
        /// <returns>对象集合</returns>
        int QueryCount(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据表达式判断数据是否存在
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns></returns>
        Task<bool> Any(Expression<Func<TEntity, bool>> whereExpression);
        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否顺序排序，默认为true，即按顺序查询</param>
        /// <returns>实体数据集合</returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        /// <summary>
        /// 查询实体数据集合
        /// </summary>
        /// <param name="strWhere">sql语句中的Where语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>实体数据集合</returns>
        Task<List<TEntity>> Query(string strWhere, SugarParameter[] parameters = null);


        Task<List<TResult>> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new();

        Task<List<TResult>> QueryMuchByPaging<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            RefAsync<int> totalCount,
            int pageIndex = 0,
            int pageSize = 10,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new();

        Task<List<TResult>> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new();

        int QueryMuchCount<T1, T2>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new();

        Task<List<TResult>> QueryMuchByPaging<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression,
        Expression<Func<T1, T2, TResult>> selectExpression,
        RefAsync<int> totalCount,
        int pageIndex = 0,
        int pageSize = 10,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new();

        Task<List<TEntity>> QueryByPaging(Expression<Func<TEntity, bool>> whereExpression,
            RefAsync<int> totalCount,
            int pageIndex = 0,
            int pageSize = 10);

        Task<List<TResult>> QueryByPaging<TResult>(Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, TResult>> selectExpression,
            RefAsync<int> totalCount,
            int pageIndex = 0,
            int pageSize = 10);
        #endregion

        #region Add

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        Task<int> Add(TEntity entity);

        /// <summary>
        /// 添加数据并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>主键</returns>
        Task<int> AddAndReturnId(TEntity entity);


        /// <summary>
        /// 添加数据并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>主键</returns>
        Task<TEntity> AddAndReturnTEntity(TEntity entity);

        /// <summary>
        /// 添加实体指定列的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="insertColumns">要添加的列</param>
        /// <returns>受影响行数</returns>
        Task<int> AddWithColumns(TEntity entity, Expression<Func<TEntity, object>> insertColumns);

        /// <summary>
        /// 添加实体指定列的数据 并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="insertColumns">要添加的列</param>
        /// <returns>主键id</returns>
        Task<int> AddWithColumnsAndReturnId(TEntity entity, Expression<Func<TEntity, object>> insertColumns);

        /// <summary>
        /// 添加实体除了指定列的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">要忽略的列</param>
        /// <returns>受影响行数</returns>
        Task<int> AddWithoutColumns(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 添加实体除了指定列的数据 并返回自增主键
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">要忽略的列</param>
        /// <returns>主键id</returns>
        Task<int> AddWithoutColumnsAndReturnId(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响行数</returns>
        Task<int> Add(List<TEntity> entities);
        #endregion

        #region Update

        /// <summary>
        /// 根据实体更新（主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>更新结果</returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 更新数据（主键加锁）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateWithUpdLock(TEntity entity);

        /// <summary>
        /// 根据实体更新（主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="strSqlWhere">sql语句中的Where语句</param>
        /// <returns>更新结果</returns>
        Task<bool> Update(TEntity entity, string strSqlWhere);

        /// <summary>
        /// 只更新实体指定列（实体主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="updateColumns">要更新的列</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateWithColumns(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        /// 忽略指定列更新实体（实体主键要有值，主键是更新条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">要忽略的列</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateWithoutColumns(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="updateColumns">要更新的列</param>
        /// <param name="whereColumns">条件表达式</param>
        /// <returns>更新结果</returns>
        Task<bool> Update(Expression<Func<TEntity, TEntity>> updateColumns, Expression<Func<TEntity, bool>> whereColumns);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>更新结果</returns>
        Task<bool> Update(string strSql, SugarParameter[] parameters = null);

        #endregion
    }
}
