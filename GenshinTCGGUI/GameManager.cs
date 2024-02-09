using System.IO;
using System.Linq;
using System.Text.Json;
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
            Registry.Instance.RFDll.LoadDlls(Directory.GetCurrentDirectory() + "/mods");
            Registry.Instance.RFJson.LoadFolders(Directory.GetCurrentDirectory() + "/mods", "genshin3_3");
        }
        public GuiClient Client0;
        public SocketServerClient Client1;

        public SocketClientClient ClientClient;
    }
}
