using Bara.Abstract.Core;
using Bara.Abstract.Builder;
using Bara.Core.Context;
using Bara.Exceptions;
using Bara.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Builder
{
    public class SqlBuilder : ISqlBuilder
    {
        private ILogger _logger;
        private IBaraMapper _baraMapper { get; set; }

        private IDictionary<String, Statement> MappedStatements => _baraMapper.BaraMapConfig.MappedStatements;
        public SqlBuilder(ILoggerFactory loggerFactory, IBaraMapper baraMapper)
        {
            this._logger = loggerFactory.CreateLogger<SqlBuilder>();
            this._baraMapper = baraMapper;
        }

        public String BuildSql(RequestContext context)
        {
            if (!MappedStatements.ContainsKey(context.FullSqlId)) {
                throw new BaraException($"Can't Find Sql:{context.FullSqlId} in MappedStatements.Please Check Name or .Xml file.");
            }
            var stateMent = MappedStatements[context.FullSqlId];
            return GenerateSql(context, stateMent);
        }

        public string GenerateSql(RequestContext context, Statement statement) {
            if (statement == null) {
                throw new BaraException($"Can't Find Sql:{context.FullSqlId} in MappedStatements.Please Check Name or .Xml file.");
            }
            return statement.BuildSql(context);
        }

    }
}
