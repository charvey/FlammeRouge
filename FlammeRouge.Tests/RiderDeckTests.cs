using FlammeRouge.Core;

namespace FlammeRouge.Tests;

public class RiderDeckTests
{
    private RiderDeck Create(params int[] energy)
    {
        return new RiderDeck(energy.Select(e => new EnergyCard(e)).ToArray<Card>());
    }

    [Fact]
    public void DrawReturnsCards()
    {
        var deck = new RiderDeck([new EnergyCard(2), new EnergyCard(3), new EnergyCard(4), new EnergyCard(5)]);

        var hand = deck.Draw();

        Assert.Equal(4, hand.Count);
        Assert.Contains(hand, c => c.Energy == 2);
        Assert.Contains(hand, c => c.Energy == 3);
        Assert.Contains(hand, c => c.Energy == 4);
        Assert.Contains(hand, c => c.Energy == 5);
    }

    [Fact]
    public void RecycleCardsShuffledBackIn()
    {
        var deck = Create(2, 3, 4, 5);
        var hand = deck.Draw();

        deck.Recycle(hand.Skip(1).ToList());

        Assert.Equivalent(deck.Draw(), hand.Skip(1));
    }

    [Fact]
    public void DiscardStartsEmpty()
    {
        var deck = new RiderDeck([]);

        Assert.Empty(deck.Discard);
    }

    [Fact]
    public void ExhaustionCardsAddedToDiscard()
    {
        var deck = new RiderDeck([]);

        deck.AddExhaustion();

        Assert.IsType<ExhaustionCard>(deck.Discard.Single());
    }

    [Fact]
    public void DrawReturnsAllAvailable()
    {
        var deck = new RiderDeck([new EnergyCard(2), new EnergyCard(3)]);

        var hand = deck.Draw();

        Assert.Equal(2, hand.Count);
        Assert.Contains(hand, c => c.Energy == 2);
        Assert.Contains(hand, c => c.Energy == 3);
    }
}