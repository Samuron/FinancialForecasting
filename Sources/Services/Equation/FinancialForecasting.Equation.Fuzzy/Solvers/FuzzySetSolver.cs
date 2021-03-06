﻿using System;
using System.Collections.Generic;

namespace FinancialForecasting.Equation
{
    public class FuzzySetSolver : IFuzzySolver
    {
        public Tuple<double, Risk> Solve(IReadOnlyList<double> balance)
        {
            var x = balance.ToParameters();

            var l = new double[5, 6];

            if ((x[0] >= 0) & (x[0] < 0.1))
                l[0, 0] = 1;
            if ((x[0] >= 0.1) & (x[0] < 0.2))
            {
                l[0, 0] = (0.2 - x[0])/0.1;
                l[1, 0] = (x[0] - 0.1)/0.1;
            }
            if ((x[0] >= 0.2) & (x[0] < 0.25))
                l[1, 0] = 1;
            if ((x[0] >= 0.25) & (x[0] < 0.3))
            {
                l[1, 0] = (0.3 - x[0])/0.05;
                l[2, 0] = (x[0] - 0.25)/0.05;
            }
            if ((x[0] >= 0.3) & (x[0] < 0.45))
                l[2, 0] = 1;
            if ((x[0] >= 0.45) & (x[0] < 0.5))
            {
                l[2, 0] = (0.5 - x[0])/0.05;
                l[3, 0] = (x[0] - 0.45)/0.05;
            }
            if ((x[0] >= 0.5) & (x[0] < 0.6))
                l[3, 0] = 1;
            if ((x[0] >= 0.6) & (x[0] < 0.7))
            {
                l[3, 0] = (0.7 - x[0])/0.1;
                l[4, 0] = (x[0] - 0.6)/0.1;
            }
            if ((x[0] >= 0.7) & (x[0] < 1))
                l[4, 0] = 1;

            if ((x[1] >= -1) & (x[1] < -0.005))
                l[0, 1] = 1;
            if ((x[1] >= -0.005) & (x[1] < 0))
            {
                l[0, 1] = (-x[1])/0.005;
                l[1, 1] = (x[1] + 0.005)/0.005;
            }
            if ((x[1] >= 0) & (x[1] < 0.09))
                l[1, 1] = 1;
            if ((x[1] >= 0.09) & (x[1] < 0.11))
            {
                l[1, 1] = (0.11 - x[1])/0.02;
                l[2, 1] = (x[1] - 0.09)/0.02;
            }
            if ((x[1] >= 0.11) & (x[1] < 0.3))
                l[2, 1] = 1;
            if ((x[1] >= 0.3) & (x[1] < 0.35))
            {
                l[2, 1] = (0.35 - x[1])/0.05;
                l[3, 1] = (x[1] - 0.3)/0.05;
            }
            if ((x[1] >= 0.35) & (x[1] < 0.45))
                l[3, 1] = 1;
            if ((x[1] >= 0.45) & (x[1] < 0.5))
            {
                l[3, 1] = (0.5 - x[1])/0.05;
                l[4, 1] = (x[1] - 0.45)/0.05;
            }
            if ((x[1] >= 0.5) & (x[1] < 1))
                l[4, 1] = 1;

            if ((x[2] >= 0) & (x[2] < 0.5))
                l[0, 2] = 1;
            if ((x[2] >= 0.5) & (x[2] < 0.6))
            {
                l[0, 2] = (0.6 - x[2])/0.1;
                l[1, 2] = (x[2] - 0.5)/0.1;
            }
            if ((x[2] >= 0.6) & (x[2] < 0.7))
                l[1, 2] = 1;
            if ((x[2] >= 0.7) & (x[2] < 0.8))
            {
                l[1, 2] = (0.8 - x[2])/0.1;
                l[2, 2] = (x[2] - 0.7)/0.1;
            }
            if ((x[2] >= 0.8) & (x[2] < 0.9))
                l[2, 2] = 1;
            if ((x[2] >= 0.9) & (x[2] < 1))
            {
                l[2, 2] = (1 - x[2])/0.1;
                l[3, 2] = (x[2] - 0.9)/0.1;
            }
            if ((x[2] >= 1) & (x[2] < 1.3))
                l[3, 2] = 1;
            if ((x[2] >= 1.3) & (x[2] < 1.5))
            {
                l[3, 2] = (1.5 - x[2])/0.2;
                l[4, 2] = (x[2] - 1.3)/0.2;
            }
            if ((x[2] >= 1.5))
                l[4, 2] = 1;

            if ((x[3] >= 0) & (x[3] < 0.02))
                l[0, 3] = 1;
            if ((x[3] >= 0.02) & (x[3] < 0.03))
            {
                l[0, 3] = (0.03 - x[3])/0.01;
                l[1, 3] = (x[3] - 0.02)/0.01;
            }
            if ((x[3] >= 0.03) & (x[3] < 0.08))
                l[1, 3] = 1;
            if ((x[3] >= 0.08) & (x[3] < 0.1))
            {
                l[1, 3] = (0.1 - x[3])/0.02;
                l[2, 3] = (x[3] - 0.08)/0.02;
            }
            if ((x[3] >= 0.1) & (x[3] < 0.3))
                l[2, 3] = 1;
            if ((x[3] >= 0.3) & (x[3] < 0.35))
            {
                l[2, 3] = (0.35 - x[3])/0.05;
                l[3, 3] = (x[3] - 0.3)/0.05;
            }
            if ((x[3] >= 0.35) & (x[3] < 0.5))
                l[3, 3] = 1;
            if ((x[3] >= 0.5) & (x[3] < 0.6))
            {
                l[3, 3] = (0.6 - x[3])/0.1;
                l[4, 3] = (x[3] - 0.5)/0.1;
            }
            if ((x[3] >= 0.6))
                l[4, 3] = 1;

            if ((x[4] >= 0) & (x[4] < 0.12))
                l[0, 4] = 1;
            if ((x[4] >= 0.12) & (x[4] < 0.14))
            {
                l[0, 4] = (0.14 - x[4])/0.02;
                l[1, 4] = (x[4] - 0.12)/0.02;
            }
            if ((x[4] >= 0.14) & (x[4] < 0.18))
                l[1, 4] = 1;
            if ((x[4] >= 0.18) & (x[4] < 0.2))
            {
                l[1, 4] = (0.2 - x[4])/0.02;
                l[2, 4] = (x[4] - 0.18)/0.02;
            }
            if ((x[4] >= 0.2) & (x[4] < 0.3))
                l[2, 4] = 1;
            if ((x[4] >= 0.3) & (x[4] < 0.4))
            {
                l[2, 4] = (0.4 - x[4])/0.1;
                l[3, 4] = (x[4] - 0.3)/0.1;
            }
            if ((x[4] >= 0.4) & (x[4] < 0.5))
                l[3, 4] = 1;
            if ((x[4] >= 0.5) & (x[4] < 0.8))
            {
                l[3, 4] = (0.8 - x[4])/0.3;
                l[4, 4] = (x[4] - 0.5)/0.3;
            }
            if ((x[4] >= 0.8))
                l[4, 4] = 1;

            if (x[5] < 0)
                l[0, 5] = 1;
            if ((x[5] >= 0) & (x[5] < 0.006))
                l[1, 5] = 1;
            if ((x[5] >= 0.006) & (x[5] < 0.01))
            {
                l[1, 5] = (0.01 - x[5])/0.004;
                l[2, 5] = (x[5] - 0.006)/0.004;
            }
            if ((x[5] >= 0.01) & (x[5] < 0.06))
                l[2, 5] = 1;
            if ((x[5] >= 0.06) & (x[5] < 0.1))
            {
                l[2, 5] = (0.1 - x[5])/0.04;
                l[3, 5] = (x[5] - 0.06)/0.04;
            }
            if ((x[5] >= 0.1) & (x[5] < 0.225))
                l[3, 5] = 1;
            if ((x[5] >= 0.225) & (x[5] < 0.4))
            {
                l[3, 5] = (0.4 - x[5])/0.175;
                l[4, 5] = (x[5] - 0.225)/0.175;
            }
            if ((x[5] >= 0.4))
                l[4, 5] = 1;

            const double r = 0.1666667;
            double g = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    g = g + r*(0.9 - 0.2*i)*l[i, j];
                }
            }
            var G = new double[5];

