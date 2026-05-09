using FlammeRouge.Core;
using FlammeRouge.Terminal;

var game = new Game([new RandomPlayer(), new RandomPlayer()]);
var renderer = new ConsoleGameRenderer();
var gameRunner = new GameRunner(game, renderer.Print);

gameRunner.RunGameLoop();