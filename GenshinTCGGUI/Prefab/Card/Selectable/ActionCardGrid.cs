using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Web;
using TCGBase;

namespace Prefab
{
    public class ActionCardGrid : SelectableGrid
    {
        public UniformGrid CostContainer { get; }
        public ActionCardGrid(string nameid, int index) : base(index)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/action/{nameid}.png");
            MainImage = new()
            {
                Source = new BitmapImage(File.Exists(path) ? new(path) : new("Resource/Minecraft/Action/unknown.png", UriKind.Relative)),
                Margin = new Thickness(3),
            };
            CostContainer = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 96,
                Rows = 5,
                Columns = 1,
            };

            Children.Add(MainImage);
            Children.Add(new Image() { Source = new BitmapImage(new("Resource/Util/Card/Edge.png", UriKind.Relative)) });
            Children.Add(CostContainer);
        }

        public void UpdateCost(DiceCostVariable dcv) => UpdateCost(dcv.CostSame, dcv.Costs);
        public void UpdateCost(bool sameDice, int[] cost)
        {
            CostContainer.Children.Clear();
            cost.Select((x, element) => (element, x)).Where(p => p.x > 0)
                .Select(p => new ActionCardCost(sameDice, p.element, p.x)).ToList()
                .ForEach(c => CostContainer.Children.Add(c));
        }
        public class ActionCardCost : Grid
        {
            public ActionCardCost(bool same, int element, int num)
            {
                Image cost = new()
                {
                    Source = new BitmapImage(new($"Resource/Util/Cost/{(ElementCategory)element}{((element == 0 && !same) ? "_Void" : "")}.png", UriKind.Relative)),
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
