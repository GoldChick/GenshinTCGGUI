using GenshinTCGGUI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Prefab
{
    public abstract class GamingSelectableGrid : Grid
    {
        private bool _canbeselected;
        protected Image MainImage;
        public bool CanBeSelected
        {
            get => _canbeselected; set
            {
                _canbeselected = value;
                if (value)
                {
                    MainImage.Opacity = 1;
                    MainImage.Effect = null;
                }
                else
                {
                    MainImage.Opacity = 0.4;
                    MainImage.Effect = new DropShadowEffect()
                    {
                        BlurRadius = 25,
                        Color = (Color)ColorConverter.ConvertFromString("#00000000"),
                        ShadowDepth = 0
                    };
                }
            }
        }
        /// <summary>
        /// 在List中排的位置
        /// </summary>
        public int Index { get; set; }
        protected GamingSelectableGrid(int index)
        {
            Index = index;
            MouseLeftButtonDown += (s, e) => MainWindow.Instance.TrySelect(this);
        }
        /// <summary>
        /// 0 无色 | 1 绿色 | 2 黄色 | 3 红色 | 4 黑色
        /// </summary>
        public void Glow(int type)
        {
            MainImage.Effect = type == 0 ? null : new DropShadowEffect()
            {
                BlurRadius = 25,
                Color = (Color)ColorConverter.ConvertFromString(type switch
                {
                    3 => "#FFFF9700", //红色
                    2 => "#97FFFF00", //黄色
                    _ => "#FF97FF00", //绿色
                }),
                ShadowDepth = 0
            };
        }
    }
}
