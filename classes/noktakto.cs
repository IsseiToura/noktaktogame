using assessment2;
namespace assessment2
{

    public class Noktakto : Game
    {
        public Noktakto()
        {
            const int GRIDSIZE = 3;
            this.boards = new Board[3] { new Board(GRIDSIZE), new Board(GRIDSIZE), new Board(GRIDSIZE) };
            this.moveHistory = new MoveHistory();
            this.currentPlayerIndex = 0;
            this.player1Mark = 'X';
            this.player2Mark = 'X';
            this.helpSystem = new NoktaktoHelpSystem();
        }

        public override void Start()
        {
            bool isLoadGame =UI.IsLoad();
            if(isLoadGame)
            {
                const int GRIDSIZE = 3;
                this.boards = new Board[3] { new Board(GRIDSIZE), new Board(GRIDSIZE), new Board(GRIDSIZE) };
                Load();
            }
            else
            {
                UI.WelcomeMessage();
            }

            while(!IsGameOver())
            {
                UI.DisplayBoard(boards);
                UI.HelpDisplay(helpSystem);
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
                if(CheckThreeInLIne(board))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool CheckThreeInLIne(Board board)
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
}