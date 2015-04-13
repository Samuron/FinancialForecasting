using System.Data;
using FinancialForecasting.Migration.Entities;

namespace FinancialForecasting.Migration
{
    internal static class EnterpriseIndexFactory
    {
        public static EnterpriseIndex ReadEnterpriseIndex(this IDataRecord reader, Enterprise enterprise)
        {
            return new EnterpriseIndex
            {
                X1 = reader.GetDouble(0),
                X2 = reader.GetDouble(1),
                X3 = reader.GetDouble(2),
                X4 = reader.GetDouble(3),
                X5 = reader.GetDouble(4),
                X6 = reader.GetDouble(5),
                X7 = reader.GetDouble(6),
                Enterprise = enterprise
            };
        }
    }
}