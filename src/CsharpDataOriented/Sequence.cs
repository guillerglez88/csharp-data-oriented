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

    public static Seq Get(
        this Seq seq,
        params string[] path)
    {
        var leaf = path
            .OrEmpty()
            .Aggregate(seq, (acc, curr) =>
                acc is null ? acc : (Seq)(acc
                .Cast<Seq>()
                .FirstOrDefault(seq => Equals(seq.Nth<string>(0), curr))?.Last()));

        return leaf;
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
