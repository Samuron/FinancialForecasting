using System;
using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public class ModelErrorCalculator
    {
        private readonly EquationBuilder _equationBuilder;
        private readonly IReadOnlyList<EquationNodeModel> _nodes;
        private readonly RegressionIndexListFormatter _regressionIndexListFormatter;

        public ModelErrorCalculator(IReadOnlyList<EquationNodeModel> nodes)
        {
            _nodes = nodes;
            _regressionIndexListFormatter = new RegressionIndexListFormatter(nodes);
            _equationBuilder = new EquationBuilder(nodes);
        }

        public ModelErrors Calculate(IReadOnlyList<double[]> indices)
        {
            // TODO: use prepared indexes
            var factors = _nodes.Factors();
            var resultPosition = _nodes.FindIndex(x => x.IsResult);
            var formated = _regressionIndexListFormatter.FormatIndexList(indices);
            var given = indices.Skip(indices.Count - formated.Count).Select(x => x[resultPosition]);
            var calculated = formated.Select(x => _equationBuilder.Calculate(factors, x));

            var yAverage = given.Average();
            var count = formated.Count;

            var epsilons = given.Zip(calculated,
                (givenValue, calculatedValue) =>
                    new {EpsilonCalculated = givenValue - calculatedValue, EpsilonAverage = givenValue - yAverage});

            var eSquared = epsilons.Sum(x => x.EpsilonCalculated*x.EpsilonCalculated);
            var eAverage = epsilons.Sum(x => x.EpsilonAverage*x.EpsilonAverage);
            var determination = 1 - eSquared/eAverage;
            var dw =
                epsilons.Skip(1).Zip(epsilons, (e1, e2) => e1.EpsilonCalculated - e2.EpsilonCalculated).Sum(x => x*x)/
                eSquared;

            var skp = Math.Sqrt(eSquared/count);
            var sapp = epsilons.Zip(given, (x, y) => Math.Abs(x.EpsilonCalculated)/Math.Abs(y)).Sum()/count;
            var theil = skp/(Math.Sqrt(given.Sum(x => x*x)/count) + Math.Sqrt(calculated.Sum(x => x*x)/count));
            return new ModelErrors(determination, eSquared, dw, skp, sapp, theil);
        }
    }

    public class ModelErrors
    {
        public ModelErrors(double determination,
            double eSquared,
            double darbinWattson,
            double skp,
            double sapp,
            double theil)
        {
            Determination = determination;
            ESquared = eSquared;
            DarbinWattson = darbinWattson;
            Skp = skp;
            Sapp = sapp;
            Theil = theil;
        }

        public double Determination { get; }

        public double ESquared { get; }

        public double DarbinWattson { get; }

        public double Skp { get; }

        public double Sapp { get; }

        public double Theil { get; }
    }
}