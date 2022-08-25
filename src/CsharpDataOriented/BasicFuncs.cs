using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented;

public static class BasicFuncs
{
    public static T Identity<T>(T arg)
        => arg;

    public static void Ignore<T>(T _arg) { }

    public static Func<T, W> Memoize<T, W>(Func<T, W> compu)
        where T : notnull
    {
        var cache = new ConcurrentDictionary<T, W>();

        return (arg) => cache.GetOrAdd(arg, compu);
    } 

    public static void Require<T>()
        where T : new()
        => Ignore(new T());
}
