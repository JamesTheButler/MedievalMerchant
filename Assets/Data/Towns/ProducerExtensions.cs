using System.Linq;
using Common;

namespace Data.Towns
{
    public static class ProducerExtensions
    {
        public static int GetProducerCount(this Producer producer, Tier tier)
        {
            return producer.GetProducers(tier).WhereNotNull().Count();
        }
    }
}