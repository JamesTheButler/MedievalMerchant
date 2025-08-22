using System.Collections.Generic;

public static class CollectionExtensions
{
    public static T PickRandom<T>(this IList<T> source)
    {
        if (source == null || source.Count == 0)
        {
            return default;
        }

        var index = UnityEngine.Random.Range(0, source.Count);
        return source[index];
    }
}