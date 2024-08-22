using assessment2;
namespace assessment2
{
    public class ComputerPlayer : Player
    {
        public override Move MakeMove(Board[] boards)
        {
            return Move.GetRandomValidMove(boards);
        }   
    }

}