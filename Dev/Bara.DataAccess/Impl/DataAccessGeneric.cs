using Bara.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.DataSource;
using Bara.Abstract.Core;
using Bara.DataAccess.Model;
using System.Reflection;

namespace Bara.DataAccess.Impl
{
    public class DataAccessGeneric<TEntity> : DataAccess, IQuery<TEntity>, IWrite<TEntity> where TEntity : class
    {
        public DataAccessGeneric(String BaraMapConfigPath = "BaraMapConfig.xml") : base(BaraMapConfigPath)
        {

        }
        public DataAccessGeneric(IBaraMapper baraMapper) : base(baraMapper)
        {

        }

        protected String PrimaryKey { get; set; } = "Id";
        public int Delete<TPrimary>(TPrimary Id)
        {
            var parameters = new Dictionary<string, object>
            {
                { PrimaryKey, Id }
            };
            return baraMapper.Execute(new Core.Context.RequestContext
            {
                Request = parameters,
                Scope = this.Scope,
                SqlId = DefaultSqlId.DELETE
            });
        }

        public TEntity GetEntity<TPrimary>(TPrimary Id, DataSourceType sourceType = DataSourceType.Read)
        {
            var parameters = new Dictionary<string, object>
            {
                { PrimaryKey, Id }
            };
            return baraMapper.QuerySingle<TEntity>(new Core.Context.RequestContext
            {
                Request = parameters,
                Scope = this.Scope,
                SqlId = DefaultSqlId.GETENTITY
            }, sourceType);
        }

        public TEntity GetSingleEntity(object reqParams, DataSourceType sourceType = DataSourceType.Read)
        {
            return baraMapper.QuerySingle<TEntity>(new Core.Context.RequestContext
            {
                Request = reqParams,
                Scope = this.Scope,
                SqlId = DefaultSqlId.GETENTITY
            }, sourceType);
        }

        public IEnumerable<TResponse> QueryList<TResponse>(object paramObj, DataSourceType sourceType = DataSourceType.Read)
        {
            return baraMapper.Query<TResponse>(new Core.Context.RequestContext
            {
                Request = paramObj,
                Scope = this.Scope,
                SqlId = DefaultSqlId.QUERYLIST
            }, sourceType);
        }

        public IEnumerable<TResponse> QueryListByPage<TResponse>(object paramObj, DataSourceType sourceType = DataSourceType.Read)
        {
            return baraMapper.Query<TResponse>(new Core.Context.RequestContext
            {
                Request = paramObj,
                Scope = this.Scope,
                SqlId = DefaultSqlId.QUERYLISTBYPAGE
            }, sourceType);
        }

        public IEnumerable<TResponse> QueryListByPage<TResponse>(object paramObj, int PageIndex, int PageSize, DataSourceType sourceType = DataSourceType.Read)
        {
            Dictionary<String, object> reqParams =new Dictionary<String, object>();
            foreach (var keyvalue in paramObj.GetType().GetProperties())
            {
                reqParams.Add(keyvalue.Name, keyvalue.GetValue(paramObj));
            }
            reqParams.Add("PageIndex", PageIndex);
            reqParams.Add("PageSize", PageSize);
            return baraMapper.Query<TResponse>(new Core.Context.RequestContext
            {
                Request = reqParams,
                Scope = this.Scope,
                SqlId = DefaultSqlId.QUERYLISTBYPAGE
            }, sourceType);
        }

        public int GetRecord(object paramObj, DataSourceType sourceType = DataSourceType.Read)
        {
            return baraMapper.ExecuteScalar<int>(new Core.Context.RequestContext
            {
                Request = paramObj,
                Scope = this.Scope,
                SqlId = DefaultSqlId.GETRECORD
            });
        }

        public TPrimary Insert<TPrimary>(TEntity entity)
        {
            bool isNoneIdentity = typeof(TPrimary) == typeof(NoneIdentity);
            if (!isNoneIdentity)
            {
                return baraMapper.ExecuteScalar<TPrimary>(new Core.Context.RequestContext
                {
                    Request = entity,
                    Scope = this.Scope,
                    SqlId = DefaultSqlId.INSERT
                });
            }
            else
            {
                baraMapper.Execute(new Core.Context.RequestContext
                {
                    Scope = this.Scope,
                    Request = entity,
                    SqlId = DefaultSqlId.INSERT
                });
                return default(TPrimary);
            }
        }

        public bool IsExist(object reqParams, DataSourceType sourceType = DataSourceType.Read)
        {
            return baraMapper.QuerySingle<int>(new Core.Context.RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.GETRECORD,
                Request = reqParams
            }, sourceType) > 0;
        }

        public int Update(TEntity entity)
        {
            return DynamicUpdate(entity);
        }

        protected override void InitScope()
        {
            this.Scope = typeof(TEntity).Name;
        }

        public void Insert(TEntity entity)
        {
            Insert<NoneIdentity>(entity);
        }

        public int DynamicUpdate(object entity)
        {
            return baraMapper.Execute(new Core.Context.RequestContext
            {
                Request = entity,
                Scope = this.Scope,
                SqlId = DefaultSqlId.UPDATE
            });
        }

        public int DeleteBatch<TPrimary>(TPrimary[] ids)
        {
            return baraMapper.Execute(new Core.Context.RequestContext
            {
                Request = new { Ids = ids },
                Scope = Scope,
                SqlId = "DeleteBatch"
            });
        }


    }
}
