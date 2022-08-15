using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented;

public record SeqProp(string Name, Func<object, object?> GetValue);

public class Seq : IEnumerable<object>
{
    private IEnumerable<object> actualSeq;

    public Seq(IEnumerable<object> actualSeq)
    {
        this.actualSeq = actualSeq;
    }

    public IEnumerator<object> GetEnumerator()
        => actualSeq.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => actualSeq.GetEnumerator();
}
