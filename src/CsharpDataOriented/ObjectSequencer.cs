using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.Linq.Expressions;

using static CsharpMultimethod.Multi;
using static CsharpMultimethod.TypeBasedMultiExtensions;
using static CsharpDataOriented.BasicFuncs;

namespace CsharpDataOriented;

public class ObjectSequencer
{
    private static readonly Func<Type, object, IEnumerable<SeqProp>?> getProps;

    static ObjectSequencer()
    {
        var memoGetPropsComplex = Memoize((Type t) => GetPropsComplex(t));

        var getPropsMulti = DefMulti(
                contract: ((Type type, object val) arg) => default(IEnumerable<SeqProp>),
                dispatch: (arg) => DispatchByType(arg.type));

        getPropsMulti
            .DefMethod("primitive", (_) => GetPropsPrimitive())
            .DefMethod("dict", (arg) => GetPropsDict((IDictionary<string, dynamic>)arg.val))
            .DefMethod("collection", (arg) => GetPropsCollection((IEnumerable<dynamic>)arg.val))
            .DefMethod("complex", (arg) => memoGetPropsComplex.Invoke(arg.type));

        getProps = (type, val) => getPropsMulti.Invoke((type, val));
    } 

    public static Seq Seq<T>(T arg)
        where T : notnull
    {
        if (arg is Seq seq)
            return seq;

        var props = getProps(arg.GetType(), arg)
            .Select(prop => (
                name: prop.Name,
                isLeaf: prop.Name == ".",
                value: prop.GetValue(arg)))
            .Select(prop => prop.isLeaf
                ? prop.value
                : new Seq(new[] { prop.name, Seq<dynamic>((dynamic)prop.value) }));

        return new Seq(props);
    }

    private static string DispatchByType(Type type)
    {
        if (type.IsPrimitive
            || typeof(DateTime).IsAssignableFrom(type)
            || typeof(string).IsAssignableFrom(type))
            return "primitive";
        if (typeof(IDictionary<,>).IsAssignableFrom(type))
            return "dict";
        if (typeof(IEnumerable).IsAssignableFrom(type))
            return "collection";

        return "complex";
    }

    private static IEnumerable<SeqProp> GetPropsPrimitive() => new[] { new SeqProp(
        Name: ".",
        GetValue: Identity) };

    private static IEnumerable<SeqProp> GetPropsDict<T>(IDictionary<string, T> dict) => dict.Select(e => new SeqProp(
        Name: e.Key,
        GetValue: (_) => dict[e.Key]));

    private static IEnumerable<SeqProp> GetPropsCollection<T>(IEnumerable<T> source) => source.Select((e, i) => new SeqProp(
        Name: $"{i}",
        GetValue: (_) => e));

    private static IEnumerable<SeqProp> GetPropsComplex(Type type) => type
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
        .Select(prop => new SeqProp(
            Name: prop.Name,
            GetValue: BuildGetter(prop)));

    private static Func<object, object?> BuildGetter(PropertyInfo prop)
    {
        var param = Expression.Parameter(typeof(object), "obj");
        var castParam = Expression.Convert(param, prop.DeclaringType);
        var memberAccess = Expression.MakeMemberAccess(castParam, prop);
        var castResult = Expression.Convert(memberAccess, typeof(object));
        var getter = Expression.Lambda<Func<object, object?>>(castResult, param);

        return getter.Compile();
    }
}
