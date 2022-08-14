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
        this PropSeq seq,
        string[] path)
    {
        var getValue = DefMulti((PropSeq propSeq) => default(W), DispatchByProp<PropSeq>(ignoreProps: new[] { nameof(PropSeq.Name) }))
            .DefMethod(nameof(PropSeq.Complex), (propSeq) => { throw new InvalidOperationException($"Ilegal access"); })
            .DefMethod(nameof(PropSeq.Primitive), (propSeq) => (W)propSeq.Primitive);

        var leaf = path
            .OrEmpty()
            .Aggregate(seq, (acc, curr) => acc.Complex.First(p => p.Name == curr));

        return getValue.Invoke(leaf);
    }
}
