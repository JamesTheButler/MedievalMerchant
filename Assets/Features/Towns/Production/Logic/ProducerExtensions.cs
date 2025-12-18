using System.Linq;
using Common.Types;
using Common.Utility;

namespace Features.Towns.Production.Logic
{
    public static class ProducerExtensions
    {
        public static int GetProducerCount(this ProductionManager self, Tier tier)
        {
            return self.GetProducers(tier).WhereNotNull().Count();
        }
    }
}