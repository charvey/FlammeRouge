namespace FlammeRouge.Core;

public abstract class Card
{
    public abstract int Energy { get; }
}

public class ExhaustionCard : Card
{
    public override int Energy => 2;
}

public class EnergyCard(int energy) : Card
{
    public override int Energy => energy;
}