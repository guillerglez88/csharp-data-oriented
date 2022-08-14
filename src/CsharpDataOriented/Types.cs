using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented;

public class Seq : IEnumerable<object>
{
    private IEnumerable<object> actualSeq;

    public Seq(IEnumerable<object> actualSeq)
    {
        this.actualSeq = actualSeq;
    }

    public IEnumerator GetEnumerator()
        => actualSeq.GetEnumerator();

    IEnumerator<object> IEnumerable<object>.GetEnumerator()
        => actualSeq.GetEnumerator();
}
