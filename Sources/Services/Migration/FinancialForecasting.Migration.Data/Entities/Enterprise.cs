using System.Collections.Generic;

namespace FinancialForecasting.Migration.Entities
{
    public class Enterprise
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<EnterpriseIndex> EnterpriseIndices { get; set; }
    }
}