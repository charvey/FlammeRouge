namespace FlammeRouge.Core;

public abstract class Player
{
    private readonly Dictionary<RiderType, RiderDeck> decks = new()
    {
        { RiderType.Rouleur, RiderDeck.CreateRouleur() },
        { RiderType.Sprinteur, RiderDeck.CreateSprinteur() }
    };

    //TODO make game aware to avoid placing on top
    protected abstract IEnumerable<RiderType> PickStartingRiderOrder();
    protected abstract int PickStartingRiderSquare(RiderType riderType);

    public IEnumerable<(RiderType Type, int Square)> PickStarting()
    {
        return PickStartingRiderOrder().Select(r => (r, PickStartingRiderSquare(r)));
    }

    //TODO make situation aware
    protected abstract IEnumerable<RiderType> PickEnergyRiderOrder();
    protected abstract Card PickEnergyCard(IReadOnlyList<Card> hand);

    public Dictionary<RiderType, Card> PickCards()
    {
        var result = new Dictionary<RiderType, Card>();
        foreach (var riderType in PickEnergyRiderOrder())
        {
            var hand = decks[riderType].Draw();
            var card = PickEnergyCard(hand);
            result[riderType] = card;
            decks[riderType].Recycle(hand.Except([card]).ToList());
        }

        return result;
    }
}

public class RandomPlayer : Player
{
    protected override IEnumerable<RiderType> PickStartingRiderOrder()
    {
        var order = Enum.GetValues<RiderType>();
        Random.Shared.Shuffle(order);
        return order;
    }

    protected override int PickStartingRiderSquare(RiderType riderType)
    {
        return Random.Shared.Next(5); //todo reference game
    }

    protected override IEnumerable<RiderType> PickEnergyRiderOrder()
    {
        return PickStartingRiderOrder();
    }

    protected override Card PickEnergyCard(IReadOnlyList<Card> hand)
    {
        return Random.Shared.GetItems(hand.ToArray(), 1).Single();
    }
}

public class MinPlayer : Player
{
    protected override IEnumerable<RiderType> PickStartingRiderOrder()
    {
        return Enum.GetValues<RiderType>();
    }

    protected override int PickStartingRiderSquare(RiderType riderType)
    {
        return 0;
    }

    protected override IEnumerable<RiderType> PickEnergyRiderOrder()
    {
        return Enum.GetValues<RiderType>();
    }

    protected override Card PickEnergyCard(IReadOnlyList<Card> hand)
    {
        return hand.MinBy(c => c.Energy) ?? throw new InvalidOperationException();
    }
}

public class MaxPlayer : Player
{
    protected override IEnumerable<RiderType> PickStartingRiderOrder()
    {
        return Enum.GetValues<RiderType>();
    }

    protected override int PickStartingRiderSquare(RiderType riderType)
    {
        return 4; //todo reference game
    }

    protected override IEnumerable<RiderType> PickEnergyRiderOrder()
    {
        return Enum.GetValues<RiderType>();
    }

    protected override Card PickEnergyCard(IReadOnlyList<Card> hand)
    {
        return hand.MaxBy(c => c.Energy) ?? throw new InvalidOperationException();
    }
}