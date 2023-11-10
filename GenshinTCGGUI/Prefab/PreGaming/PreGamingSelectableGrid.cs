using GenshinTCGGUI;
using System.IO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using TCGBase;
namespace Prefab
{
    public  class PreGamingSelectableGrid : Grid
    {
        protected Image MainImage;
        public string NameID { get; }
        public PreGamingSelectableGrid(RegistryType type, string nameid)
        {
            NameID = nameid;

            var strs = nameid.Split(':');
            Uri url = new("Resource/minecraft/action/unknown.png", UriKind.Relative);
            if (strs.Length == 2)
            {
                string path="assets/path.png";
                switch (type)
                {
                    case RegistryType.CharacterCard:
                        path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{strs[0]}/character/{strs[1]}/main.png");
                        break;
                    case RegistryType.ActionCard:
                        path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{strs[0]}/action/{strs[1]}.png");
                        break;
                }
                if (File.Exists(path))
                {
                    url = new(path);
                }
            }
            MainImage = new()
            {
                Source = new BitmapImage(url),
                Height=160,
                Width=84
            };
            Children.Add(MainImage);

            MouseLeftButtonDown += (s, e) => MainWindow.Instance.TrySelect(this);
        }
        /// <summary>
        /// 0 无色 | 1 绿色 | 2 黄色
        /// </summary>
        public void Glow(int type)
        {
            MainImage.Effect = type == 0 ? null : new DropShadowEffect()
            {
                BlurRadius = 25,
                Color = (Color)ColorConverter.ConvertFromString(type switch
                {
                    2 => "#97FFFF00", //黄色
                    3 => "#FFFF9700", //绿色
                    _ => "#FF97FF00", //绿色
                }),
                ShadowDepth = 0
            };
        }
    }
}
