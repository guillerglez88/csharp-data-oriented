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
        var givenName = personSeq
            .Get("Name", "Given")
            .Cast<Seq>()
            .Select(item => item.Nth<Seq>(1).Nth<string>(0));

        Assert.Equal("Rodriguez", lastName);
        Assert.Equal("184 #42301, esq 423", addrLine);
        Assert.Contains("Ruben", givenName);
    }

    [Fact]
    public void CanGetAllByPath()
    {
        var personSeq = Seq(Fixture.Person);

        var postalCodes = personSeq.Get("Addresses", null, "PostalCode");
        var strPostalCodes = $"[{string.Join(", ", postalCodes.Cast<Seq>().Select(item => item.Nth<string>(0)))}]";

        Assert.Equal("[18100, 11100]", strPostalCodes);
    }

    [Fact]
    public void CanExtendSeq()
    {
        var dob = new DateTime(2009, 10, 3);
        var personSeq = Seq(Fixture.Person);

        var extended = personSeq.With(new
        {
            DoB = dob,
            Gender = "Male"
        });

        Assert.Equal(dob, extended.Get<DateTime>("DoB"));
        Assert.Equal("Male", extended.Get<string>("Gender"));
        Assert.Equal("Rodriguez", extended.Get<string>("Name", "Family"));
    }
}
