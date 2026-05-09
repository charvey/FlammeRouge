using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public abstract class PlayerTests
{
    protected abstract Player subject { get; }

    [Fact]
    public void StartingSquaresAreValid()
    {
        var game = new Game([subject]);

        var starting = subject.PickStarting();

        Assert.All(starting.Select(s => s.Square), s => Assert.True(0 <= s && s < game.StartingLine));
    }

    [Fact]
    public void StartingPositionsCoverAllRiderTypes()
    {
        var game = new Game([subject]);

        var starting = subject.PickStarting();

        Assert.Equivalent(Enum.GetValues<RiderType>(), starting.Select(s => s.Type));
    }
}

public class RandomPlayerTests : PlayerTests
{
    protected override Player subject { get; } = new RandomPlayer();
}

public class MinPlayerTests : PlayerTests
{
    protected override Player subject { get; } = new RandomPlayer();
}

public class MaxPlayerTests : PlayerTests
{
    protected override Player subject { get; } = new RandomPlayer();
}