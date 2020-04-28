using System;
using System.Collections.Generic;

namespace KHBC.DataAccess
{
    /// <summary>
    /// 分页集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PagedList<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前页数据源
        /// </summary>
        public IList<T> Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int? TotalPage
        {
            get
            {
                int total = (int)Math.Ceiling((double)TotalCount / PageSize);
                return total > 0 ? total : 1;
            }
        }

        public PagedList()
        {

        }

        public PagedList(int totalCount, int pageSize, IList<T> data)
        {
            TotalCount = totalCount;
            Data = data;
            PageSize = pageSize;
        }
    }
}
