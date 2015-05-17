namespace FinancialForecasting.Desktop.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Migration = new MigrationViewModel();
            Solving = new SolvingViewModel();
            Fuzzy = new FuzzyViewModel();
        }

        public MigrationViewModel Migration { get; }

        public SolvingViewModel Solving { get; }

        public FuzzyViewModel Fuzzy { get; }
    }
}