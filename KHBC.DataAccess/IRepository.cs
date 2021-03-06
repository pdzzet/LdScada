﻿using KHBC.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace KHBC.DataAccess
{
    /// <summary>
    /// 仓储层基接口
    /// </summary>where T : BaseEntity,new();
    public interface IRepository
    {
            #region 事务

            /// <summary>
            /// 初始化事务
            /// </summary>
            /// <param name="level"></param>
            void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted);

            /// <summary>
            /// 完成事务
            /// </summary>
            void CommitTran();

            /// <summary>
            /// 完成事务
            /// </summary>
            void RollbackTran();
            #endregion

            #region 新增 

            /// <summary>
            /// 新增
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entity"> 实体对象 </param> 
            /// <param name="isLock">是否加锁</param>
            /// <returns>新增是否成功</returns>
            bool Add<T>(T entity, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 新增
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entitys">泛型集合</param>
            /// <param name="isLock">是否加锁</param>
            /// <returns>新增是否成功</returns>
            bool Add<T>(List<T> entitys, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 新增
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entity"> 实体对象 </param> 
            /// <param name="isLock">是否加锁</param>
            /// <returns>返回实体</returns>
            T AddReturnEntity<T>(T entity, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 新增
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entity"> 实体对象 </param> 
            /// <param name="isLock">是否加锁</param>
            /// <returns>返回Id</returns>
            int AddReturnId<T>(T entity, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 新增
            /// </summary> 
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entity"> 实体对象 </param> 
            /// <param name="isLock">是否加锁</param>
            /// <returns>返回bool, 并将identity赋值到实体</returns>
            bool AddReturnBool<T>(T entity, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 新增
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entitys">泛型集合</param>
            /// <param name="isLock">是否加锁</param>
            /// <returns>返回bool, 并将identity赋值到实体</returns>
            bool AddReturnBool<T>(List<T> entitys, bool isLock = false) where T : BaseEntity,new();

            #endregion

            #region 修改 

            /// <summary>
            /// 修改数据源
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
            /// <returns>数据源</returns>
            IUpdateable<T> Updateable<T>() where T : BaseEntity,new();

            /// <summary>
            /// 修改（主键是更新条件）
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entity"> 实体对象 </param> 
            /// <param name="isLock"> 是否加锁 </param> 
            /// <returns>修改是否成功</returns>
            bool Update<T>(T entity, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 修改
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="update"> 实体对象 </param> 
            /// <param name="where"> 条件 </param> 
            /// <param name="isLock"> 是否加锁 </param> 
            /// <returns>修改是否成功</returns>
            bool Update<T>(Expression<Func<T, T>> update, Expression<Func<T, bool>> where, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 修改（主键是更新条件）
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entitys"> 实体对象集合 </param> 
            /// <param name="isLock"> 是否加锁 </param> 
            /// <returns>修改是否成功</returns>
            bool Update<T>(List<T> entitys, bool isLock = false) where T : BaseEntity,new();

        #endregion

            #region 删除

            bool Delete<T>(List<T> list, bool isLock = false) where T : BaseEntity, new();
            /// <summary>
            /// 删除
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="entity"> 实体对象 </param> 
            /// <param name="isLock"> 是否加锁 </param> 
            /// <returns>删除是否成功</returns>
            bool Delete<T>(T entity, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 删除
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="where"> 条件 </param> 
            /// <param name="isLock"> 是否加锁 </param> 
            /// <returns>删除是否成功</returns>
            bool Delete<T>(Expression<Func<T, bool>> where, bool isLock = false) where T : BaseEntity,new();
            /// <summary>
            /// 删除所有
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="isLock"> 是否加锁 </param> 
            /// <returns></returns>
            bool DeleteAll<T>(bool isLock = false) where T : BaseEntity,new();
            /// <summary>
            /// 根据主键物理删除实体对象
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="id">主键</param>
            ///<param name="isLock"> 是否加锁 </param> 
            /// <returns>删除是否成功</returns>
            bool DeleteById<T>(dynamic id, bool isLock = false) where T : BaseEntity,new();

            /// <summary>
            /// 根据主键批量物理删除实体集合
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="ids">主键集合</param>
            ///<param name="isLock"> 是否加锁 </param> 
            /// <returns>删除是否成功</returns>
            bool DeleteByIds<T>(dynamic[] ids, bool isLock = false) where T : BaseEntity,new();

            #endregion

            #region 查询

            /// <summary>
            /// 查询数据源
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
            /// <returns>数据源</returns>
            ISugarQueryable<T> Queryable<T>() where T : BaseEntity,new();

            /// <summary>
            /// 查询集合
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <returns>实体</returns>
            List<T> GetList<T>() where T : BaseEntity,new();

            /// <summary>
            /// 查询集合
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="whereLambda">查询表达式</param>
            /// <param name="orderbyLambda">排序表达式</param> 
            /// <param name="isAsc">是否升序</param> 
            /// <returns>实体</returns>
            List<T> GetList<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderbyLambda = null, bool isAsc = true) where T : BaseEntity,new();

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="parameters"></param>
        /// <returns>实体</returns>
        List<T> GetList<T>(string sql, object parameters) where T : class, new();

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="parameters"></param>
        /// <returns>实体</returns>
        List<T> GetList<T>(string sql, SugarParameter[] parameters) where T : BaseEntity,new();

            /// <summary>
            /// 根据条件获取实体列表
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="conditionals">Sugar调价表达式集合</param>
            /// <returns>实体</returns>
            List<T> GetList<T>(List<IConditionalModel> conditionals) where T : BaseEntity,new();

            /// <summary>
            /// 查询集合
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="whereLambda">查询表达式</param>
            /// <returns>实体</returns>
            DataTable GetDataTable<T>(Expression<Func<T, bool>> whereLambda) where T : BaseEntity,new();

            /// <summary>
            /// 查询集合
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="sql">sql</param>
            /// <returns>实体</returns>
            DataTable GetDataTable<T>(string sql) where T : BaseEntity,new();

            /// <summary>
            /// 查询存储过程
            /// </summary> 
            /// <param name="procedureName">存储过程名称</param>
            /// <param name="parameters">参数</param>
            DataTable GetProcedure(string procedureName, List<SugarParameter> parameters);

            /// <summary>
            /// 查询单条数据
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
            /// <param name="whereLambda">查询表达式</param> 
            /// <param name="orderbyLambda">排序表达式</param> 
            /// <param name="isAsc">是否升序</param> 
            /// <returns></returns>
            T Single<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderbyLambda = null, bool isAsc = true) where T : BaseEntity,new();

        /// <summary>
        /// 根据主键获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById<T>(dynamic id) where T : BaseEntity,new();

            /// <summary>
            /// 获取首行首列
            /// </summary>
            /// <param name="sql">sql语句</param>
            /// <param name="parameters">参数</param>
            /// <returns></returns>
            object GetScalar(string sql, object parameters);

            /// <summary>
            /// 获取首行首列
            /// </summary>
            /// <param name="sql">sql语句</param>
            /// <param name="parameters">参数</param>
            /// <returns></returns>
            object GetScalar(string sql, params SugarParameter[] parameters);

            /// <summary>
            /// 是否存在
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
            /// <param name="whereLambda">查询表达式</param> 
            /// <returns></returns>
            bool IsExist<T>(Expression<Func<T, bool>> whereLambda) where T : BaseEntity,new();

            #endregion

            #region 分页查询

            /// <summary>
            /// 获取分页列表【页码，每页条数】
            /// </summary>
            /// <typeparam name="T">数据源类型</typeparam>
            /// <param name="pageIndex">页码（从0开始）</param>
            /// <param name="pageSize">每页条数</param>
            /// <returns>实体</returns>
            PagedList<T> GetPageList<T>(int pageIndex, int pageSize) where T : BaseEntity,new();

            /// <summary>
            /// 获取分页列表【排序，页码，每页条数】
            /// </summary>
            /// <typeparam name="T">数据源类型</typeparam>
            /// <param name="orderExp">排序表达式</param>
            /// <param name="orderType">排序类型</param>
            /// <param name="pageIndex">页码（从0开始）</param>
            /// <param name="pageSize">每页条数</param>
            /// <returns>实体</returns>
            PagedList<T> GetPageList<T>(Expression<Func<T, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where T : BaseEntity,new();

            /// <summary>
            /// 获取分页列表【Linq表达式条件，页码，每页条数】
            /// </summary>
            /// <typeparam name="T">数据源类型</typeparam>
            /// <param name="whereExp">Linq表达式条件</param>
            /// <param name="pageIndex">页码（从0开始）</param>
            /// <param name="pageSize">每页条数</param>
            /// <returns>实体</returns>
            PagedList<T> GetPageList<T>(Expression<Func<T, bool>> whereExp, int pageIndex, int pageSize) where T : BaseEntity,new();

            /// <summary>
            /// 获取分页列表【Linq表达式条件，排序，页码，每页条数】
            /// </summary>
            /// <typeparam name="T">数据源类型</typeparam>
            /// <param name="whereExp">Linq表达式条件</param>
            /// <param name="orderExp">排序表达式</param>
            /// <param name="orderType">排序类型</param>
            /// <param name="pageIndex">页码（从0开始）</param>
            /// <param name="pageSize">每页条数</param>
            /// <returns>实体</returns>
            PagedList<T> GetPageList<T>(Expression<Func<T, bool>> whereExp, Expression<Func<T, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where T : BaseEntity,new();

            /// <summary>
            /// 获取分页列表【Sugar表达式条件，页码，每页条数】
            /// </summary>
            /// <typeparam name="T">数据源类型</typeparam>
            /// <param name="conditionals">Sugar条件表达式集合</param>
            /// <param name="pageIndex">页码（从0开始）</param>
            /// <param name="pageSize">每页条数</param>
            /// <returns>实体</returns>
            PagedList<T> GetPageList<T>(List<IConditionalModel> conditionals, int pageIndex, int pageSize) where T : BaseEntity,new();

            /// <summary>
            ///  获取分页列表【Sugar表达式条件，排序，页码，每页条数】
            /// </summary>
            /// <typeparam name="T">数据源类型</typeparam>
            /// <param name="conditionals">Sugar条件表达式集合</param>
            /// <param name="orderExp">排序表达式</param>
            /// <param name="orderType">排序类型</param>
            /// <param name="pageIndex">页码（从0开始）</param>
            /// <param name="pageSize">每页条数</param>
            /// <returns>实体</returns>
            PagedList<T> GetPageList<T>(List<IConditionalModel> conditionals, Expression<Func<T, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where T : BaseEntity,new();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>实体列表</returns>
        PagedList<T> GetPageList<T>(QueryDescriptor query) where T : BaseEntity,new();

            /// <summary>
            /// 分页查询
            /// </summary>
            /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
            /// <param name="query">查询条件</param>
            /// <param name="totalCount">总行数</param>
            /// <returns>DataTable</returns>
            DataTable GetDataTablePageList<T>(QueryDescriptor query, out int totalCount) where T : BaseEntity,new();

        

        #endregion
    }
}
