using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationFormatter
    {
        public static string Format(IReadOnlyList<EquationNodeModel> nodes)
        {
            var resultNode = nodes.First(x => x.IsResult);
            var expressionBuilder = new StringBuilder($"{resultNode.ShortName}=");
            for (var i = 0; i < nodes.Count; i++)
            {
                if (i != 0 && !nodes[i].IsResult)
                    expressionBuilder.Append("+");
                expressionBuilder.AppendNode(nodes[i], i);
            }
            return expressionBuilder.ToString();
        }

        private static StringBuilder AppendNode(this StringBuilder builder, EquationNodeModel node, int index)
        {
            return node.IsResult
                ? (node.IsDefined
                    ? AppendDefinedResultNode(builder, node)
                    : AppendUndefinedResultNode(builder, node, index))
                : (node.IsDefined
                    ? AppendDefinedParamNode(builder, node)
                    : AppendUndefinedParamNode(builder, node, index));
        }

        private static StringBuilder AppendDefinedParamNode(StringBuilder builder, EquationNodeModel node)
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

        private static StringBuilder AppendDefinedResultNode(StringBuilder builder, EquationNodeModel node)
        {
            if (!NeedsFormatting(node))
                return builder;
            builder.Append("+[");
            if (node.IsK1Enabled)
                builder.AppendFormat("{0:0.0000;-0.0000}*{1}(k-1)", node.FactorK1, node.ShortName);
            if (node.IsK2Enabled)
                builder.AppendFormat("{2}{0:0.0000;-0.0000}*{1}(k-2)",
                    node.FactorK2,
                    node.ShortName,
                    node.IsK1Enabled ? "+" : string.Empty);
            if (node.IsK3Enabled)
                builder.AppendFormat("{2}{0:0.0000;-0.0000}*{1}(k-3)",
                    node.FactorK3,
                    node.ShortName,
                    node.IsK2Enabled ? "+" : string.Empty);
            return builder.Append("]");
        }

        private static bool NeedsFormatting(EquationNodeModel node)
        {
            return node.IsK1Enabled || node.IsK2Enabled || node.IsK3Enabled;
        }

        private static StringBuilder AppendUndefinedResultNode(StringBuilder builder, EquationNodeModel node, int index)
        {
            if (!NeedsFormatting(node))
                return builder;
            builder.Append("+[");
            if (node.IsK1Enabled)
                builder.AppendFormat("a({0},1)*{1}(k-1)", index + 1, node.ShortName);
            if (node.IsK2Enabled)
                builder.AppendFormat("{2}a({0},2)*{1}(k-2)",
                    index + 1,
                    node.ShortName,
                    node.IsK1Enabled ? "+" : string.Empty);
            if (node.IsK3Enabled)
                builder.AppendFormat("{2}a({0},3)*{1}(k-3)",
                    index + 1,
                    node.ShortName,
                    node.IsK2Enabled ? "+" : string.Empty);
            return builder.Append("]");
        }
    }
}