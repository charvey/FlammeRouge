using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public class GameRunnerTests
{
    [Fact]
    public void GameFinishes()
    {
        var game = new Game([new RandomPlayer(), new RandomPlayer()]);

        new GameRunner(game, _ => { }).RunGameLoop();

        Assert.True(game.IsOver);
    }
}