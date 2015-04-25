using System.Collections.Generic;
using Excel;
using FinancialForecasting.Migration.Entities;

namespace FinancialForecasting.Migration
{
    internal class ExcelEnterpriseIndexEnumerator
    {
        private readonly FinancialForecastingContext _context;

        public ExcelEnterpriseIndexEnumerator(FinancialForecastingContext context)
        {
            _context = context;
        }

        public IEnumerable<EnterpriseIndex> GetEnterpriseIndices(IExcelDataReader reader)
        {
            reader.Read();
            while (reader.Read())
            {
                var enterprise = new Enterprise {Id = reader.GetString(7), Name = reader.GetString(8)};
                yield return
                    new EnterpriseIndex
                    {
                        Y = reader.GetDouble(0),
                        X1 = reader.GetDouble(1),
                        X2 = reader.GetDouble(2),
                        X3 = reader.GetDouble(3),
                        X4 = reader.GetDouble(4),
                        X5 = reader.GetDouble(5),
                        X6 = reader.GetDouble(6),
                        Enterprise = _context.Enterprises.Find(enterprise.Id) ?? enterprise
                    };
            }
        }
    }
}