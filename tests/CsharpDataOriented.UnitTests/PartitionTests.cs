using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented.UnitTests;

public class PartitionTests
{
    [Fact]
    public void CanPartition()
    {
        var range = Enumerable.Range(0, 11);

        var parts = range
            .Partition(size: 3)
            .Select(part => $"[{string.Join(", ", part)}]")
            .ToList();

        Assert.Contains("[0, 1, 2]", parts);
        Assert.Contains("[3, 4, 5]", parts);
        Assert.Contains("[6, 7, 8]", parts);
        Assert.Contains("[9, 10]", parts);
    }

    [Fact]
    public void CanPartitionLinkedListStyle()
    {
        var range = Enumerable.Range(0, 11);

        var parts = range
            .Partition(size: 2, step: 1)
            .Select(part => $"[{string.Join(", ", part)}]")
            .ToList();

        Assert.Contains("[0, 1]", parts);
        Assert.Contains("[1, 2]", parts);
        Assert.Contains("[2, 3]", parts);
        Assert.Contains("[3, 4]", parts);
        Assert.Contains("[4, 5]", parts);
        Assert.Contains("[5, 6]", parts);
        Assert.Contains("[6, 7]", parts);
        Assert.Contains("[7, 8]", parts);
        Assert.Contains("[8, 9]", parts);
        Assert.Contains("[9, 10]", parts);
        Assert.Contains("[10]", parts);
    }

    [Fact]
    public void CanPartitionRingStyle()
    {
        var range = Enumerable.Range(0, 11);

        var parts = range
            .Partition(size: 3, step: 2, range)
            .Select(part => $"[{string.Join(", ", part)}]")
            .ToList();

        Assert.Contains("[0, 1, 2]", parts);
        Assert.Contains("[2, 3, 4]", parts);
        Assert.Contains("[4, 5, 6]", parts);
        Assert.Contains("[6, 7, 8]", parts);
        Assert.Contains("[8, 9, 10]", parts);
    }

    [Fact]
    public void CanPartitionSpiralStyle()
    {
        var range = Enumerable.Range(0, 11);

        var parts = range
            .Partition(size: 3, step: 1, range)
            .Select(part => $"[{string.Join(", ", part)}]")
            .ToList();

        Assert.Contains("[0, 1, 2]", parts);
        Assert.Contains("[1, 2, 3]", parts);
        Assert.Contains("[2, 3, 4]", parts);
        Assert.Contains("[3, 4, 5]", parts);
        Assert.Contains("[4, 5, 6]", parts);
        Assert.Contains("[5, 6, 7]", parts);
        Assert.Contains("[6, 7, 8]", parts);
        Assert.Contains("[8, 9, 10]", parts);
        Assert.Contains("[9, 10, 0]", parts);
    }
}
