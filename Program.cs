using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

using assessment2;
namespace assessment2;
class Program
{
    static void Main(string[] args)
    {
        GameFactory factory = new GameFactory("Noktakto");
        Game game = factory.GetGame();
        game.SetGame();
        game.Start();
    }
}
