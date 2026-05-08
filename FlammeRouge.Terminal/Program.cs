using FlammeRouge.Core;

var g = new Game([new RandomPlayer(), new RandomPlayer()]);

//assign starters
foreach (var player in g.Players)
{
    var spots=player.Player.PickStarting();
    foreach (var spot in spots)
        g.PlaceRider(player.Color,spot.Key,spot.Value);
}


while (true)
{
    //energy phase

    
    //movement phase
    
    //end phase

    Print(g);
}

void Print(Game game)
{
    Console.Clear();
    foreach (var s in game.Track)
    {
        //TODO print start and finish lines
        Console.Write('|');
        PrintRider(s.Right);
        Console.Write('|');
        PrintRider(s.Left);
        Console.Write('|');
        Console.WriteLine();
    }
}

void PrintRider(Rider? rider)
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