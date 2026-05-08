namespace FlammeRouge.Core;

public class RiderDeck
{
    private const int HAND_SIZE = 4;

    public RiderDeck(Card[] cards)
    {
        Random.Shared.Shuffle(cards);
        Cards = cards.ToList();
    }

    private List<Card> Cards { get; }
    public List<Card> Discard { get; } = [];

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

    public void AddExhaustion()
    {
        Discard.Add(new ExhaustionCard());
    }
}