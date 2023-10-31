using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using TCGBase;

namespace Prefab
{
    public class HelpCardGrid : SelectableGrid
    {
        /// <summary>
        /// True为支援区，False为召唤区
        /// </summary>
        public bool IsSupport { get; init; }
        public HelpCardGrid(string nameid, int index, params int[] infos)
        {
            Index = index;
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/action/{nameid}.png");
            if (!File.Exists(path))
            {
                path = "null";
            }
            var b = BitmapFrame.Create(new Uri(path, UriKind.Relative));
            int a = (int)Math.Round(b.Width / 210);
            //NOTE:有些图片是210宽，有些是420，所以有些问题
            MainImage = new()
            {
                Source = new CroppedBitmap(b, new Int32Rect(0, 75*a, 210 * a, 210 * a)),
                Margin = new Thickness(3)
            };
            Grid timer = new()
            {
                Margin = new Thickness(68, 0, 0, 68),
            };
            Image timeimg = new()
            {
                //TODO:不同的timer
                Source = new BitmapImage(new("Resource/Util/Cost/Trival.png", UriKind.Relative)),
                Margin = new Thickness(0, -10, -10, 0)
            };
            TextBlock num = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new System.Windows.Media.FontFamily("Arial Black"),
                Margin = new(14, 0, 0, 0),
                FontSize = 20,
                Text = infos[0].ToString()
            };
            timer.Children.Add(timeimg);
            timer.Children.Add(num);

            Children.Add(MainImage);
            Children.Add(timer);
        }
    }
}
