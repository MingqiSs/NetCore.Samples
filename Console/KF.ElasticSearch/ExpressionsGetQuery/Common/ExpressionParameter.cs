using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KF.ElasticSearch
{
    public class ExpressionParameter
    {
        public ExpressionContext Context { get; set; }
        public ExpressionParameter BaseParameter { get; set; }
        public Expression BaseExpression { get; set; }

        public Expression CurrentExpression { get; set; }
    }
}
