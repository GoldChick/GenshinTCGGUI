using TCGBase;

namespace GenshinTCGGUI
{
    internal class GameManager
    {
        private static readonly GameManager _instance = new();
        public static GameManager Instance => _instance;
        public Game Game;
        public GameManager()
        {
            Game= new Game();
        }
    }
}
