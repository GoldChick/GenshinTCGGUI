using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;

namespace Prefab
{
    public class CharacterEffectGrid : Grid
    {
        private int _availabletimes;
        public Uri? Uri { get; }
        public int Index { get; set; }
        public TextBlock Num { get; }
        public int AvailableTimes
        {
            get => _availabletimes; set
            {
                _availabletimes = value;
                Num.Text = _availabletimes.ToString();
            }
        }
        public CharacterEffectGrid(Uri? uri, int index, int availabletimes)
        {
            Index = index;
            Uri = uri;
            if (uri != null)
            {
                Children.Add(new Image()
                {
                    Source = new BitmapImage(uri),
                });
            }

            Children.Add(new Ellipse()
            {
                Width = 12,
                Height = 12,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Fill = new VisualBrush()
                {
                    Visual = new Ellipse()
                    {
                        Width = 10,
                        Height = 10,
                        Fill = new SolidColorBrush(Colors.Black)
                    }
                }
            });
            Num = new TextBlock()
            {
                FontFamily = new("Arial Black"),
                Foreground = new SolidColorBrush(Colors.White),
                Width = 12,
                Height = 12,
                RenderTransformOrigin = new Point(0.5, 0.5),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, -2, 1),
            };
            AvailableTimes = availabletimes;
            Children.Add(Num);
            //TODO:呈现形式？
        }
    }
}
