using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Prefab
{
    public class SideCardGrid : GamingSelectableGrid
    {
        public string? CardName { get; }
        public string? CardText { get; }
        /// <summary>
        /// True为支援区，False为召唤区
        /// </summary>
        public bool IsSupport { get; init; }
        public TextBlock TextRightUp { get; }
        public SideCardGrid(string nameSpace, string nameid, int index, int variant, int availabletimes) : base(index)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/action/{nameid}.png");
            if (!File.Exists(path))
            {
                path = "assets/minecraft/action/unknown.png";
            }
            var b = BitmapFrame.Create(new Uri(path, UriKind.Relative));
            double a = b.Width / 210;
            //NOTE:有些图片是210宽，有些是420，所以有些问题
            MainImage = new()
            {
                Source = new CroppedBitmap(b, new Int32Rect(0, (int)Math.Round(75 * a), (int)Math.Round(210 * a), (int)Math.Round(210 * a))),
                Margin = new Thickness(3)
            };
            Grid timer = new()
            {
                Margin = new Thickness(68, 0, 0, 68),
            };
            Image timeimg = new()
            {
                //TODO:不同的timer
                Source = new BitmapImage(new("Resource/util/icon/counter_simple.png", UriKind.Relative)),
                Margin = new Thickness(0, -10, -10, 0)
            };

            TextRightUp = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Arial Black"),
                Margin = new(14, 0, 0, 0),
                FontSize = 20,
                Text = availabletimes.ToString()
            };
            timer.Children.Add(timeimg);
            timer.Children.Add(TextRightUp);

            Children.Add(MainImage);
            Children.Add(timer);

            path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/pattern/persistent/{nameid}.json");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var converter = JsonSerializer.Deserialize<SideTextureConverter>(json);
                if (converter != null)
                {
                    CardName = converter.Name;
                    CardText = converter.Text;
                    if (!IsSupport)
                    {
                        Grid damage_grid = new()
                        {
                            Margin = new Thickness(0, 50, 50, 0)
                        };
                        Image Element = new()
                        {
                            Source = new BitmapImage(new($"Resource/util/element/{converter.Element[variant / 10]}.png", UriKind.Relative)),
                        };
                        TextBlock damage = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontFamily = new FontFamily("Arial Black"),
                            FontSize = 20,
                            Foreground = new SolidColorBrush(Colors.White),
                            Text = converter.Damage.ToString(),
                            Effect = new DropShadowEffect()
                            {
                                ShadowDepth = 0,
                                BlurRadius = 5,
                                Color = Colors.Black
                            }
                        };
                        damage_grid.Children.Add(Element);
                        damage_grid.Children.Add(damage);
                        Children.Add(damage_grid);
                    }
                }
            }
        }
        public void SetAvailableTimes(int availabletimes)
        {
            TextRightUp.Text = availabletimes.ToString();
        }
    }
}
