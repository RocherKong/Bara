using Bara.Abstract.Tag;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public abstract class Tag : ITag
    {
        public abstract TagType Type { get; }

        /// <summary>
        /// 是否是迭代标签
        /// </summary>
        [Obsolete("Remove In Tag")]
        public bool In { get; set; }

        /// <summary>
        /// 满足条件则添加
        /// 不满足则不拼接
        /// </summary>
        /// <param name="objParam"></param>
        /// <returns></returns>
        public abstract bool IsNeedShow(RequestContext context);

        /// <summary>
        /// 操作属性
        /// </summary>
        public String Property { get; set; }

        /// <summary>
        /// 连接字符
        /// </summary>
        public virtual String Prepend { get; set; }

        /// <summary>
        /// 嵌套标签
        /// </summary>
        public IList<ITag> Children { get; set; }

        /// <summary>
        /// 构建sql
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameterPrefix"></param>
        /// <returns></returns>
        public virtual string BuildSql(RequestContext context)
        {
            if (IsNeedShow(context))
            {
                string dbPrefix = GetDbProviderPrefix(context);
                if (In)
                {
                    return $"{Prepend} In {dbPrefix}{Property}";
                }
               
                return $"{Prepend}{BuildChildSql(context).ToString()}";
            }
            else
            {
                return string.Empty;
            }
        }

        public virtual StringBuilder BuildChildSql(RequestContext context)
        {
            StringBuilder sb = new StringBuilder();
            if (Children != null && Children.Count > 0)
            {
                foreach (var child in Children)
                {
                    String strSql = child.BuildSql(context);
                    if (String.IsNullOrWhiteSpace(strSql))
                    {
                        continue;
                    }
                    sb.Append(strSql);
                }
            }

            return sb;
        }

        protected virtual String GetDbProviderPrefix(RequestContext context)
        {
            return context._baraMap.BaraMapConfig.DataBase.DbProvider.ParameterPrefix;
        }

        protected virtual Object GetPropertyValue(RequestContext context)
        {
            var reqParams = context.RequestParameters;
            if (reqParams == null) { return null; }
            if (reqParams.ContainsKey(Property))
            {
                return reqParams[Property];
            }
            return null;
        }
    }
}
