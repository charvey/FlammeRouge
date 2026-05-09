namespace FlammeRouge.Core;

public class GameRunner(Game game, Action<Game> renderAction)
{
    public void RunGameLoop()
    {
        //assign starters
        foreach (var player in game.Players)
        {
            var spots = player.Player.PickStarting();
            foreach (var spot in spots)
                game.PlaceRider(player.Color, spot.Key, spot.Value);
        }

        while (!game.IsOver)
        {
            //energy phase
            var selections = new Dictionary<Color, Dictionary<RiderType, Card>>();
            foreach (var player in game.Players)
                selections[player.Color] = player.Player.PickCards();

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
                }
            }

            //end phase
            //todo slipstream
            //todo exhaustion

            renderAction(game);
        }
    }
}