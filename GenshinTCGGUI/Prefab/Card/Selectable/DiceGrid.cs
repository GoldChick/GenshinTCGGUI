using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using TCGBase;
using System.Windows.Media.Imaging;
using System.Windows;
using System;

namespace Prefab
{
    public class DiceGrid : SelectableGrid
    {
        public ElementCategory Element { get; init; }
        public DiceGrid(int element, int index) : base(index)
        {
            Element = (ElementCategory)element;
            MainImage = new Image()
            {
                Width = 64,
                Height = 64,
                Source = new BitmapImage(new($"Resource/Util/Cost/{Element}.png", UriKind.Relative)),
                Margin = new Thickness(0, 5, 0, 0)
            };
            var ele = new Image()
            {
                Width = 32,
                Height = 32,
                Opacity = 0.8,
                Source = new BitmapImage(new($"Resource/Util/Element/{Element}.png", UriKind.Relative)),
                Margin = new Thickness(0, 5, 0, 0)
            };
            Children.Add(MainImage);
            Children.Add(ele);
        }
    }
}
