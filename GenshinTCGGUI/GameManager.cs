using System.IO;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    internal class GameManager
    {
        private static readonly GameManager _instance = new();
        public static GameManager Instance => _instance;
        public GameManager()
        {
            Registry.Instance.LoadDlls(Directory.GetCurrentDirectory() + "/mods");
        }
        public GuiClient Client0;
        public SocketServerClient Client1;

        public SocketClientClient ClientClient;
    }
}
