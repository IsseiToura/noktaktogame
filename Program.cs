using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace assessment2;

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
    private readonly string saveFilepath = @"C:\Users\issei\OneDrive - Queensland University of Technology\IFN563 Object Oriented Design\assessment2\savefile.log"; 
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

public class Noktakto : Game
{
    public Noktakto()
    {
        int gridSize = 3;
        this.boards = new Board[3] { new Board(gridSize), new Board(gridSize), new Board(gridSize) };
        this.moveHistory = new MoveHistory();
        this.currentPlayerIndex = 0;
        this.player1Mark = 'X';
        this.player2Mark = 'X';
    }

    public override void Start()
    {
        bool isLoadGame =UI.IsLoad();
        if(isLoadGame)
        {
            int gridSize = 3;
            this.boards = new Board[3] { new Board(gridSize), new Board(gridSize), new Board(gridSize) };
            Load();
        }
        else
        {
            UI.WelcomeMessage();
        }

        while(!IsGameOver())
        {
            UI.DisplayBoard(boards);
            var player = GetCurrentPlayer();
            var move = player.MakeMove(boards);
            if (move.IsValid(boards))
            {
                char currentMark = GetCurrentMark();
                move.Execute(boards, currentMark);
                moveHistory.RecordMove(move);
                UI.DisplayBoard(boards);
                if(IsGameOver())
                {
                    UI.DisplayBoard(boards);
                    Console.WriteLine("{0} loses a game!", player.Name);
                    break;
                }

                if(player is HumanPlayer)
                {
                    bool isMoveFinish;
                    do
                    {
                        isMoveFinish = UI.IsMoveConfirmed();
                        if(!isMoveFinish)
                        {
                            moveHistory.Undo(boards);
                            bool isRedo = UI.IsRedo();
                            if(!isRedo)
                            {
                                move = player.MakeMove(boards);
                                if(move.IsValid(boards))
                                {
                                    move.Execute(boards, currentMark);
                                    moveHistory.RecordMove(move);
                                }
                            }
                            else
                            {
                                moveHistory.Redo(boards, currentMark);
                            }

                        }
                    }while(!isMoveFinish);
                }

                bool isSaveGame = UI.IsSave();
                if(isSaveGame)
                {
                    Save();
        
                }
                else
                {
                    SwitchPlayer();
                }
            }
            else
            {
                Console.WriteLine("This move is invalid. Try again");
            }
        }       
    }

    public override bool IsGameOver()
    {
        foreach (var board in boards)
        {
            if(CheckThreeInRow(board))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckThreeInRow(Board board)
    {
        char[,] grid = board.GetGrid();
        for (int row = 0; row < 3 ; row++)
        {
            if(grid[row,0] != '\0' && grid[row,0] == grid[row,1] && grid[row,1] == grid[row,2])
            {
                return true;
            }
        }

        for (int col = 0; col < 3 ; col++)
        {
            if(grid[0,col] != '\0' && grid[0,col] == grid[1,col] && grid[1,col] == grid[2,col])
            {
                return true;
            }
        }

        if(grid[0,0] != '\0' && grid[0,0] == grid[1,1] && grid[1,1] == grid[2,2])
        {
            return true;
        }

        if(grid[0,2] != '\0' && grid[0,2] == grid[1,1] && grid[1,1] == grid[0,2])
        {
            return true;
        }
        
        return false;
    }
}

public abstract class Player
{
    public string Name { get; set;}
    public abstract Move MakeMove(Board[] boards);
}

public class HumanPlayer : Player
{
    public override Move MakeMove(Board[] boards)
    {
        return UI.GetMoveFromHumanPlayer(this);
    }
}

public class ComputerPlayer : Player
{
    public override Move MakeMove(Board[] boards)
    {
        return Move.GetRandomValidMove(boards);
    }   
}
public class Move
{
    public int BoardIndex { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public bool IsValid(Board[] boards)
    {
        if(BoardIndex < 0 || BoardIndex >= boards.Length)
        {
            return false;
        }

        char[,] grid = boards[BoardIndex].GetGrid();
        if(Row <0 || Row >= grid.GetLength(0) || Col < 0 || Col >= grid.GetLength(1))
        {
            return false;
        }

        if(grid[Row, Col] != '\0')
        {
            return false;
        }

        return true;
    }
    public void Execute(Board[] boards, char playerMark)
    {
        if(IsValid(boards)) 
        {
            char[,] grid = boards[BoardIndex].GetGrid();
            grid[Row,Col] = playerMark;
        }
    }
    public static Move GetRandomValidMove(Board[] boards)
    {
        Random random = new Random();
        List<Move> validMoves = new List<Move>();

        for(int b = 0; b < boards.Length; b++ )
        {
            char[,] grid = boards[b].GetGrid();
            for(int r = 0; r < grid.GetLength(0); r++)
            {
                for(int c = 0; c < grid.GetLength(0); c++)
                {
                    if(grid[r, c] == '\0')
                    {
                        validMoves.Add(new Move {BoardIndex = b, Row = r, Col = c});
                    }

                }
            }
        }
        if(validMoves.Count > 0)
        {
            int index = random.Next(validMoves.Count);
            return validMoves[index]; 
        }
        else
        {
            Console.WriteLine("There's no point to put a piece.");
            return null;
        }
    }
}

public class MoveHistory
{
    private Stack<Move> history = new Stack<Move>();
    private Stack<Move> redoStack = new Stack<Move>();
    public void RecordMove(Move move)
    {
        history.Push(move);
        redoStack.Clear();
    }
    public void Undo(Board[] boards)
    {
        if (history.Count >0 )
        {
            var move = history.Pop();
            redoStack.Push(move);

            char[,] grid = boards[move.BoardIndex].GetGrid(); 
            grid[move.Row, move.Col] = '\0';
        }
    }
    public void Redo(Board[] boards, char playerMark)
    {
        if (redoStack.Count >0 )
        {
            var move = redoStack.Pop();
            history.Push(move);

            char[,] grid = boards[move.BoardIndex].GetGrid(); 
            grid[move.Row, move.Col] = playerMark;
        }
    }

}
public class Board
{
    private char[,] grid;
    public Board(int size)
    {
        grid = new char[size, size];
    }
    public char[,]  GetGrid()
    {
        return grid;
    }
}



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

public static class HelpSystem
{
    public static void ShowHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine(" - save: Save the current game state");
        Console.WriteLine(" - load: Load a saved game state");
        Console.WriteLine();
    }
}
class Program
{
    static void Main(string[] args)
    {
        GameFactory factory = new GameFactory("Noktakto");
        Game game = factory.GetGame();
        game.SetGame();
        game.Start();
    }
}
