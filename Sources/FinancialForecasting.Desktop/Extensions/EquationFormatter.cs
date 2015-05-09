using System.Linq;
using System.Text;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationFormatter
    {
        public static string Format(params EquationNodeModel[] nodes)
        {
            var expressionBuilder = new StringBuilder("y=");
            var enabledNodes = nodes.Where(x => x.IsEnabled).ToList();
            for (var i = 0; i < enabledNodes.Count; i++)
            {
                if (i != 0)
                    expressionBuilder.Append("+");
                expressionBuilder.AppendNode(enabledNodes[i], i);
            }
            return expressionBuilder.ToString();
        }

        private static StringBuilder AppendSign(this StringBuilder expressionBuilder, EquationNodeModel node)
        {
            return node.Factor >= 0.0 || !node.IsDefined ? expressionBuilder.Append("+") : expressionBuilder;
        }

        private static StringBuilder AppendNode(this StringBuilder builder, EquationNodeModel node, int index)
        {
            return node.IsDefined ? AppendDefinedNode(builder, node) : AppendUndefinedNode(builder, node, index);
        }

        private static StringBuilder AppendDefinedNode(StringBuilder builder, EquationNodeModel node)
        {
            if (node.IsVisible)
            {
                builder.Append("[");
                builder.AppendFormat("{0:0.0000;-0.0000}*{1}", node.Factor, node.ShortName);
                if (node.IsK1Enabled)
                    builder.AppendFormat("{0:+0.0000;-0.0000}*{1}(k-1)", node.FactorK1, node.ShortName);
                if (node.IsK2Enabled)
                    builder.AppendFormat("{0:+0.0000;-0.0000}*{1}(k-2)", node.FactorK2, node.ShortName);
                if (node.IsK3Enabled)
                    builder.AppendFormat("{0:+0.0000;-0.0000}*{1}(k-3)", node.FactorK3, node.ShortName);
                return builder.Append("]");
            }
            return builder.AppendFormat("{0:0.0000;-0.0000}", node.Factor);
        }

        private static StringBuilder AppendUndefinedNode(StringBuilder builder, EquationNodeModel node, int index)
        {
            builder.Append("[");
            builder.AppendFormat("a({0},0)*{1}", index + 1, node.ShortName);
            if (node.IsK1Enabled)
                builder.AppendFormat("+a({0},1)*{1}(k-1)", index + 1, node.ShortName);
            if (node.IsK2Enabled)
                builder.AppendFormat("+a({0},2)*{1}(k-2)", index + 1, node.ShortName);
            if (node.IsK3Enabled)
                builder.AppendFormat("+a({0},3)*{1}(k-3)", index + 1, node.ShortName);
            return builder.Append("]");
        }
    }
}