using System.Collections.Generic;

namespace FinancialForecasting.DataAccess
{
    public interface IDataAccessService
    {
        IEnumerable<EnterpriseDto> GetEntrprises();

        IEnumerable<EntetpriseIndexDto> GetIndexes();
    }
}