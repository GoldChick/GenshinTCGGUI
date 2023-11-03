using Prefab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _ip = "127.0.0.1";
        private int _port = 11451;
        private static MainWindow _instance;
        internal GuiClient Client0;
        internal SocketServerClient Client1;

        internal SocketClientClient ClientClient;
        internal TeamRegion TeamMe;
        internal TeamRegion TeamEnemy;

        internal int CurrCharacterCashe = -1;
        public static MainWindow Instance { get => _instance; }
        public Game game;
        public MainWindow()
        {
            _instance = this;
            TargetEnumSelected = new();
            RerollCardsSelected = new();
            DiceSelected = new();
            InitializeComponent();
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (game != null && game.Stage == GameStage.PreGame && Client1 != null)
            {
                try
                {
                    Task.Run(game.StartGame);
                }
                catch (Exception ex)
                {
                    //console_text0.Text = ex.Message;
                    //console_text1.Text = ex.StackTrace;
                }
            }
        }
        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            var now = DateTime.Now;
            string gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";
            Client0 = new GuiClient(InitRender, UpdateRender, RequestEventCallBack);
            Client1 = new SocketServerClient(_ip, _port);

            Client0.InitServerSetting(null);
            Client1.InitServerSetting(null);
            Client1.StartListen();

            game = new();
            game.AddClient(Client0);
            game.AddClient(Client1);
        }
        private void StartClient_Click(object sender, RoutedEventArgs e)
        {
            if (game == null)
            {
                if (ClientClient == null)
                {
                    ClientClient = new(_ip, _port, UpdateFullRender);
                    ClientClient.StartClient();
                }
            }
        }
        private void InitRender(ReadonlyGame game)
        {
            Dispatcher.Invoke(() =>
            {
                TeamMe = new(Me, game.Me);
                TeamEnemy = new(Enemy, game.Enemy);
            });
        }
        private void UpdateRender(ReadonlyGame game)
        {
            Thread.Sleep(10);
            Dispatcher.Invoke(() =>
            {
                TeamMe.Update(game.Me);
                CardNumMe.Text = game.LeftCardsNum.ToString();
                CardNumEnemy.Text = game.EnemyLeftCardsNum.ToString();
                TeamEnemy.Update(game.Enemy);

                RenderSkill(game.Me);
                assist0.Text = $"当前行动：{((game.CurrTeam == game.MeID) ? "Me" : "Enemy")}";
            });
        }
        private void RenderSkill(ReadonlyRegion me, bool forced = false)
        {
            if (me.CurrCharacter >= 0 && (forced || CurrCharacterCashe != me.CurrCharacter))
            {
                CurrCharacterCashe = me.CurrCharacter;
                var c = me.Characters[CurrCharacterCashe];

                SkillMe.Children.Clear();
                List<SkillCardGrid> skills = new();
                for (int i = 0; i < c.SkillCount; i++)
                {
                    skills.Add(new SkillCardGrid(c.Name, i));
                }

                skills.Reverse();
                skills.ForEach(s => SkillMe.Children.Add(s));
                //只是为了观感上从左往右，并且靠右
            }
        }

        public void UpdateHelpText(string str)
        {
            Dispatcher.Invoke(() => assist0.Text = str);
        }
        private void UpdateFullRender(ReadonlyGame game)
        {
            Thread.Sleep(10);
            Dispatcher.Invoke(() =>
            {

                if (TeamMe == null)
                {
                    TeamMe = new(Me, game.Me);
                    TeamEnemy = new(Enemy, game.Enemy);
                }
                var me = game.Me;
                TeamMe.Update(me);
                TeamEnemy.Update(game.Enemy);

                if (me.CurrCharacter >= 0 && CurrCharacterCashe != me.CurrCharacter)
                {
                    CurrCharacterCashe = me.CurrCharacter;
                    var c = me.Characters[CurrCharacterCashe];

                    SkillMe.Children.Clear();
                    List<SkillCardGrid> skills = new();
                    for (int i = 0; i < c.SkillCount; i++)
                    {
                        skills.Add(new SkillCardGrid(c.Name, i));
                    }

                    skills.Reverse();
                    skills.ForEach(s => SkillMe.Children.Add(s));
                    //只是为了观感上从左往右，并且靠右
                }
            });
        }

        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas.SetTop(CardMe, 560);
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Canvas.SetTop(CardMe, 650);
        }
    }
}
