using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Equation;
using Microsoft.Win32;

namespace FinancialForecasting.Desktop.ViewModels
{
    public class FuzzyViewModel : INotifyPropertyChanged
    {
        private readonly IFuzzySolver _altmanSolver;
        private readonly IFuzzySolver _fuzzySetSolver;
        private readonly IFuzzySolver _mamdaniSolver;
        private FuzzyResultModel _altman;
        private FuzzyResultModel _fuzzySet;
        private FuzzyResultModel _mamdani;
        private double[] _parameters;

        public FuzzyViewModel()
        {
            _altmanSolver = new AltmanModelSolver();
            _mamdaniSolver = new MamdaniModelSolver();
            _fuzzySetSolver = new FuzzySetSolver();

            LoadBalancesCommand = new DelegateCommand(LoadBalances);
        }

        public DelegateCommand LoadBalancesCommand { get; }

        public FuzzyResultModel Altman
        {
            get { return _altman; }
            set
            {
                _altman = value;
                OnPropertyChanged();
            }
        }

        public FuzzyResultModel Mamdani
        {
            get { return _mamdani; }
            set
            {
                _mamdani = value;
                OnPropertyChanged();
            }
        }

        public FuzzyResultModel FuzzySet
        {
            get { return _fuzzySet; }
            set
            {
                _fuzzySet = value;
                OnPropertyChanged();
            }
        }

        public double[] Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadBalances(object sender)
        {
            var dialogWindow = new OpenFileDialog
            {
                Filter = "Text Files|*.txt",
                Title = "Please, select file with financial data"
            };
            if (dialogWindow.ShowDialog() == true)
            {
                var balances =
                    File.ReadAllLines(dialogWindow.FileName)
                        .Select(x => x.Split(new[] {' ', '\t', '\n'}, StringSplitOptions.RemoveEmptyEntries))
                        .Where(x => x.Any())
                        .Select(x => x.Last())
                        .Select(double.Parse)
                        .ToArray();

                Parameters = balances.ToParameters();
                Altman = new FuzzyResultModel(_altmanSolver.Solve(balances));
                Mamdani = new FuzzyResultModel(_mamdaniSolver.Solve(balances));
                FuzzySet = new FuzzyResultModel(_fuzzySetSolver.Solve(balances));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}