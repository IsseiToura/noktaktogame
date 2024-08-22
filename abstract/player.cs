using assessment2;
namespace assessment2
{
    public abstract class Player
    {
        public string Name { get; set;}
        public abstract Move MakeMove(Board[] boards);
    }
    
}