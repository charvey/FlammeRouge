namespace FlammeRouge.Core;

public abstract class Player
{
    protected abstract IEnumerable<RiderType> PickStartingRiderOrder(Game game);
    protected abstract int PickStartingRiderSquare(Game game, RiderType riderType);

    public IEnumerable<(RiderType Type, int Square)> PickStarting(Game game)
    {
        return PickStartingRiderOrder(game).Select(r => (r, PickStartingRiderSquare(game, r)));
    }

    //TODO make situation aware
    protected abstract IEnumerable<RiderType> PickEnergyRiderOrder();
    protected abstract Card PickEnergyCard(IReadOnlyList<Card> hand);

    public Dictionary<RiderType, Card> PickCards(Dictionary<RiderType, RiderDeck> decks)
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
    protected override IEnumerable<RiderType> PickStartingRiderOrder(Game game)
    {
        var order = Enum.GetValues<RiderType>();
        Random.Shared.Shuffle(order);
        return order;
    }

    protected override int PickStartingRiderSquare(Game game, RiderType riderType)
    {
        int row;
        do
        {
            row = Random.Shared.Next(game.StartingLine);
        } while (!game.Track[row].HasSpace);

        return row;
    }

    protected override IEnumerable<RiderType> PickEnergyRiderOrder()
    {
        var order = Enum.GetValues<RiderType>();
        Random.Shared.Shuffle(order);
        return order;
    }

    protected override Card PickEnergyCard(IReadOnlyList<Card> hand)
    {
        return Random.Shared.GetItems(hand.ToArray(), 1).Single();
    }
}

public class MinPlayer : Player
{
    protected override IEnumerable<RiderType> PickStartingRiderOrder(Game game)
    {
        return Enum.GetValues<RiderType>();
    }

    protected override int PickStartingRiderSquare(Game game, RiderType riderType)
    {
        return game.Track.Select((s, i) => (s, i))
            .Where(x => x.i < game.StartingLine)
            .Where(x => x.s.HasSpace)
            .Min(x => x.i);
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
    protected override IEnumerable<RiderType> PickStartingRiderOrder(Game game)
    {
        return Enum.GetValues<RiderType>();
    }

    protected override int PickStartingRiderSquare(Game game, RiderType riderType)
    {
        return game.Track.Select((s, i) => (s, i))
            .Where(x => x.i < game.StartingLine)
            .Where(x => x.s.HasSpace)
            .Max(x => x.i);
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