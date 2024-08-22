using assessment2;
namespace assessment2
{

    public class UI
    {
        public static int SelectGameMode()
        {
            while(true)
            {
                Console.WriteLine("Select Game Mode:");
                Console.WriteLine("1: Human vs Human");
                Console.WriteLine("2: Human vs Computer");
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int mode) && (mode ==1 || mode ==2))
                {
                    return mode;
                }
                else
                {
                    Console.WriteLine("This input is invalid");
                }
            }
        }
        public static void WelcomeMessage()
        {
            Console.WriteLine("Let's start the game!");
            Console.WriteLine();
        }
        public static bool IsLoad()
        {
            while(true)
            {
                Console.WriteLine("Do you want to load a previous game? (Y or N)");
                string input = Console.ReadLine().Trim().ToUpper();
                if(input == "Y")
                {
                    return true;
                }
                else if(input == "N")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid value. Please enter 'Y' or 'N'");
                }
            }
        }

        public static void DisplayBoard(Board[] boards)
        {
            int gridSize = boards[0].GetGrid().GetLength(0);
            for (int b = 0; b < boards.Length; b++)
            {
                Console.Write($"Board{b + 1}");
                if (b < boards.Length - 1)
                {
                    Console.Write("      ");
                }
            }
            Console.WriteLine();

            for(int r = 0; r < gridSize; r++)
            {
                for(int b = 0; b < boards.Length; b++)
                {
                    char[,] grid = boards[b].GetGrid();
                    for(int c = 0; c < gridSize; c++)
                    {
                        char cell = grid[r, c];
                        Console.Write(cell == '\0' ? ' ' : cell);

                        if(c < gridSize - 1)
                        {
                            Console.Write(" | ");
                        }
                    }
                    if(b < boards.Length - 1)
                    {
                        Console.Write("   "); 
                    }
                }
                Console.WriteLine();

                if(r < gridSize - 1)
                {
                    for(int b = 0; b < boards.Length; b++)
                    {
                        for(int c = 0; c < gridSize; c++)
                        {
                            Console.Write("--");
                            if(c < gridSize - 1)
                            {
                                Console.Write("--");
                            }
                        }
                        if(b < boards.Length - 1)
                        {
                            Console.Write("  "); 
                        }
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        public static Move GetMoveFromHumanPlayer(HumanPlayer player)
        {
            while(true)
            {
                try
                {
                    Console.WriteLine($"{player.Name}");
                    Console.WriteLine("Enter your move as 'Board Index,row,col' (e.g., 1,1,2), or type 'help' for instractions.");
                    string input = Console.ReadLine().Trim().ToLower();
                    if(input == "help")
                    {
                        HelpSystem.ShowHelp();
                        continue;
                    }

                    string[] parts = input.Split(',');

                    if(parts.Length != 3)
                    {
                        Console.WriteLine("Invalid input. Please enter as 'Board Index,row,col'");
                        continue;
                    }
                    int boardIndex = int.Parse(parts[0]) -1;
                    int row = int.Parse(parts[1]) -1;
                    int col = int.Parse(parts[2]) -1;

                    return new Move
                    {
                        BoardIndex = boardIndex,
                        Row = row,
                        Col = col
                    };
                }

                catch(FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter numbers only");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"An Error occured: {ex.Message}");
                }
            }
        }

        public static bool IsMoveConfirmed()
        {
            while(true)
            {
                Console.WriteLine("Do you confirm the move? (Y or N)");
                string input = Console.ReadLine().Trim().ToUpper();
                if(input == "Y")
                {
                    return true;
                }
                else if(input == "N")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid value. Please enter 'Y' or 'N'");
                }
            }
        }
        public static bool IsRedo()
        {
            while(true)
            {
                Console.WriteLine("Do you redo? (Y or N)");
                string input = Console.ReadLine().Trim().ToUpper();
                if(input == "Y")
                {
                    return true;
                }
                else if(input == "N")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid value. Please enter 'Y' or 'N'");
                }
            }
        }
        public static bool IsSave()
        {
            while(true)
            {
                Console.WriteLine("Do you want to save a game? (Y or N)");
                string input = Console.ReadLine().Trim().ToUpper();
                if(input == "Y")
                {
                    return true;
                }
                else if(input == "N")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid value. Please enter 'Y' or 'N'");
                }
            }
        }
    }
}