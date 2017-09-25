using Bara.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.DataSource;
using Bara.Abstract.Core;
using Bara.DataAccess.Model;

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
        public int Delete<TPrimary>(TPrimary Id)
        {
            return baraMapper.Execute(new Core.Context.RequestContext
            {
                Request = new { Id = Id },
                Scope = this.Scope,
                SqlId = DefaultSqlId.DELETE
            });
        }

        public TEntity GetEntity<TPrimary>(TPrimary Id, DataSourceType sourceType = DataSourceType.Read)
        {
            return baraMapper.QuerySingle<TEntity>(new Core.Context.RequestContext
            {
                Request = new { Id = Id },
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
            return baraMapper.Execute(new Core.Context.RequestContext
            {
                Request = entity,
                SqlId = DefaultSqlId.UPDATE,
                Scope = this.Scope
            });
        }

        protected override void InitScope()
        {
            this.Scope = typeof(TEntity).Name;
        }
    }
}
