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
    var x = new Dictionary<Color, Dictionary<RiderType, Card>>();
    foreach (var player in g.Players)
    {
        x[player.Color] = player.Player.PickCards();
    }
    
    //movement phase
    for (var i = g.Track.Length - 1; i >= 0; i--)
    {
        var square = g.Track[i];
        if (square.Right is not null)
        {
            var card = x[square.Right.Color][square.Right.RiderType];
            var target=i+card.Energy;
            while (!g.Track[target].HasSpace)
            {
                target--;
            }
            g.Track[target].Place(g.Track[i].Right);
            g.Track[i].Right = null;
        }
        if (square.Left is not null)
        {
            var card = x[square.Left.Color][square.Left.RiderType];
            var target=i+card.Energy;
            while (!g.Track[target].HasSpace)
            {
                target--;
            }
            g.Track[target].Place(g.Track[i].Left);
            g.Track[i].Left = null;
        }
    }
    
    //end phase
    //todo slipstream
    //todo exhaustion
    
    //Thread.Sleep(TimeSpan.FromSeconds(5));
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