namespace FlammeRouge.Core;

public class GameRunner(Dictionary<Color, Player> players, Action<Game> renderAction)
{
    public Game RunGame()
    {
        var game = new Game([..players.Keys]);

        //assign starters
        foreach (var player in game.Colors)
        {
            var spots = players[player].PickStarting(game);
            foreach (var spot in spots)
            {
                game.PlaceRider(player, spot.Type, spot.Square);
                renderAction(game);
            }
        }

        while (!game.IsOver)
        {
            //energy phase
            var selections = new Dictionary<Color, Dictionary<RiderType, Card>>();
            foreach (var color in game.Colors)
                selections[color] = players[color].PickCards(game.Decks[color]);

            //movement phase
            for (var i = game.Track.Length - 1; i >= 0; i--)
            {
                var square = game.Track[i];
                if (square.Right is not null)
                {
                    var rider = square.Right;
                    var card = selections[square.Right.Color][square.Right.RiderType];
                    var target = i + card.Energy;
                    while (!game.Track[target].HasSpace)
                        target--;
                    game.Track[target].Place(rider);
                    game.Track[i].Right = null;

                    renderAction(game);
                }

                if (square.Left is not null)
                {
                    var rider = square.Left;
                    var card = selections[square.Left.Color][square.Left.RiderType];
                    var target = i + card.Energy;
                    while (!game.Track[target].HasSpace)
                        target--;
                    game.Track[target].Place(rider);
                    game.Track[i].Left = null;

                    renderAction(game);
                }
            }

            //end phase
            //slipstream
            for (var i = 0; i < game.Track.Length - 2; i++)
                if (game.Track[i].HasRider && !game.Track[i + 1].HasRider && game.Track[i + 2].HasRider)
                {
                    var j = i;
                    while (game.Track[j].HasRider)
                    {
                        game.Track[j + 1].Left = game.Track[j].Left;
                        game.Track[j].Left = null;
                        game.Track[j + 1].Right = game.Track[j].Right;
                        game.Track[j].Right = null;
                        i--;
                        renderAction(game);
                    }
                }

            //exhaustion
            for (var i = 0; i < game.Track.Length - 1; i++)
                if (game.Track[i].HasRider && !game.Track[i + 1].HasRider)
                {
                    var rightRider = game.Track[i].Right;
                    if (rightRider is not null) game.Decks[rightRider.Color][rightRider.RiderType].AddExhaustion();

                    var leftRider = game.Track[i].Left;
                    if (leftRider is not null) game.Decks[leftRider.Color][leftRider.RiderType].AddExhaustion();
                }
        }

        return game;
    }
}