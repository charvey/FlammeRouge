using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public abstract class PlayerTests
{
    protected abstract Player subject { get; }

    [Fact]
    public void StartingSquaresAreValid()
    {
        var game = new Game([Color.Blue]);

        var starting = subject.PickStarting(game);

        Assert.All(starting.Select(s => s.Square), s => Assert.True(0 <= s && s < game.StartingLine));
    }

    [Fact]
    public void StartingPositionsCoverAllRiderTypes()
    {
        var game = new Game([Color.Blue]);

        var starting = subject.PickStarting(game);

        Assert.Equivalent(Enum.GetValues<RiderType>(), starting.Select(s => s.Type));
    }

    [Fact]
    public void StartingPositionsDontConflict()
    {
        var game = new Game([Color.Blue, Color.Black, Color.Green, Color.Red]);
        game.PlaceRider(Color.Blue, RiderType.Rouleur, 0);
        game.PlaceRider(Color.Blue, RiderType.Sprinteur, 0);
        game.PlaceRider(Color.Black, RiderType.Rouleur, game.StartingLine - 1);
        game.PlaceRider(Color.Black, RiderType.Sprinteur, game.StartingLine - 1);
        game.PlaceRider(Color.Green, RiderType.Rouleur, game.StartingLine / 2);
        game.PlaceRider(Color.Green, RiderType.Sprinteur, game.StartingLine / 2);

        var starting = subject.PickStarting(game);

        foreach (var placement in starting)
        {
            Assert.True(game.Track[placement.Square].HasSpace);
            game.PlaceRider(Color.Red, placement.Type, placement.Square);
        }
    }
}

public class RandomPlayerTests : PlayerTests
{
    protected override Player subject { get; } = new RandomPlayer();
}

public class MinPlayerTests : PlayerTests
{
    protected override Player subject { get; } = new MinPlayer();

    [Fact]
    public void PicksBackRowWhenAvailable()
    {
        var color = Color.Blue;
        var game = new Game([color]);

        foreach (var pick in subject.PickStarting(game))
            game.PlaceRider(color, pick.Type, pick.Square);

        Assert.True(game.Track[0].Left?.Color == color);
        Assert.True(game.Track[0].Right?.Color == color);
    }
}

public class MaxPlayerTests : PlayerTests
{
    protected override Player subject { get; } = new MaxPlayer();

    [Fact]
    public void PicksFrontRowWhenAvailable()
    {
        var color = Color.Blue;
        var game = new Game([color]);

        foreach (var pick in subject.PickStarting(game))
            game.PlaceRider(color, pick.Type, pick.Square);

        Assert.True(game.Track[game.StartingLine - 1].Left?.Color == color);
        Assert.True(game.Track[game.StartingLine - 1].Right?.Color == color);
    }
}