using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class Switch : Tag
    {
        public override TagType Type => TagType.Switch;

        public override bool IsNeedShow(RequestContext context)
        {
            return true;
        }

        public override string BuildSql(RequestContext context)
        {
            var MatchedTag = Children.FirstOrDefault((tag) =>
            {
                if (tag.Type == TagType.Case)
                {
                    var caseTag = tag as Case;
                    return caseTag.IsNeedShow(context);
                }
                return false;
            });
            if (MatchedTag == null)
            {
                MatchedTag = Children.FirstOrDefault(tag => tag.Type == TagType.SwitchDefault);
            }

            if (MatchedTag != null)
            {
                return MatchedTag.BuildSql(context);
            }
            return String.Empty;
        }

        public class Case : CompareTag
        {
            public override TagType Type => TagType.Case;

            public override bool IsNeedShow(RequestContext context)
            {
                var reqVal = GetPropertyValue(context);
                if (reqVal == null)
                {
                    return false;
                }
                String caseValue = string.Empty;
                if (reqVal is Enum)
                {
                    caseValue = reqVal.GetHashCode().ToString();
                }
                else
                {
                    caseValue = reqVal.ToString();
                }
                return caseValue.Equals(CompareValue);
            }
        }

        public class Default : Tag
        {
            public override TagType Type => TagType.SwitchDefault;

            public override bool IsNeedShow(RequestContext context)
            {
                return true;
            }
        }
    }
}
