using System.Collections.Generic;

namespace FinancialForecasting.Migration.Entities
{
    public class Enterprise
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<EnterpriseIndex> EnterpriseIndices { get; set; }
    }
}