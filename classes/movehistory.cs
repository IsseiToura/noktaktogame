using assessment2;
namespace assessment2
{
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
}