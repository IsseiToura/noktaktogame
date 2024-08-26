using assessment2;
namespace assessment2
{
    public class HumanPlayer : Player
    {
        public override Move MakeMove(Board[] boards)
        {
            Console.WriteLine($"{Name}, it's your turn");
            return UI.GetMoveFromHumanPlayer();
        }
    }

}