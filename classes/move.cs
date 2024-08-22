using assessment2;
namespace assessment2
{
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

}