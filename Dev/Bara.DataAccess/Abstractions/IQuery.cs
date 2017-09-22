using Bara.Abstract.DataSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.DataAccess.Abstractions
{
    public interface IQuery<TEntity> : IExist, IGetEntity<TEntity>, IGetList, IGetListByPage, IGetRecord
         where TEntity : class
    {
    }

    public interface IExist {
        bool IsExist(object reqParams, DataSourceType sourceType = DataSourceType.Read);
    }

    public interface IGetEntity<TEntity> where TEntity : class {
        TEntity GetEntity<TPrimary>(TPrimary Id,DataSourceType sourceType=DataSourceType.Read);
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    public interface IGetList
    {
        IEnumerable<TResponse> GetList<TResponse>(object paramObj, DataSourceType sourceType = DataSourceType.Read);
    }
    /// <summary>
    /// 分页
    /// </summary>
    public interface IGetListByPage
    {
        IEnumerable<TResponse> GetListByPage<TResponse>(object paramObj, DataSourceType sourceType = DataSourceType.Read);
    }
    /// <summary>
    /// 获取记录数
    /// </summary>
    public interface IGetRecord
    {
        int GetRecord(object paramObj, DataSourceType sourceType = DataSourceType.Read);
    }
}
