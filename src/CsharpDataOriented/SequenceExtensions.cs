using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpDataOriented.Sequence;

namespace CsharpDataOriented;

public static class SequenceExtensions
{
    public static T Get<T>(
        this Seq seq,
        params object[] path)
    {
        var result = seq.Get(path);

        return result.Nth<T>(0);
    }

    public static Seq With<T>(
        this Seq seq,
        T values)
        where T : class
        => seq.With(Seq(values));
}
