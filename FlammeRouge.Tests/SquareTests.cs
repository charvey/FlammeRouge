using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public class SquareTests
{
    [Fact]
    public void SquareStartsWithSpace()
    {
        Assert.True(new Square().HasSpace);
    }
    
    [Fact]
    public void SquareWithOneRiderPlacedStillHasSpace()
    {
        var subject = new Square();

        subject.Place(new(Color.Blue, RiderType.Rouleur));
        
        Assert.True(subject.HasSpace);
    }
    
    [Fact]
    public void SquareWithTwoRidersPlacedDoesNotHaveSpace()
    {
        var subject = new Square();

        subject.Place(new(Color.Blue, RiderType.Rouleur));
        subject.Place(new(Color.Blue, RiderType.Sprinteur));
        
        Assert.False(subject.HasSpace);
    }
}