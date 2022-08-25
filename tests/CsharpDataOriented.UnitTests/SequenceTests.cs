using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpDataOriented.Sequence;

namespace CsharpDataOriented.UnitTests;

public class SequenceTests
{
    [Fact]
    public void CanGetPropByPath()
    {
        var personSeq = Seq(Fixture.Person);
        var lastName = personSeq.Get<string>("Name", "Family");
        var addrLine = personSeq.Get<string>("Addresses", "0", "Lines", "0");

        Assert.Equal("Rodriguez", lastName);
        Assert.Equal("184 #42301, esq 423", addrLine);
    }
}
