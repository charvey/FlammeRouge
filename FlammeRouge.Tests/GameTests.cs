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
}