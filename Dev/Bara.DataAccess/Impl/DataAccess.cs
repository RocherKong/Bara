using Bara.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Session;
using System.Data;
using Microsoft.Extensions.Logging;
using Bara.Abstract.Core;
using Bara.Core.Mapper;

namespace Bara.DataAccess.Impl
{
    public abstract class DataAccess : ITransaction
    {
        public String Scope { get; protected set; }
        protected abstract void InitScope();
        public IBaraMapper baraMapper;
        public DataAccess(String baraMapConfigPath = "BaraMapConfig.xml")
        {
            baraMapper = MapperContainer.Instance.GetBaraMapper(baraMapConfigPath);
            InitScope();
        }

        public DataAccess(ILoggerFactory loggerFactory, String baraMapConfigPath = "BaraMapConfig.xml")
        {
            baraMapper = MapperContainer.Instance.GetBaraMapper(loggerFactory, baraMapConfigPath);
            InitScope();
        }

        public DataAccess(IBaraMapper BaraMapper)
        {
            this.baraMapper = BaraMapper;
            InitScope();
        }
        public IDbConnectionSession BeginTransaction()
        {
            return baraMapper.BeginTransaction();
        }
      

        public IDbConnectionSession BeginTransaction(IsolationLevel isolationLevel)
        {
            return baraMapper.BeginTransaction();
        }

        public void CommitTransaction()
        {
            baraMapper.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            baraMapper.RollbackTransaction();
        }
    }
}
