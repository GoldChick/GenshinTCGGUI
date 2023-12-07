using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System;
using System.Linq;
using static Prefab.ActionCardGrid;
using TCGBase;
using TCGClient;

namespace Prefab
{
    public class SkillCardGrid : GamingCostableGrid
    {
        public SkillCardGrid(string character_nameid, int index) : base(index)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/character/{character_nameid}/{index}.png");
            MainImage = new()
            {
                Source = new BitmapImage(File.Exists(path) ? new(path) : new("null", UriKind.Relative)),
            };
            CostContainer = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 64, 0, -26),
                Orientation = Orientation.Horizontal
            };

            Children.Add(MainImage);
            Children.Add(CostContainer);
        }
    }
}
