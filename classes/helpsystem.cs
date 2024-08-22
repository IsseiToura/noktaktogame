using assessment2;
namespace assessment2
{
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

}