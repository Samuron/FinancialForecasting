namespace FinancialForecasting.Desktop
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Migration = new MigrationViewModel();
        }

        public MigrationViewModel Migration { get; set; }
    }
}