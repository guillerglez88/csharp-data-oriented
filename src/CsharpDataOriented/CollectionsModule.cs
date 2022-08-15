using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using static CsharpMultimethod.PropBasedMultiExtensions;
using static CsharpMultimethod.MultimethodsModule;

namespace CsharpDataOriented;

public static class CollectionsModule
{
    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
        => source ?? Enumerable.Empty<T>();

    public static W? Get<W>(
        this Seq seq,
        string[] path)
    {
        var leaf = path
            .OrEmpty()
            .Aggregate(seq, (acc, curr) => (Seq)(acc
                .Cast<Seq>()
                .First(seq => Equals((string)seq.First(), curr))
                .Last()));

        return leaf.Cast<W>().First();
    }
}
