using Prefab;
using System.Linq;
using System.Windows.Controls;
using static Prefab.ActionCardGrid;
using TCGBase;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System;

namespace Prefab
{
    public class GamingCostableGrid : GamingSelectableGrid
    {
        protected Panel CostContainer { get; init; }
        protected GamingCostableGrid(int index) : base(index)
        {
            //panel init in sub
        }
        public void UpdateCost(CostVariable dcv) => UpdateCost(dcv.DiceCost);
        protected void UpdateCost(int[] cost)
        {
            CostContainer.Children.Clear();
            var list = cost.Select((x, element) => (element, x)).Where(p => p.x > 0)
                .Select(p => new CostGrid(p.element, p.x)).ToList();
            list.Reverse();
            list.ForEach(c => CostContainer.Children.Add(c));
        }
        public class CostGrid : Grid
        {
            public CostGrid(int element, int num)
            {
                Image cost = new()
                {
                    Source = new BitmapImage(new($"Resource/Util/Cost/{element switch
                    {
                        8 => "void",
                        9 => "mp",
                        _ => ((ElementCategory)element).ToString()
                    }}.png", UriKind.Relative)),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(-10, -9, 0, 0),
                    Height = 37,
                    Width = 32
                };
                TextBlock numtext = new()
                {
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 20,
                    Text = num.ToString(),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextAlignment = TextAlignment.Center,
                    Height = 27,
                    Width = 22,
                    Margin = new Thickness(-6, -4, 0, 0)
                };
                Children.Add(cost);
                Children.Add(numtext);
            }
        }

    }
}
