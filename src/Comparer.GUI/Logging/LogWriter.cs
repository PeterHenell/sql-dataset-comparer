using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Comparer.GUI.Logging
{
    public class PropertyAppendLogWriter<T> : TextWriter
    {
        private T parent;
        private Expression<Func<T, string>> outExpr;

        public PropertyAppendLogWriter(T target, Expression<Func<T, string>> outExpr)
        {
            this.parent = target;
            this.outExpr = outExpr;
        }

        public override void WriteLine(string message)
        {
            base.WriteLine();
            var expr = (MemberExpression)outExpr.Body;
            var prop = (PropertyInfo)expr.Member;
            string value = prop.GetValue(parent) as string;
            prop.SetValue(parent, value + message + Environment.NewLine, null);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}
