using assessment2;
namespace assessment2
{
    public class NoktaktoHelpSystem : IHelpSystem
    {
        public void ShowHelp()
        {
            Console.WriteLine("Noktakto Help:");
            Console.WriteLine(" - Enter your move as 'Board Index,row,col' (e.g., 1,1,2)");
            Console.WriteLine(" - save: Save the current game state");
            Console.WriteLine(" - load: Load a saved game state");
            Console.WriteLine();
        }
    }

}