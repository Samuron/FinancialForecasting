namespace FinancialForecasting.Migration.Entities
{
    public class EnterpriseIndex
    {
        public int Id { get; set; }

        public double X1 { get; set; }

        public double X2 { get; set; }

        public int EnterpriseId { get; set; }

        public virtual Enterprise Enterprise { get; set; }
    }
}