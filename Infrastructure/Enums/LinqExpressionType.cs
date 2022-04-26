using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Enums
{
    public enum LinqExpressionType
    {
        Equal = 0,//=
        NotEqual = 1,//!=
        GreaterThan,//>
        LessThan,//<
        ThanOrEqual,//>=
        LessThanOrEqual,//<=
        In,
        Contains,//Contains
        NotContains//NotContains
    }
}
