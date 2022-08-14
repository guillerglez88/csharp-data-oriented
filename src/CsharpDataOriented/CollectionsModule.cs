using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace CsharpDataOriented;
public class CollectionsModule
{
    public static Func<T, IEnumerable<object?>> Seq<T>(T sampleType)
        => (arg) => Seq(typeof(T)).Invoke(arg);

    public static Func<T, IEnumerable<object?>> Seq<T>()
        => (arg) => Seq(typeof(T)).Invoke(arg);

    public static Func<object, IEnumerable<object?>> Seq(Type type)
    {
        var props = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
            .Select(prop => new { name = prop.Name, getter = BuildGetter(prop) });

        return (obj) => props
            .Select(prop =>
            {
                var val = prop.getter.Invoke(obj);

                if (val is null || LeaveAsIs(val))
                    return new object[] { prop.name, val };

                var seqVal = Seq(val.GetType()).Invoke(val);

                return new object[] { prop.name, seqVal };
            });
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
