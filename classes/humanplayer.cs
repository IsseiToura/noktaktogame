using assessment2;
namespace assessment2
{
    public class HumanPlayer : Player
    {
        public override Move MakeMove(Board[] boards)
        {
            return UI.GetMoveFromHumanPlayer(this);
        }
    }

}