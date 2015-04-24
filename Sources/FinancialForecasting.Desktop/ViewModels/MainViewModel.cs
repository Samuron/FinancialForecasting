namespace FinancialForecasting.Desktop.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Migration = new MigrationViewModel();
            Solving = new SolvingViewModel();
        }

        public MigrationViewModel Migration { get; }

        public SolvingViewModel Solving { get; }
    }
}