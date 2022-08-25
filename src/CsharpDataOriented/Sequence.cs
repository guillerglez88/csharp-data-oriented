using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented;

public static class Sequence
{
    public static Seq S(params object[] items)
        => new(items);

    public static Seq Seq<T>(T arg)
        where T : notnull
        => ObjectSequencer.Seq(arg);

    public static W? Get<W>(
        this Seq seq,
        params string[] path)
    {
        var leaf = path
            .OrEmpty()
            .Aggregate(seq, (acc, curr) => (Seq)(acc
                .Cast<Seq>()
                .First(seq => Equals(seq.Nth<string>(0), curr))
                .Last()));

        return leaf.Nth<W>(0);
    } 
}
