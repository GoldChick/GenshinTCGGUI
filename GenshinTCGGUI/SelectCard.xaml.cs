using Prefab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCard : Window
    {
        public static SelectCard Instance;
        protected List<(int, string)> Chars = new();
        protected List<(int, string)> Actions = new();

        protected List<AbstractCardCharacter> CardCharacters = new();
        protected List<AbstractCardAction> CardActions = new();
        public SelectCard()
        {
            InitializeComponent();
            Instance = this;
            var g = GameManager.Instance;

            CardCharacters = Registry.Instance.GetCharacterCards();
            CardActions = Registry.Instance.GetActionCards();
            for (int i = 0; i < CardCharacters.Count; i++)
            {
                CharacterCardToSelectPanel.Children.Add(new PreGamingSelectableGrid(CardCharacters[i], i));
            }
            CharacterCardScroll.Visibility = Visibility.Visible;

            int cnt = 0;
            for (int i = 0; i < CardActions.Count; i++)
            {
                var a = CardActions[i];
                for (int j = 0; j < a.MaxNumPermitted; j++)
                {
                    ActionCardToSelectPanel.Children.Add(new PreGamingSelectableGrid(a, cnt));
                    cnt++;
                }
            }
            ActionCardScroll.Visibility = Visibility.Hidden;
        }

        private void ChooseCharacter(object sender, RoutedEventArgs e)
        {
            CharacterCardScroll.Visibility = Visibility.Visible;
            ActionCardScroll.Visibility = Visibility.Hidden;
        }
        private void ChooseAction(object sender, RoutedEventArgs e)
        {
            CharacterCardScroll.Visibility = Visibility.Hidden;
            ActionCardScroll.Visibility = Visibility.Visible;
            var cs = Chars.Select(p => CardCharacters[p.Item1]).ToList();
            foreach (var acg in ActionCardToSelectPanel.Children)
            {
                if (acg is PreGamingSelectableGrid g)
                {
                    if (g.Card is AbstractCardAction acc)
                    {
                        if (acc.CanBeArmed(cs))
                        {
                            g.MainImage.Opacity = 1;
                        }
                        else
                        {
                            g.MainImage.Opacity = 0.25;
                        }
                    }
                }
            }
        }

        public void Select(RegistryType type, string nameSpace, string nameid, int index)
        {
            string fullid = $"{nameSpace}:{nameid}";

            List<(int, string)> list;
            UniformGrid uniform;
            StackPanel toselect;
            int limit;
            switch (type)
            {
                case RegistryType.CharacterCard:
                    list = Chars;
                    uniform = CharacterCardsSelected;
                    toselect = CharacterCardToSelectPanel;
                    limit = 3;
                    break;
                case RegistryType.ActionCard:
                    list = Actions;
                    uniform = ActionCardsSelected;
                    toselect = ActionCardToSelectPanel;
                    limit = 30;
                    break;
                default:
                    throw new Exception("UnknownRegistryType");
            }
            if (toselect.Children[index] is PreGamingSelectableGrid g)
            {
                if (list.Contains((index, fullid)))
                {
                    list.Remove((index, fullid));
                    g.MainImage.Opacity = 1;
                }
                else if (list.Count < limit && double.Abs(g.MainImage.Opacity - 1) < 0.01)
                {
                    list.Add((index, fullid));
                    g.MainImage.Opacity = 0.5;
                }
            }
            uniform.Children.Clear();
            list.ForEach(tuple =>
            {
                var strs = tuple.Item2.Split(':');
                uniform.Children.Add(new PreGamingSelectableGrid(type, strs[0], strs[1], tuple.Item1, 0));
            });
        }

        private void ChooseSave(object sender, RoutedEventArgs e)
        {
            if (Chars.Count == 3 && Actions.Count == 30)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/cardsets/");
                CardSetSetting s = new()
                {
                    CardSetName = "test",
                    CardSet = new()
                    {
                        Characters = Chars.Select(p => p.Item2).ToArray(),
                        ActionCards = Actions.Select(p => p.Item2).ToArray(),
                    }
                };
                File.WriteAllText(Directory.GetCurrentDirectory() + "/cardsets/0.json", JsonSerializer.Serialize(s));

                Start start = new();
                App.Current.MainWindow = start;
                start.Show();
                this.Close();
            }
        }
    }
}
