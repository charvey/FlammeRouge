namespace FlammeRouge.Core;

public record struct Square(Rider? Right, Rider? Left)
{
    public bool HasRider => Right is not null || Left is not null;
    public bool HasSpace => Right is null || Left is null;

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

        throw new InvalidOperationException($"Can't place {nameof(rider)} on an occupied square");
    }
}

public record Rider(Color Color, RiderType RiderType);

public class Game
{
    public Game(HashSet<Color> colors)
    {
        if (colors.Count == 0 || colors.Count > Enum.GetValues<Color>().Length)
            throw new ArgumentOutOfRangeException(nameof(colors));
        Colors = colors.ToArray();
        Random.Shared.Shuffle(Colors);

        Track = new Square[9 * 6 + 12 * 2];

        Decks = colors.ToDictionary(c => c, _ => new Dictionary<RiderType, RiderDeck>
        {
            { RiderType.Rouleur, RiderDeck.CreateRouleur() },
            { RiderType.Sprinteur, RiderDeck.CreateSprinteur() }
        });
    }

    public int StartingLine => 5;
    public int FinishingLine => Track.Length - 5;

    public Square[] Track { get; }
    public Color[] Colors { get; }

    public IReadOnlyDictionary<Color, Dictionary<RiderType, RiderDeck>> Decks { get; }

    public bool IsOver
    {
        get
        {
            for (var i = FinishingLine; i < Track.Length; i++)
                if (Track[i].HasRider)
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