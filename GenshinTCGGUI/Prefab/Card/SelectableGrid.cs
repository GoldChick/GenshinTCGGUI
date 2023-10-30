using GenshinTCGGUI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Prefab
{
    public abstract class SelectableGrid : Grid
    {
        protected Image MainImage;
        /// <summary>
        /// 在List中排的位置
        /// </summary>
        public int Index { get; set; }
        protected SelectableGrid()
        {
            MouseLeftButtonDown += (s, e) => MainWindow.Instance.TrySelect(this);
        }
        /// <summary>
        /// 0 无色 | 1 绿色 | 2 黄色
        /// </summary>
        public void Glow(int type)
        {
            MainImage.Effect = type == 0 ? null : new DropShadowEffect()
            {
                BlurRadius = 25,
                Color = (Color)ColorConverter.ConvertFromString(type switch
                {
                    2 => "#97FFFF00", //黄色
                    3 => "#FFFF9700", //绿色
                    _ => "#FF97FF00", //绿色
                }),
                ShadowDepth = 0
            };
        }
    }
}
