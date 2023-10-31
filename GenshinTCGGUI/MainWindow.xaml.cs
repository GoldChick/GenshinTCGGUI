using Prefab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private static MainWindow _instance;
        internal GuiClient Client;
        internal TeamRegion TeamMe;
        internal TeamRegion TeamEnemy;

        public static MainWindow Instance { get => _instance; }
        public Game game = new();
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

            var now = DateTime.Now;
            string gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";

            Client = new GuiClient(InitRender, UpdateRender, UpdatePacketRender, RequestEventCallBack);
            var c1 = new SleepClient();

            Client.InitServerSetting(null);

            game.AddClient(Client);
            game.AddClient(c1);

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
        private void InitRender(ReadonlyGame game)
        {
            Dispatcher.Invoke(() =>
            {
                TeamMe = new(Me, game.Me);
                TeamEnemy = new(Enemy, game.Enemy);
            });
        }
        private void UpdatePacketRender(ClientUpdatePacket packet)
        {
            Dispatcher.Invoke(() =>
            {
                int teamid = packet.Category / 10;
                int category = packet.Category % 10;
                switch (packet.Type)
                {
                    case ClientUpdateType.None:
                        break;
                    case ClientUpdateType.CurrTeam:
                        break;
                    case ClientUpdateType.WaitingTime:
                        break;
                    case ClientUpdateType.Character:
                        break;
                    case ClientUpdateType.Persistent:
                        break;
                    case ClientUpdateType.Dice:
                        {
                            if (teamid == Client.MeID)
                            {
                                DiceContainer.Children.Clear();
                                for (int i = 0; i < packet.Ints.Length; i++)
                                {
                                    DiceContainer.Children.Add(new DiceGrid(packet.Ints[i], i));
                                }
                                DiceCount.Text = DiceContainer.Children.Count.ToString();
                            }
                            else
                            {
                                //对方的
                            }
                        }
                        break;
                    case ClientUpdateType.Card:
                        {
                            switch (category)
                            {
                                case 0://Use
                                case 1://Blend
                                    if (teamid == Client.MeID)
                                    {
                                        CardMe.Children.RemoveAt(packet.Ints[0]);
                                    }
                                    break;
                                case 2://Obtain
                                    if (teamid == Client.MeID)
                                    {
                                        var req = Client.GetEventFinalDiceRequirement(new(ActionType.UseCard, CardMe.Children.Count));
                                        CardMe.Children.Add(new ActionCardGrid(packet.Strings[0], CardMe.Children.Count, req.Cost.CostSame, req.Cost.Costs));
                                    }
                                    break;
                                case 3://Push
                                    if (teamid == Client.MeID)
                                    {
                                        int cnt = packet.Ints.Length;
                                        foreach (var i in packet.Ints.Reverse())
                                        {
                                            CardMe.Children.RemoveAt(i);
                                        }
                                    }
                                    break;
                                case 4://Pop
                                    if (teamid == Client.MeID)
                                    {

                                    }
                                    break;
                                case 5://Broke
                                    break;
                            }
                            int cardcount = CardMe.Children.Count;
                            CardMe.Columns = 10 + cardcount % 2;
                            CardMe.FirstColumn = (CardMe.Columns - cardcount) / 2;
                            for (int i = 0; i < cardcount; i++)
                            {
                                var acg = CardMe.Children[i] as ActionCardGrid;
                                acg.Index = i;
                            }
                        }
                        break;
                    default:
                        break;
                }
            });
        }
        private void UpdateRender(ReadonlyGame game)
        {
            Thread.Sleep(10);
            Dispatcher.Invoke(() =>
            {
                TeamMe.Update(game.Me);
                TeamEnemy.Update(game.Enemy);

                RenderSkill(game.Me);
                assist0.Text = $"当前行动：{((game.CurrTeam == game.MeID) ? "Me" : "Enemy")}";
            });
        }
        private void RenderSkill(ReadonlyRegion me)
        {
            if (me.CurrCharacter >= 0)
            {
                var c = me.Characters[me.CurrCharacter];
                SkillMe.Children.Clear();
                List<SkillCardGrid> skills = new();
                for (int i = 0; i < c.SkillCount; i++)
                {
                    var req = Client.GetEventFinalDiceRequirement(new(ActionType.UseSKill, i));
                    skills.Add(new SkillCardGrid(c.Name, i, req.Cost.CostSame, req.Cost.Costs));
                }
                skills.Reverse();
                skills.ForEach(s => SkillMe.Children.Add(s));
            }
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
