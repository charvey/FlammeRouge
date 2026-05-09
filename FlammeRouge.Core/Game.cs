namespace FlammeRouge.Core;

public record struct Square(Rider? Right, Rider? Left)
{
    public bool HasSpace => Right is null && Left is null;

    public void Place(Rider rider)
    {
        if (Right is null)
        {
            Right = rider;
            return;
        }

        if (Left is null)
        {
            Left = rider;
            return;
        }

        throw new InvalidOperationException(nameof(rider));
    }
}

public record Rider(Color Color, RiderType RiderType);

public class Game
{
    public Game(Player[] players)
    {
        if (players.Length == 0 || players.Length > Enum.GetValues<Color>().Length)
            throw new ArgumentOutOfRangeException(nameof(players));
        Players = players.Select((player, i) => ((Color)i, player)).ToArray();

        Track = new Square[9 * 6 + 12 * 2];
    }

    public int StartingLine => 5;
    public int FinishingLine => Track.Length - 5;

    public Square[] Track { get; }
    public (Color Color, Player Player)[] Players { get; }

    public bool IsOver
    {
        get
        {
            for (var i = FinishingLine; i < Track.Length; i++)
                if (!Track[i].HasSpace)
                    return true;

            return false;
        }
    }

    public void PlaceRider(Color color, RiderType riderType, int row)
    {
        if (row < 0 || row >= StartingLine) throw new ArgumentOutOfRangeException(nameof(row));

        Track[row].Place(new Rider(color, riderType));
    }
}

public enum Color
{
    Red,
    Green,
    Blue,
    Black
}

public enum RiderType
{
    Sprinteur,
    Rouleur
}