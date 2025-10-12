using System.Linq;
using Common;
using Common.Types;

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