using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public class GameTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void GameCantHaveInvalidNumberOfPlayers(int playerCount)
    {
        var colors = Enumerable.Range(0, playerCount).Select(i => (Color)i);

        Assert.Throws<ArgumentOutOfRangeException>(() => new Game([..colors]));
    }

    [Fact]
    public void GameHasTrackInitialized()
    {
        Assert.Equal(9 * 6 + 12 * 2, new Game([Color.Black]).Track.Length);
    }

    [Fact]
    public void CantPlaceRiderOnOccupiedSquare()
    {
        var game = new Game([Color.Blue, Color.Green]);

        game.PlaceRider(Color.Blue, RiderType.Rouleur, 0);
        game.PlaceRider(Color.Blue, RiderType.Sprinteur, 0);

        Assert.Throws<InvalidOperationException>(() => game.PlaceRider(Color.Green, RiderType.Rouleur, 0));
    }
}