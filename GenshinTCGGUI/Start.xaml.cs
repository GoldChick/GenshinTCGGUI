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
    /// Start.xaml 的交互逻辑
    /// </summary>
    public partial class Start : Window
    {
        private string _ip = "127.0.0.1";
        private int _port = 11451;
        public Start()
        {
            InitializeComponent();
            b0.Visibility = Visibility.Hidden;
            try
            {
                var setjson = File.ReadAllText(Directory.GetCurrentDirectory() + "/cardsets/0.json");
                var set = JsonSerializer.Deserialize<CardSetSetting>(setjson);
                CardMe.Children.Clear();
                var cs = Registry.Instance.GetCharacterCards();
                foreach (var c in set.CardSet.Characters)
                {
                    var strs = c.Split(':');
                    CardMe.Children.Add(new PreGamingSelectableGrid(RegistryType.CharacterCard, strs[0], strs[1], -1, -2));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            if (GameManager.Instance.Client1 != null)
            {
                var now = DateTime.Now;
                string gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";

                MainWindow main = new(true);
                App.Current.MainWindow = main;
                main.Show();
                Close();
            }
        }

        private void StartClient(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.ClientClient = new(_ip, _port);
            GameManager.Instance.ClientClient.BindHelpTextAction((str) => Dispatcher.Invoke(() =>
            {
                HelpText.Text = str;
                if (str == "客户端连接成功")
                {
                    b1.Visibility = Visibility.Hidden;
                    b2.Visibility = Visibility.Hidden;
                    b3.Visibility = Visibility.Hidden;
                }
            }));
            GameManager.Instance.ClientClient.StartClient();
            GameManager.Instance.ClientClient.BindHelpTextAction(null);
            MainWindow main = new(false);
            App.Current.MainWindow = main;
            main.Show();
            Close();
        }
        private void StartServer(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.Client0 = new GuiClient();
            GameManager.Instance.Client1 = new SocketServerClient(_ip, _port);

            GameManager.Instance.Client1.BindHelpTextAction((str) => Dispatcher.Invoke(() =>
            {
                HelpText.Text = str;
                if (str == "服务端开启成功")
                {
                    b0.Visibility = Visibility.Visible;
                    b1.Visibility = Visibility.Hidden;
                    b2.Visibility = Visibility.Hidden;
                    b3.Visibility = Visibility.Hidden;
                }
            }));
            GameManager.Instance.Client1.StartListen();
            GameManager.Instance.Client1.BindHelpTextAction(null);
        }

        private void UpdateCardSet(object sender, RoutedEventArgs e)
        {
            SelectCard select = new();
            App.Current.MainWindow = select;
            select.Show();
            Close();
        }
    }
}
