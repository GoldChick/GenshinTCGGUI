using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCGBase;

namespace Prefab
{
    public class CharacterCardGrid : SelectableGrid
    {
        public string NameID { get; init; }
        /// <summary>
        /// 仅仅用于出战角色的发红光
        /// </summary>
        public Image SecondImage { get; init; }
        public StackPanel ElementPanel { get; init; }
        private StackPanel EffectsPanel { get; init; }
        private StackPanel TeamEffectsPanel { get; init; }
        private StackPanel LeftPanel { get; init; }
        private StackPanel RightPanel { get; init; }
        private TextBlock HPText { get; init; }
        //TODO:unchecked data stored
        private int[] ints = new int[3] { 0, 0, 0 };
        public int HP { get; set; }
        public int MP { get; set; }
        public int Element { get; set; }
        public CharacterCardGrid(string nameid, int maxhp, int maxmp, int index)
        {
            Index = index;
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/character/{nameid}/main.png");
            Uri url = File.Exists(path) ? new(path) : new("null", UriKind.Relative);
            SecondImage = new()
            {
                Source = new BitmapImage(url),
                Margin = new Thickness(3),
            };
            MainImage = new()
            {
                Source = new BitmapImage(url),
                Margin = new Thickness(3),
            };

            ElementPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, -250, 0, 0),
                Height = 32
            };

            LeftPanel = new()
            {
                Margin = new Thickness(-110, -30, 0, 0),
            };
            Grid hp_container = new() { Width = 70 };
            hp_container.Children.Add(new Image()
            {
                Source = new BitmapImage(new("Resource/Util/Card/Hp.png", UriKind.Relative)),
                Width = 60
            });
            HPText = new TextBlock()
            {
                FontFamily = new FontFamily("Arial Black"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20,
                Text = maxhp.ToString()
            };
            hp_container.Children.Add(HPText);

            LeftPanel.Children.Add(hp_container);


            RightPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(120, 10, 0, 0),
                Width = 35,
            };
            for (int i = 0; i < maxmp; i++)
            {
                RightPanel.Children.Add(new Image() { Source = new BitmapImage(new("Resource/Util/Card/Mp.png", UriKind.Relative)) });
            }

            EffectsPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 170, 0, 0),
                Width = 117,
            };
            TeamEffectsPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 200, 0, -30),
                Width = 117,
            };
            NameID = nameid;

            Children.Add(SecondImage);
            Children.Add(MainImage);
            Children.Add(new Image() { Source = new BitmapImage(new("Resource/Util/Card/Edge.png", UriKind.Relative)) });
            Children.Add(LeftPanel);
            Children.Add(RightPanel);
            Children.Add(EffectsPanel);
            Children.Add(ElementPanel);
            Children.Add(TeamEffectsPanel);
        }
        public void Update(ReadonlyCharacter c)
        {
            HP = c.HP;
            HPText.Text = HP.ToString();
            MP = c.MP;
            Element = c.Element;

            ElementPanel.Children.Clear();
            if (c.Element == 5)
            {
                ElementPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new($"Resource/Util/Element/{ElementCategory.Cryo}.png", UriKind.Relative)),
                    Width = 32,
                    Height = 32,
                });
                ElementPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new($"Resource/Util/Element/{ElementCategory.Dendro}.png", UriKind.Relative)),
                    Width = 32,
                    Height = 32,
                });
            }
            else if (c.Element > 0 && c.Element < 7)
            {
                ElementPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new($"Resource/Util/Element/{(ElementCategory)c.Element}.png", UriKind.Relative)),
                    Width = 32,
                    Height = 32,
                });
            }

            EffectsPanel.Children.Clear();
            c.Effects.ForEach(e => EffectsPanel.Children.Add(new CharacterEffectGrid(e.Infos)));

            if (ints[0]==0&& c.Weapon != null)
            {
                LeftPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new("Resource/Util/Icon/Weapon.png", UriKind.Relative)),
                    Width = 40
                });
                ints[0] = 1;
            }
            if (ints[1] == 0 && c.Artifact != null)
            {
                LeftPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new("Resource/Util/Icon/Artifact.png", UriKind.Relative)),
                    Width = 40
                });
                ints[1] = 1;
            }
            if (ints[2] == 0 && c.Talent != null)
            {
                LeftPanel.Children.Add(new Image()
                {
                    Source = new BitmapImage(new("Resource/Util/Icon/Talent.png", UriKind.Relative)),
                    Width = 40
                });
                ints[2] = 1;
            }
        }
        public void UpdateTeamEffects(List<ReadonlyPersistent>? teamEffects)
        {
            TeamEffectsPanel.Children.Clear();
            teamEffects?.ForEach(e => TeamEffectsPanel.Children.Add(new CharacterEffectGrid(e.Infos)));
        }
        public class CharacterEffectGrid : Grid
        {
            public CharacterEffectGrid(int[] infos)
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/effect/atk_self.png");
                Uri url = File.Exists(path) ? new(path) : new("null", UriKind.Relative);
                Children.Add(new Image()
                {
                    Source = new BitmapImage(url),
                });
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
                Children.Add(new TextBlock()
                {
                    FontFamily = new("Arial Black"),
                    Foreground = new SolidColorBrush(Colors.White),
                    Width = 12,
                    Height = 12,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 0, -2, 1),
                    Text = infos[0].ToString()
                });
            }
        }
    }
}
