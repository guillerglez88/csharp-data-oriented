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

    [Fact]
    public void CanExtendSeq()
    {
        var dob = new DateTime(2009, 10, 3);
        var personSeq = Seq(Fixture.Person);

        var extended = personSeq.With(new {
            DoB = dob,
            Gender = "Male" });

        Assert.Equal(dob, extended.Get<DateTime>("DoB"));
        Assert.Equal("Male", extended.Get<string>("Gender"));
        Assert.Equal("Rodriguez", extended.Get<string>("Name", "Family"));
    }
}
