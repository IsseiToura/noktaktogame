using assessment2;
namespace assessment2
{
    public abstract class Game
    {
        protected int gameMode;
        protected Board[] boards;
        protected Player player1;
        protected char player1Mark;
        protected Player player2;
        protected char player2Mark;
        protected MoveHistory moveHistory;
        protected int currentPlayerIndex;
        protected IHelpSystem helpSystem;   
        private readonly string saveFilepath = Path.Combine(Environment.CurrentDirectory, "savefile.log"); 
        public void SetGame()
        {
            gameMode = UI.SelectGameMode();
            player1 = new HumanPlayer {Name = "Player1"};
            if(gameMode == 1)
            {
                player2 = new HumanPlayer {Name = "Player2"};
            }
            else
            {
                player2 = new ComputerPlayer {Name = "Computer"};
            }
        }

        public abstract void Start();
        
        public abstract bool IsGameOver();
        
        protected void SwitchPlayer()
        {
            currentPlayerIndex = 1 - currentPlayerIndex; 
        }
        
        protected Player GetCurrentPlayer()
        {
            return currentPlayerIndex == 0 ? player1 : player2;
        }
        
        protected char GetCurrentMark()
        {
            return currentPlayerIndex == 0 ? player1Mark : player2Mark;
        }
        
        public void ShowHelp()
        {
            helpSystem.ShowHelp();
        }
        public void Save()
        {
            try
            {
                using(StreamWriter writer = new StreamWriter(saveFilepath))
                {
                    writer.WriteLine(gameMode);
                    writer.WriteLine(currentPlayerIndex);
                    foreach(var board in boards)
                    {
                        char[,] grid = board.GetGrid();
                        for(int r = 0; r < grid.GetLength(0); r++)
                        {
                            for(int c = 0; c < grid.GetLength(1); c++)
                            {
                                writer.Write(grid[r,c]);
                            }
                        }
                        writer.WriteLine();
                    }
                }
                Console.WriteLine("Game saved successfully!");
                Console.WriteLine("Exit the game....");
                Environment.Exit(0);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occured while saving a game: {ex.Message}");
            }
        }
        
        public void Load()
        {
            try
            {
                if(!File.Exists(saveFilepath))
                {
                    Console.WriteLine("No saved games.");
                    return;
                }

                using(StreamReader reader = new StreamReader(saveFilepath))
                {
                    int saveGameMode =int.Parse(reader.ReadLine());
                    if (saveGameMode != gameMode)
                    {
                        throw new InvalidOperationException("The loaded game mode does not match the current game mode.");
                    }
                    currentPlayerIndex = 1 - int.Parse(reader.ReadLine());
                    for(int b = 0; b < boards.Length; b++)
                    {
                        char[,] grid = boards[b].GetGrid();
                        string line = reader.ReadLine();

                        int lineIndex = 0;
                        for(int r = 0; r < grid.GetLength(0); r++)
                        {
                            for(int c = 0; c < grid.GetLength(1); c++)
                            {
                                grid[r,c] = line[lineIndex];
                                lineIndex++;
                            }
                        }
                    }
                    Console.WriteLine("Game loaded successfully.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occured while loading a game: {ex.Message}");
            }
        }
    }
}
