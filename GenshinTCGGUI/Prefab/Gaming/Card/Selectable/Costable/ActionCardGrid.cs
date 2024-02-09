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
    public class ActionCardGrid : GamingCostableGrid
    {
        public string NameSpace { get; }
        public string NameID { get; }
        public ActionCardGrid(string nameSpace, string nameid, int index) : base(index)
        {
            NameSpace = nameSpace;
            NameID = nameid;

            var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/action/{nameid}.png");
            MainImage = new()
            {
                Source = new BitmapImage(File.Exists(path) ? new(path) : new("Resource/minecraft/action/unknown.png", UriKind.Relative)),
                Margin = new Thickness(3),
            };
            CostContainer = new UniformGrid()
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
    }
}
