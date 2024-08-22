using assessment2;
namespace assessment2
{
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
}