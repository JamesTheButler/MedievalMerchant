using System;
using System.Collections.Generic;
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

        public static IReadOnlyDictionary<Tier, int> GetProducerCount(this Producer producer)
        {
            var result = new Dictionary<Tier, int>();

            foreach (Tier tier in Enum.GetValues(typeof(Tier)))
            {
                result.Add(tier, producer.GetProducers(tier).WhereNotNull().Count());
            }

            return result;
        }

        public static IReadOnlyDictionary<Tier, Good[]> GetProducers(this Producer producer)
        {
            var result = new Dictionary<Tier, Good[]>();

            foreach (Tier tier in Enum.GetValues(typeof(Tier)))
            {
                result.Add(tier, producer.GetProducers(tier).WhereNotNull().ToArray());
            }

            return result;
        }
    }
}