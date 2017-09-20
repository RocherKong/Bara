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
        public bool In { get; set; }

        /// <summary>
        /// 满足条件则添加
        /// 不满足则不拼接
        /// </summary>
        /// <param name="objParam"></param>
        /// <returns></returns>
        public abstract bool IsNeedShow(object objParam);

        /// <summary>
        /// 操作属性
        /// </summary>
        public String Property { get; set; }

        /// <summary>
        /// 连接字符
        /// </summary>
        public String Prepend { get; set; }

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
        public virtual string BuildSql(RequestContext context, string parameterPrefix)
        {
            if (IsNeedShow(context.Request))
            {
                if (In)
                {
                    return $"{Prepend} In {parameterPrefix}{Property}";
                }
                StringBuilder sb = new StringBuilder();
                if (Children != null && Children.Count > 0)
                {
                    foreach (var child in Children)
                    {
                        var _strSql = child.BuildSql(context, parameterPrefix);
                        sb.Append(_strSql);
                    }
                }
                return $"{Prepend}{sb.ToString()}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
