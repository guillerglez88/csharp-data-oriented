using CsharpMultimethod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static CsharpMultimethod.Multi;
using static CsharpDataOriented.BasicFuncs;
namespace CsharpDataOriented;

public static class Sequence
{
    public const string GET_MULTI_DISPATCHING_VAL_IDENTITY = nameof(GET_MULTI_DISPATCHING_VAL_IDENTITY);
    public const string GET_MULTI_DISPATCHING_VAL_GET_ALL = nameof(GET_MULTI_DISPATCHING_VAL_GET_ALL);
    public const string GET_MULTI_DISPATCHING_VAL_SINGLE_PROP = nameof(GET_MULTI_DISPATCHING_VAL_SINGLE_PROP);
    public const string GET_MULTI_DISPATCHING_VAL_ALL_PROPS = nameof(GET_MULTI_DISPATCHING_VAL_ALL_PROPS);
    public const string GET_MULTI_DISPATCHING_VAL_BY_EXAMPLE = nameof(GET_MULTI_DISPATCHING_VAL_BY_EXAMPLE);

    private static Func<Seq, object[], IEnumerable<Seq>> get;

    static Sequence()
    {
        var getMulti = DefMulti(
            contract: ((Seq seq, object[] path) arg) => default(IEnumerable<Seq>),
            dispatch: (arg) => DispatchGet(arg.seq, arg.path));

        getMulti
            .DefMethod(GET_MULTI_DISPATCHING_VAL_IDENTITY, (arg) => new[] { arg.seq })
            .DefMethod(GET_MULTI_DISPATCHING_VAL_SINGLE_PROP, (arg) => GetProp(arg.seq, (string)arg.path.First()))
            .DefMethod(GET_MULTI_DISPATCHING_VAL_ALL_PROPS, (arg) => GetProps(arg.seq, arg.path))
            .DefMethod(GET_MULTI_DISPATCHING_VAL_GET_ALL, (arg) => GetAll(arg.seq));

        get = (seq, path) => getMulti.Invoke((seq, path));
    }

    public static Seq S(params object[] items)
        => new(items);

    public static Seq Seq<T>(T arg)
        where T : notnull
        => ObjectSequencer.Seq(arg);

    public static Seq Get(
        this Seq seq,
        params object[] path)
    {
        var actualPath = path.OrEmpty()
            .Select(cmp => cmp is null || cmp is string ? cmp : Seq(cmp))
            .ToArray();

        var result = get(seq, path);

        if (!result.OrEmpty().Any())
            return null;

        if (result.OrEmpty().Count() == 1)
            return result.First();

        return Seq(result);
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

    private static string DispatchGet(Seq seq, object[] path)
    {
        if (seq is null)
            return GET_MULTI_DISPATCHING_VAL_IDENTITY;
        if (!path.OrEmpty().Any())
            return GET_MULTI_DISPATCHING_VAL_IDENTITY;
        if (path.First() is null)
            return GET_MULTI_DISPATCHING_VAL_GET_ALL;
        if (path.First() is Seq)
            return GET_MULTI_DISPATCHING_VAL_BY_EXAMPLE;
        if (path.Length == 1)
            return GET_MULTI_DISPATCHING_VAL_SINGLE_PROP;

        return GET_MULTI_DISPATCHING_VAL_ALL_PROPS;
    }

    private static IEnumerable<Seq> GetProp(Seq seq, string prop)
    {
        return seq
            .Cast<Seq>()
            .Where(seq => Equals(seq.Nth<string>(0), prop))
            .Select(seq => seq.Nth<Seq>(1))
            .Take(1)
            .ToArray();
    }

    private static IEnumerable<Seq> GetAll(Seq seq)
    {
        return Enumerable
            .Range(0, int.MaxValue)
            .Select(i => get(seq, new[] { $"{i}" }))
            .TakeWhile(s => s.Any())
            .SelectMany(Identity)
            .ToArray();
    }

    private static IEnumerable<Seq> GetProps(Seq seq, object[] path)
    {
        return path.Aggregate(new[] { seq }, (acc, curr) => acc
            .Cast<Seq>()
            .SelectMany(s => get(s, new[] { curr }))
            .Cast<Seq>()
            .ToArray());
    }

    private static IEnumerable<Seq> GetByExample(Seq seq, Seq example)
    {
        throw new NotImplementedException();
    }
}
