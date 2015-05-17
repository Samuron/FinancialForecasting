using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> sequence)
        {
            return new ObservableCollection<T>(sequence);
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                    return retVal;
                retVal++;
            }
            return -1;
        }

        public static T Fold<T>(this IEnumerable<EquationNodeModel> nodes,
            T initial,
            Func<T, T, T> combiner,
            Func<int, EquationNodeModel, T> onDisabled,
            Func<int, EquationNodeModel, T> onEnabled,
            Func<int, EquationNodeModel, T> onK1,
            Func<int, EquationNodeModel, T> onK2,
            Func<int, EquationNodeModel, T> onK3)
        {
            var current = initial;
            var elementIndex = 0;
            foreach (var node in nodes)
            {
                if (!node.IsEnabled)
                {
                    current = combiner(current, onDisabled(elementIndex++, node));
                    continue;
                }
                current = combiner(current, onEnabled(elementIndex++, node));
                if (node.IsK1Enabled)
                    current = combiner(current, onK1(elementIndex++, node));
                if (node.IsK2Enabled)
                    current = combiner(current, onK2(elementIndex++, node));
                if (node.IsK3Enabled)
                    current = combiner(current, onK3(elementIndex++, node));
            }
            return current;
        }
    }
}