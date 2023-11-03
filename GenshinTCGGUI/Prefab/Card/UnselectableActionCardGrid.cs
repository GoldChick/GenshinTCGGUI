using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Prefab
{
    public class UnselectableActionCardGrid : Grid
    {
        /// <summary>
        /// 空白卡
        /// </summary>
        public UnselectableActionCardGrid()
        {
            Image MainImage = new()
            {
                Source = new BitmapImage(new("Resource/Minecraft/Action/amethyst.png", UriKind.Relative)),
                Margin = new Thickness(3),
            };
            Children.Add(MainImage);
            Children.Add(new Image() { Source = new BitmapImage(new("Resource/Util/Card/Edge.png", UriKind.Relative)) });
        }
    }
}
