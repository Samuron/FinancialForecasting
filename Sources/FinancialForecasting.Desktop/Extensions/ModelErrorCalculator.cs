using System;
using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Desktop.Extensions
{
    public class ModelErrorCalculator
    {
        private readonly EquationBuilder _equationBuilder;
        private readonly EquationNodeModel[] _nodes;
        private readonly RegressionIndexListFormatter _regressionIndexListFormatter;

        public ModelErrorCalculator(params EquationNodeModel[] nodes)
        {
            _nodes = nodes;
            _regressionIndexListFormatter = new RegressionIndexListFormatter(nodes);
            _equationBuilder = new EquationBuilder(nodes);
        }

        public ModelErrors Calculate(IReadOnlyList<EnterpriseIndexDto> indices)
        {
            // TODO: use prepared indexes
            var factors = _nodes.Factors();
            var formated = _regressionIndexListFormatter.FormatIndexList(indices);
            var given = indices.Skip(indices.Count - formated.Count).Select(x => x.Y);
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
        public ModelErrors(double r, double e, double dw, double skp, double sapp, double theil)
        {
            R = r;
            E = e;
            Dw = dw;
            Skp = skp;
            Sapp = sapp;
            Theil = theil;
        }

        public double R { get; }

        public double E { get; }

        public double Dw { get; }

        public double Skp { get; }

        public double Sapp { get; }

        public double Theil { get; }
    }
}