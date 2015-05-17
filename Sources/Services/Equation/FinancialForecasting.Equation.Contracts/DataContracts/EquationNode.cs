namespace FinancialForecasting.Equation.DataContracts
{
    public class EquationNode
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsResult { get; set; }

        public bool IsK1Enabled { get; set; }

        public bool IsK2Enabled { get; set; }

        public bool IsK3Enabled { get; set; }
    }
}