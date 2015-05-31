namespace FinancialForecasting.Desktop.Extensions
{
    public static class BankruptcyDecision
    {
        public static string GetDecision(double forecast)
        {
            if (forecast < 0.4)
                return "великий ризик банкрутства";
            if (forecast < 0.7)
                return "сердній ризик банкуртсва";
            return "низький ризик банкуртсва";
        }
    }
}