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

            Client = new GuiClient(InitRender, UpdateRender, RequestEventCallBack);
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

        }
        private void UpdateRender(ReadonlyGame game)
        {
            Thread.Sleep(10);
            Dispatcher.Invoke(() =>
            {
                TeamMe.Update(game.Me);
                TeamEnemy.Update(game.Enemy);

                RenderDice(game.Dices);
                RenderSkill(game.Me);
                assist0.Text = $"当前行动：{((game.CurrTeam == game.MeID) ? "Me" : "Enemy")}";
                CardMe.Children.Clear();
                int cardcount = game.Cards.Count;
                CardMe.Columns = 10 + cardcount % 2;
                CardMe.FirstColumn = (CardMe.Columns - cardcount) / 2;
                for (int i = 0; i < cardcount; i++)
                {
                    var req = Client.GetEventFinalDiceRequirement(new(ActionType.UseCard, i));
                    CardMe.Children.Add(new ActionCardGrid(game.Cards[i], i, req.Cost.CostSame, req.Cost.Costs));
                }
            });
        }
        private void RenderDice(List<int> dices)
        {
            DiceContainer.Children.Clear();
            for (int i = 0; i < dices.Count; i++)
            {
                DiceContainer.Children.Add(new DiceGrid(dices[i], i));
            }
            DiceCount.Text = DiceContainer.Children.Count.ToString();
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
