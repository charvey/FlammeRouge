using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public class GameTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void GameCantHaveInvalidNumberOfPlayers(int playerCount)
    {
        var players = Enumerable.Range(0, playerCount).Select(_ => new RandomPlayer()).ToArray();

        Assert.Throws<ArgumentOutOfRangeException>(() => new Game(players));
    }

    [Fact]
    public void GameHasTrackInitialized()
    {
        Assert.Equal(9 * 6 + 12 * 2, new Game([new RandomPlayer()]).Track.Length);
    }

    [Fact]
    public void CantPlaceRiderOnOccupiedSquare()
    {
        var game = new Game([new RandomPlayer(), new RandomPlayer()]);

        game.PlaceRider(Color.Blue, RiderType.Rouleur, 0);
        game.PlaceRider(Color.Blue, RiderType.Sprinteur, 0);

        Assert.Throws<InvalidOperationException>(() => game.PlaceRider(Color.Green, RiderType.Rouleur, 0));
    }
}