using System.Linq;
using System.Text;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationFormatter
    {
        public static string Format(params EquationNodeModel[] nodes)
        {
            var expressionBuilder = new StringBuilder("=");
            var enabledNodes = nodes.Where(x => x.IsEnabled).ToList();
            for (var i = 0; i < enabledNodes.Count; i++)
            {
                if (i != 0 && !enabledNodes[i].IsYNode)
                    expressionBuilder.Append("+");
                expressionBuilder.AppendNode(enabledNodes[i], i);
            }
            return expressionBuilder.ToString();
        }

        private static StringBuilder AppendNode(this StringBuilder builder, EquationNodeModel node, int index)
        {
            return node.IsDefined ? AppendDefinedNode(builder, node) : AppendUndefinedNode(builder, node, index);
        }

        private static StringBuilder AppendDefinedNode(StringBuilder builder, EquationNodeModel node)
        {
            if (node.IsYNode)
                return builder.Insert(0, $"{node.ShortName}");
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
            return node.IsYNode
                ? PrependUndefinedResultNode(builder, node, index)
                : AppendUndefinedParamNode(builder, node, index);
        }

        private static StringBuilder PrependUndefinedResultNode(StringBuilder builder, EquationNodeModel node, int index)
        {
            builder.Insert(0, "]");
            if (node.IsK3Enabled)
                builder.Insert(0, $"+{node.ShortName}(k-3)");
            if (node.IsK2Enabled)
                builder.Insert(0, $"+{node.ShortName}(k-2)");
            if (node.IsK1Enabled)
                builder.Insert(0, $"+{node.ShortName}(k-1)");
            builder.Insert(0, node.ShortName);
            return builder.Insert(0, "[");
        }

        private static StringBuilder AppendUndefinedParamNode(StringBuilder builder, EquationNodeModel node, int index)
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