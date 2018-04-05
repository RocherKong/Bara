using Bara.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Tag
{
    public interface ITag
    {
        TagType Type { get; }

        String BuildSql(RequestContext context);
    }

    public enum TagType
    {
        SqlText,
        IsEmpty,
        IsGreaterEqual,
        IsLessEqual,
        IsEqual,
        IsGreaterThan,
        IsLessThan,
        IsNotEmpty,
        IsNotEqual,
        IsNotNull,
        IsNull,
        Include,
        Switch,
        Case,
        SwitchDefault,
        Where,
        Dynamic,
        IsProperty,
    }
}
