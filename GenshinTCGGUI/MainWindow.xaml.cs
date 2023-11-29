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
        private static MainWindow _instance;

        internal TeamRegion TeamMe;
        internal TeamRegion TeamEnemy;

        internal int CurrCharacterCashe = -1;
        public static MainWindow Instance { get => _instance; }
        public Game game;
        public IGuiCllient MainClient { get; }
        public MainWindow(bool isServer)
        {
            _instance = this;
            TargetEnumsToSelect = new();
            TargetEnumSelected = new();
            NotValidTargetStored = new();
            RerollCardsSelected = new();
            DiceSelected = new();
            InitializeComponent();

            BlackBlocker_DiceOnly.MouseLeftButtonDown += (s, e) => SelectStateMachine = TrivalSelectStateMachine.None;

            if (isServer)
            {
                game = new();
                game.AddClient(GameManager.Instance.Client0);
                game.AddClient(GameManager.Instance.Client1);

                GameManager.Instance.Client0.BindInitRenderAction(InitRender);
                GameManager.Instance.Client0.BindUpdateRenderAction(UpdateRender);
                MainClient = GameManager.Instance.Client0;
                Task.Run(game.StartGame);
            }
            else
            {
                GameManager.Instance.ClientClient.BindUpdateFullRenderAction(UpdateFullRender);
                MainClient = GameManager.Instance.ClientClient;
            }
        }
        private void InitRender(ReadonlyGame game)
        {
            Dispatcher.Invoke(() =>
            {
                TeamMe = new(Me, game.Me,true);
                TeamEnemy = new(Enemy, game.Enemy,false);
                TeamMe.BlackBlocker.MouseLeftButtonDown += (s, e) => SelectStateMachine = TrivalSelectStateMachine.None;
                TeamEnemy.BlackBlocker.MouseLeftButtonDown += (s, e) => SelectStateMachine = TrivalSelectStateMachine.None;
            });
        }
        private void UpdateRender(ReadonlyGame game)
        {
        }
        private void RenderSkill(ReadonlyRegion me)
        {
            if (me.CurrCharacter >= 0 && CurrCharacterCashe != me.CurrCharacter)
            {
                CurrCharacterCashe = me.CurrCharacter;
                var c = me.Characters[CurrCharacterCashe];

                SkillMe.Children.Clear();
                List<SkillCardGrid> skills = new();
                for (int i = 0; i < c.SkillCount; i++)
                {
                    skills.Add(new SkillCardGrid(c.NameID, i));
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
                    TeamMe = new(Me, game.Me,true);
                    TeamEnemy = new(Enemy, game.Enemy,false);
                    TeamMe.BlackBlocker.MouseLeftButtonDown += (s, e) => SelectStateMachine = TrivalSelectStateMachine.None;
                    TeamEnemy.BlackBlocker.MouseLeftButtonDown += (s, e) => SelectStateMachine = TrivalSelectStateMachine.None;
                }
                var me = game.Me;

                if (me.CurrCharacter >= 0 && CurrCharacterCashe != me.CurrCharacter)
                {
                    CurrCharacterCashe = me.CurrCharacter;
                    var c = me.Characters[CurrCharacterCashe];

                    SkillMe.Children.Clear();
                    List<SkillCardGrid> skills = new();
                    for (int i = 0; i < c.SkillCount; i++)
                    {
                        skills.Add(new SkillCardGrid(c.NameID, i));
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
