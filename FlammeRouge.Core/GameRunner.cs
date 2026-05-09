namespace FlammeRouge.Core;

public class GameRunner(Dictionary<Color, Player> players, Action<Game> renderAction)
{
    public Game RunGame()
    {
        var game = new Game([..players.Keys]);

        //assign starters
        foreach (var player in game.Colors)
        {
            var spots = players[player].PickStarting();
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
            foreach (var player in game.Colors)
                selections[player] = players[player].PickCards();

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
            //todo slipstream
            //todo exhaustion

            renderAction(game);
        }

        return game;
    }
}