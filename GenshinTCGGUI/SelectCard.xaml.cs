using Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCGBase;

namespace GenshinTCGGUI
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            var g = GameManager.Instance.Game;
            var characters = Registry.Instance.GetCharacterCards();
            var actions = Registry.Instance.GetActionCards();
            var d = 1;
            foreach (var c in characters)
            {
                CharacterCardToSelectPanel.Children.Add(new PreGamingSelectableGrid(RegistryType.CharacterCard, c));
            }
            CharacterCardScroll.Visibility = Visibility.Visible;

            foreach (var a in actions)
            {
                ActionCardToSelectPanel.Children.Add(new PreGamingSelectableGrid(RegistryType.ActionCard, a));
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
        }

        public void Select(RegistryType type, string nameid)
        {
            
        }
    }
}
