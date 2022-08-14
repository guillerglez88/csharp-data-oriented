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
        var seq = Seq(person.GetType());

        var objColl = seq(person);

        Assert.Equal(3, objColl.Count());
    }

    [Fact]
    public void CanGetPropByPath()
    {
        var seq = Seq(person.GetType());

        var personSeq = seq(person);
        var lastName = personSeq.Get<string>(path: new[] { "Name", "Family" });

        Assert.Equal("Rodriguez", lastName);
    }

    [Fact]
    public void CanRecognizeSeqTypes()
    {
        var nameArr = new[] { "Glenis", "Mayra" };
        var nameList = nameArr.ToList();
        var nameDict = nameArr.ToDictionary(n => n);

        Assert.True(LeaveAsIs(nameArr));
        Assert.True(LeaveAsIs(nameList));
        Assert.True(LeaveAsIs(nameDict));
    }
}
