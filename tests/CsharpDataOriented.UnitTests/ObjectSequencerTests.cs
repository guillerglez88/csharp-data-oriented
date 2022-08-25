using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpDataOriented.Sequence;

namespace CsharpDataOriented.UnitTests;

public class ObjectSequencerTests
{ 
    [Fact]
    public void CanListClassProps()
    {
        var objColl = Seq(Fixture.Person);

        var result = objColl.ToList();

        Assert.Equal(3, result.Count());
    } 
}
