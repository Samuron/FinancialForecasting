using System;
using System.Text;

namespace FinancialForecasting.Desktop.Extensions
{
    public class ExpressionFormatter
    {
        public static String FormatExpression(params Boolean[] enabled)
        {
            var expressionBuilder = new StringBuilder("y=");
            for (var i = 0; i < enabled.Length; i++)
            {
                if (enabled[i])
                    expressionBuilder.AppendFormat("a{0}*x{0}+", i + 1);
            }
            return expressionBuilder.ToString(0, expressionBuilder.Length - 1);
        }

        public static String FormatWithConst(bool isConstEnabled, params Boolean[] enabled)
        {
            return isConstEnabled ? FormatExpression(enabled) + "+c" : FormatExpression(enabled);
        }
    }
}