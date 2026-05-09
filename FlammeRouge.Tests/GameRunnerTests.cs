using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public class GameRunnerTests
{
    [Fact]
    public void GameFinishes()
    {
        var subject = new GameRunner(new Dictionary<Color, Player>
        {
            { Color.Blue, new RandomPlayer() },
            { Color.Green, new MaxPlayer() },
            { Color.Red, new MinPlayer() }
        }, _ => { });

        var game = subject.RunGame();

        Assert.True(game.IsOver);
    }
}