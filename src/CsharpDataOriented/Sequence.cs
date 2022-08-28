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

    public static Seq? Get(
        this Seq seq,
        params string[] path)
    {
        if (seq is null)
            return seq;

        if (!path.OrEmpty().Any())
            return seq;

        if (path.First() is null)
        {
            return S(Enumerable
                .Range(0, int.MaxValue)
                .Select(i => seq.Get($"{i}"))
                .TakeWhile(s => s != null)
                .Cast<Seq>()
                .ToArray());
        }

        if (path.Length == 1)
        {
            return (Seq)seq
                .Cast<Seq>()
                .FirstOrDefault(seq =>
                    Equals(seq.Nth<string>(0), path.First()))?
                .Last();
        }

        var result = path.Aggregate(S(seq), (acc, curr) => S(acc
            .Cast<Seq>()
            .SelectMany(s => curr is null 
                ? s.Get(curr)
                : S(s.Get(curr)))
            .Cast<Seq>()
            .ToArray()));

        return result.Count() == 1 ? result.Nth<Seq>(0) : result;
    }

    public static Seq With(
        this Seq seq,
        Seq values)
    {
        var seqProps = seq.Cast<Seq>().Select(prop => prop.Nth<string>(0));
        var valuesProps = values.Cast<Seq>().Select(prop => prop.Nth<string>(0));
        var props = seqProps.Concat(valuesProps).Distinct();

        var items = props
            .Select(prop => S(prop, values.Get(prop) ?? seq.Get(prop)))
            .ToList();

        return new Seq(items);
    }
}
