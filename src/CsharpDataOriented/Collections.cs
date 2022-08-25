using System.Collections;

namespace CsharpDataOriented;

public static class Collections
{
    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
        => source ?? Enumerable.Empty<T>();

    public static IEnumerable<IEnumerable<T>> Partition<T>(
        this IEnumerable<T> source,
        int size,
        int? step = default,
        IEnumerable<T> pad = null)
    {
        var part = source.OrEmpty().Take(size);

        if (part.Count() < size)
            return new[] { part.Concat(pad.OrEmpty().Take(size - part.Count())) };

        return new[] { part }
            .Concat(source.Skip(step ?? size).Partition(size, step, pad))
            .Where(part => part.Any());

    }

    public static T Nth<T>(this IEnumerable source, int n)
        => source.Nth<T>(n, null);

    public static T Nth<T>(this IEnumerable source, int n, T notFound)
        where T : struct
        => source.Nth(n, () => notFound);

    public static T Nth<T>(this IEnumerable source, int n, Func<T>? notFound = null)
    {
        var getDefault = notFound ?? new Func<T>(() => throw new ArgumentOutOfRangeException(nameof(n)));

        var actualSource = source.Cast<object>();

        if (n < 0)
            return actualSource
                .Partition(size: n * -1, step: 1)
                .Where(part => part.Count() == n * -1)
                .LastOrDefault()
                .OrEmpty()
                .Reverse()
                .Nth((n * -1) - 1, getDefault);

        var firstN = actualSource.Take(n + 1);

        return firstN.Count() == n + 1 ? firstN.Skip(n).Cast<T>().First() : getDefault();
    } 
}
