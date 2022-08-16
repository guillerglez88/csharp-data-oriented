using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsharpDataOriented;

namespace CsharpDataOriented.UnitTests;

public class NthTests
{
    [Fact]
    public void CanGetNthElem()
    {
        var range = Enumerable.Range(0, 5);

        var first = range.Nth<int>(0);
        var last = range.Nth<int>(4);

        Assert.Equal(0, first);
        Assert.Equal(4, last);
    }

    [Fact]
    public void CanGetLastNthElem()
    {
        var range = Enumerable.Range(0, 5);

        var last = range.Nth<int>(-1);
        var nextToLast = range.Nth<int>(-2);

        Assert.Equal(4, last);
        Assert.Equal(3, nextToLast);
    }

    [Fact]
    public void CanGetNthElemOrDefaultVal()
    {
        var range = Enumerable.Range(0, 5);

        var notFound = range.Nth(6, 100);

        Assert.Equal(100, notFound);
    }

    [Fact]
    public void ThrowsOnOutOfRange()
    {
        var range = Enumerable.Range(0, 5);

        var throwsPositiveOutOfRange = new Action(() => { range.Nth<int>(6); });
        var throwsNegativeOutOfRange = new Action(() => { range.Nth<int>(-6); });

        Assert.Throws<ArgumentOutOfRangeException>(throwsPositiveOutOfRange);
        Assert.Throws<ArgumentOutOfRangeException>(throwsNegativeOutOfRange);
    }
}
