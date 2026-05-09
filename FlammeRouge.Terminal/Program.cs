using FlammeRouge.Core;
using FlammeRouge.Terminal;

var renderer = new ConsoleGameRenderer();
var gameRunner = new GameRunner(new Dictionary<Color, Player>
{
    { Color.Red, new RandomPlayer() }, { Color.Blue, new RandomPlayer() }
}, g =>
{
    renderer.Print(g);
    Thread.Sleep(TimeSpan.FromSeconds(0.5));
});

gameRunner.RunGame();