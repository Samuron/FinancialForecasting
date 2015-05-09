using System.Linq;
using System.Text;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationFormatter
    {
        public static string Format(params EquationNode[] nodes)
        {
            var expressionBuilder = new StringBuilder("y=");
            var enabledNodes = nodes.Where(x => x.IsEnabled).ToList();
            for (var i = 0; i < enabledNodes.Count; i++)
            {
                if (i != 0)
                    expressionBuilder.AppendSign(enabledNodes[i]);
                expressionBuilder.AppendNode(enabledNodes[i], i);
            }
            return expressionBuilder.ToString();
        }

        private static StringBuilder AppendSign(this StringBuilder expressionBuilder, EquationNode node)
        {
            return node.Factor >= 0.0 || !node.IsDefined ? expressionBuilder.Append("+") : expressionBuilder;
        }

        private static StringBuilder AppendNode(this StringBuilder builder, EquationNode node, int index)
        {
            return node.IsDefined
                ? builder.AppendFormat("{0:0.0000}*{1}", node.Factor, node.Name)
                : builder.AppendFormat("a{0}*{1}", index + 1, node.Name);
        }
    }
}