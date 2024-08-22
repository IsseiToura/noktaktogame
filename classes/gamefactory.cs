using assessment2;
namespace assessment2
{
    public class GameFactory
    {
        private string gameType;
        public GameFactory(string gameType)
        {
            this.gameType = gameType;
        }

        public Game GetGame()
        {
            if (gameType == "Noktakto")
            {
                return new Noktakto();
            }
            // else if (gameType == "Gomoku")
            // {
            //     return new Gomoku();
            // }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

}