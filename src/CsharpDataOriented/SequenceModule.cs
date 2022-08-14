﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented;

public static class SequenceModule
{
    public static Func<T, Seq> Seq<T>(T sampleType)
        => (arg) => Seq(typeof(T)).Invoke(arg);

    public static Func<T, Seq> Seq<T>()
        => (arg) => Seq(typeof(T)).Invoke(arg);

    public static Func<object, Seq> Seq(Type type)
    {
        var props = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
            .Select(prop => new { name = prop.Name, getter = BuildGetter(prop) });

        return (obj) =>
        {
            var complex = props.Select(prop =>
            {
                var val = prop.getter.Invoke(obj);

                if (val is null || LeaveAsIs(val))
                    return new Seq(new object[] { prop.name, new Seq(new[] { val }) });

                var seqVal = Seq(val.GetType()).Invoke(val);

                return new Seq(new object[] { prop.name, seqVal });
            });

            return new Seq(complex);
        };
    }

    public static Func<object, object?> BuildGetter(PropertyInfo prop)
    {
        var param = Expression.Parameter(typeof(object), "obj");
        var castParam = Expression.Convert(param, prop.DeclaringType);
        var memberAccess = Expression.MakeMemberAccess(castParam, prop);
        var castResult = Expression.Convert(memberAccess, typeof(object));
        var getter = Expression.Lambda<Func<object, object?>>(castResult, param);

        return getter.Compile();
    }

    public static bool LeaveAsIs<T>(T obj)
        => obj.GetType().IsPrimitive
        || typeof(IEnumerable).IsAssignableFrom(obj.GetType())
        || typeof(DateTime).IsAssignableFrom(obj.GetType());
}
