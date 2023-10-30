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
    public class ActionCardGrid :  SelectableGrid
    {
        public string NameID { get; init; }
        public bool SameDice { get; init; }
        public int[] Cost { get; init; }
        public ActionCardGrid(string nameid,int index, bool sameDice, int[] cost)
        {
            Index = index;
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/action/{nameid}.png");
            MainImage = new()
            {
                Source = new BitmapImage(File.Exists(path) ? new(path) : new("null", UriKind.Relative)),
                Margin = new Thickness(3),
            };
            UniformGrid cost_container = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 96,
                Rows = 5,
                Columns = 1,
            };
            cost.Select((x, element) => (element, x)).Where(p => p.x > 0)
                .Select(p => new ActionCardCost(sameDice, p.element, p.x)).ToList()
                .ForEach(c => cost_container.Children.Add(c));

            NameID = nameid;
            SameDice = sameDice;
            Cost = cost;

            Children.Add(MainImage);
            Children.Add(new Image() { Source = new BitmapImage(new("Resource/Util/Card/Edge.png", UriKind.Relative)) });
            Children.Add(cost_container);
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
