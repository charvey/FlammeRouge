namespace FlammeRouge.Core;

public abstract class Player
{
    protected readonly Dictionary<RiderType, RiderDeck> decks = new()
    {
        { RiderType.Rouleur, RiderDeck.CreateRouleur() },
        { RiderType.Sprinteur, RiderDeck.CreateSprinteur() }
    };


    public abstract Dictionary<RiderType, int> PickStarting();
    public abstract Dictionary<RiderType, Card> PickCards();
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

    public override Dictionary<RiderType, Card> PickCards()
    {
        var order = Enum.GetValues<RiderType>();
        Random.Shared.Shuffle(order);

        return order.ToDictionary(x => x, PickCard);
    }

    private Card PickCard(RiderType riderType)
    {
        var hand = decks[riderType].Draw();
        var card = Random.Shared.GetItems(hand.ToArray(), 1).Single();
        decks[riderType].Recycle(hand.Except([card]).ToList());
        return card;
    }
}