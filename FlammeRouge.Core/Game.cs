namespace FlammeRouge.Core;

public record struct Square(Rider? Right, Rider? Left)
{

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
	private const int STARTING_LINE = 5;

	public void PlaceRider(Color color, RiderType riderType, int row)
	{
		if (row is < 0 or >= STARTING_LINE) throw new ArgumentOutOfRangeException(nameof(row));

		Track[row].Place(new Rider(color, riderType));


	}
	
	public Square[] Track { get; }
	public (Color Color,Player Player)[] Players { get; }

	public Game(Player[] players)
	{
		if (players.Length == 0 || players.Length > Enum.GetValues<Color>().Length)
			throw new ArgumentOutOfRangeException(nameof(players));
		Players = players.Select((player, i) => ((Color)i, player)).ToArray();

		Track= new Square[9 * 6 + 12 * 2];
	}
}

public class RiderDeck
{
	private const int HAND_SIZE = 4;

	public static RiderDeck CreateSprinteur()
	{
		return new RiderDeck(Array.Empty<int>()
			.Concat(Enumerable.Repeat(2, 3))
			.Concat(Enumerable.Repeat(3, 3))
			.Concat(Enumerable.Repeat(4, 3))
			.Concat(Enumerable.Repeat(5, 3))
			.Concat(Enumerable.Repeat(9, 3))
			.Select(e => new EnergyCard(e))
			.ToArray<Card>()
		);
	}
	
	public static RiderDeck CreateRouleur()
	{
		return new RiderDeck(Array.Empty<int>()
			.Concat(Enumerable.Repeat(3, 3))
			.Concat(Enumerable.Repeat(4, 3))
			.Concat(Enumerable.Repeat(5, 3))
			.Concat(Enumerable.Repeat(6, 3))
			.Concat(Enumerable.Repeat(7, 3))
			.Select(e => new EnergyCard(e))
			.ToArray<Card>()
		);
	}
	
	
	public RiderDeck(Card[] cards)
	{
		Random.Shared.Shuffle(cards);
		Cards = cards.ToList();
	}

	private List<Card> Cards { get; }
	public List<Card> Discard { get; } = [];

	public List<Card> Draw()
	{
		if (Cards.Count < HAND_SIZE && Discard.Any())
		{
			var discard = Discard.ToArray();
			Random.Shared.Shuffle(discard);
			Cards.AddRange(discard);
			Discard.Clear();
		}

		var handSize = Math.Min(Cards.Count, HAND_SIZE);

		var hand = Cards.Take(handSize).ToList();
		Cards.RemoveRange(0, handSize);
		return hand;
	}

	public void Recycle(List<Card> cards)
	{
		Discard.AddRange(cards);
	}

	public void AddExhaustion() => Discard.Add(new ExhaustionCard());
}

public abstract record Card
{
	public abstract int Energy { get; }
}

public record ExhaustionCard : Card
{
	public override int Energy => 2;
}

public record EnergyCard(int energy) : Card
{
	public override int Energy => energy;
}

public abstract class Player
{
	public abstract Dictionary<RiderType,int> PickStarting();
}

public class RandomPlayer : Player
{
	public override Dictionary<RiderType, int> PickStarting()
	{
		return new()
		{
			{ RiderType.Rouleur, Random.Shared.Next(5) },
			{ RiderType.Sprinteur, Random.Shared.Next(5) }
		};


	}
}

public enum Color
{
	Red, Green, Blue, Black
}

public enum RiderType
{
	Sprinteur, Rouleur
}