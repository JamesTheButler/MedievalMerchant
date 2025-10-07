using System.Linq;
using Common;

namespace Data.Towns.Production.Logic
{
    public static class ProducerExtensions
    {
        public static int GetProducerCount(this ProductionManager self, Tier tier)
        {
            return self.GetProducers(tier).WhereNotNull().Count();
        }
    }
}