            if ((g >= 0) & (g < 0.15))
                G[4] = 1;
            if ((g >= 0.15) & (g < 0.25))
            {
                G[4] = 10*(0.25 - g);
                G[3] = 1 - G[4];
            }
            if ((g >= 0.25) & (g < 0.35))
                G[3] = 1;
            if ((g >= 0.35) & (g < 0.45))
            {
                G[3] = 10*(0.45 - g);
                G[2] = 1 - G[3];
            }
            if ((g >= 0.45) & (g < 0.55))
                G[2] = 1;
            if ((g >= 0.55) & (g < 0.65))
            {
                G[2] = 10*(0.65 - g);
                G[1] = 1 - G[2];
            }
            if ((g >= 0.65) & (g < 0.75))
                G[1] = 1;
            if ((g >= 0.75) & (g < 0.85))
            {
                G[1] = 10*(0.85 - g);
                G[0] = 1 - G[1];
            }
            if ((g >= 0.85) & (g <= 1))
                G[0] = 1;

            double max = -1;
            var k = 0;
            for (var i = 0; i < 5; i++)
            {
                if (G[i] > max)
                {
                    max = G[i];
                    k = i;
                }
            }

            var risk = Risk.VeryHigh;
            if (k == 0)
                risk = Risk.VeryHigh;
            if (k == 1)
                risk = Risk.High;
            if (k == 2)
                risk = Risk.Average;
            if (k == 3)
                risk = Risk.Low;
            if (k == 4)
                risk = Risk.VeryLow;

            return Tuple.Create(g, risk);
        }
    }
}