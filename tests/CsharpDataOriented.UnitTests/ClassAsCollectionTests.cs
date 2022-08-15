using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpDataOriented.SequenceModule;
using static CsharpDataOriented.CollectionsModule;

namespace CsharpDataOriented.UnitTests;

public class ClassAsCollectionTests
{
    private readonly object person = new {
        Name = new {
            Given = new[] { "Glen", "Ruben" },
            Family = "Rodriguez" },
        DoB = new DateTime(2011, 10, 3),
        Addresses = new[] { 
            new { 
                Lines = new[] { "184 #42301, esq 423" },
                PostalCode = "18100",
                Period = new { Start = new DateTime(2019, 04, 17) } } } };

    [Fact]
    public void CanListClassProps()
    {
        var objColl = Seq(person);

        var result = objColl.ToList();

        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void CanGetPropByPath()
    {
        var personSeq = Seq(person);
        var lastName = personSeq.Get<string>(path: new[] { "Name", "Family" });
        var addrLine = personSeq.Get<string>(path: new[] { "Addresses", "0", "Lines", "0" });

        Assert.Equal("Rodriguez", lastName);
        Assert.Equal("184 #42301, esq 423" , addrLine);
    }
}
