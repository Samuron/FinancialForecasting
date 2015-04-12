namespace FinancialForecasting.Migration.Entities
{
    public class EnterpriseIndex
    {
        public int Id { get; set; }

        public double X1 { get; set; }

        public double X2 { get; set; }

        public double X3 { get; set; }

        public double X4 { get; set; }

        public double X5 { get; set; }

        public double X6 { get; set; }

        public double X7 { get; set; }

        public string EnterpriseId { get; set; }

        public virtual Enterprise Enterprise { get; set; }
    }
}