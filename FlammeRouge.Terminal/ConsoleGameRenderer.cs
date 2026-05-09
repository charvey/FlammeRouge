using FlammeRouge.Core;

namespace FlammeRouge.Terminal;

public class ConsoleGameRenderer
{
    public void Print(Game game)
    {
        Console.Clear();
        Console.WriteLine(new string('-', game.Track.Length + 2));
        for (var i = 0; i < game.Track.Length; i++)
        {
            if (i == game.StartingLine || i == game.FinishingLine)
                Console.Write('|');
            PrintRider(game.Track[i].Left);
        }

        Console.WriteLine();

        Console.WriteLine(new string('-', game.Track.Length + 2));
        for (var i = 0; i < game.Track.Length; i++)
        {
            if (i == game.StartingLine || i == game.FinishingLine)
                Console.Write('|');
            PrintRider(game.Track[i].Right);
        }

        Console.WriteLine();
        Console.WriteLine(new string('-', game.Track.Length + 2));
    }

    private void PrintRider(Rider? rider)
    {
        if (rider is null)
        {
            Console.Write(' ');
            return;
        }

        Console.ForegroundColor = rider.Color switch
        {
            Color.Red => ConsoleColor.Red,
            Color.Green => ConsoleColor.Green,
            Color.Blue => ConsoleColor.Blue,
            Color.Black => ConsoleColor.Black,
            _ => throw new ArgumentOutOfRangeException()
        };
        Console.Write(rider.RiderType switch
        {
            RiderType.Rouleur => 'R',
            RiderType.Sprinteur => 'S',
            _ => throw new ArgumentOutOfRangeException()
        });
        Console.ResetColor();
    }
}