using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using TCGBase;

namespace Prefab
{
    public class CharacterCardGrid : GamingSelectableGrid, IPersistentManager
    {
        public string NameID { get; init; }
        /// <summary>
        /// 仅仅用于出战角色的发红光
        /// </summary>
        public Image SecondImage { get; init; }
        private StackPanel ElementPanel { get; init; }
        private StackPanel EffectsPanel { get; init; }
        private StackPanel LeftPanel { get; init; }
        private StackPanel RightPanel { get; init; }
        private TextBlock HPText { get; init; }

        private int _hp;
        private int _mp;
        private int _element;
        public int HP
        {
            get => _hp; set
            {
                _hp = int.Max(0, value);
                HPText.Text = _hp.ToString();
            }
        }
        public int MP
        {
            get => _mp;
            set
            {
                _mp = int.Max(0, value);
                for (int i = 0; i < RightPanel.Children.Count; i++)
                {
                    var child = RightPanel.Children[i];
                    if (child is Image img)
                    {
                        img.Source = new BitmapImage(new($"Resource/Util/Card/{(i < _mp ? "MPFull" : "MP")}.png", UriKind.Relative));
                    }
                }
            }
        }
        public int Element
        {
            get => _element; set
            {
                _element = value;
                ElementPanel.Children.Clear();
                if (_element == 5)
                {
                    ElementPanel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new($"Resource/util/element/{ElementCategory.Cryo}.png", UriKind.Relative)),
                        Width = 32,
                        Height = 32,
                    });
                    ElementPanel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new($"Resource/util/element/{ElementCategory.Dendro}.png", UriKind.Relative)),
                        Width = 32,
                        Height = 32,
                    });
                }
                else if (_element > 0 && _element < 7)
                {
                    ElementPanel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new($"Resource/util/element/{(ElementCategory)_element}.png", UriKind.Relative)),
                        Width = 32,
                        Height = 32,
                    });
                }
            }
        }
        public CharacterCardGrid(string nameid, int maxhp, int maxmp, int index) : base(index)
        {
            Effects = new();

            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/character/{nameid}/main.png");
            Uri url = File.Exists(path) ? new(path) : new("Resource/minecraft/action/unknown.png", UriKind.Relative);
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
                Source = new BitmapImage(new("Resource/Util/Card/hp-icon.png", UriKind.Relative)),
                Width = 60
            });
            HPText = new TextBlock()
            {
                FontFamily = new FontFamily("Arial Black"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20,
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
                RightPanel.Children.Add(new Image() { Source = new BitmapImage(new($"Resource/Util/Card/MP.png", UriKind.Relative)) });
            }

            EffectsPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 170, 0, 0),
                Width = 117,
            };

            NameID = nameid;
            HP = maxhp;

            Children.Add(SecondImage);
            Children.Add(MainImage);
            Children.Add(new Image() { Source = new BitmapImage(new("Resource/Util/Card/Edge.png", UriKind.Relative)) });
            Children.Add(LeftPanel);
            Children.Add(RightPanel);
            Children.Add(EffectsPanel);
            Children.Add(ElementPanel);
        }
        public List<CharacterEffectGrid> Effects { get; }
        public void Add(string nameSpace, string nameid, int variant, int availabletimes)
        {
            var p = new Persistent(new(nameSpace, nameid, variant, availabletimes));
            Effects.Add(new CharacterEffectGrid(p.Uri, Effects.Count, availabletimes));
            if (p.Variant >= 0 && p.Uri != null)
            {
                if (EffectsPanel.Children.Count < 4)
                {
                    EffectsPanel.Children.Add(Effects.Last());
                }
                else
                {
                    if (EffectsPanel.Children[3] is CharacterEffectGrid eg && eg.Index == -1)
                    {
                        eg.AvailableTimes++;
                    }
                    else
                    {
                        EffectsPanel.Children.RemoveAt(4);
                        EffectsPanel.Children.Add(new CharacterEffectGrid(new($"Resource/minecraft/icon/more.png", UriKind.Relative), -1, 2));
                    }
                }
            }
            else if (p.Variant < 0)
            {
                Effects.Last().Width = 40;
                LeftPanel.Children.Add(Effects.Last());
            }
        }

        public void Trigger(int index, int availabletimes)
        {
            Effects[index].AvailableTimes = availabletimes;
        }

        public void RemoveAt(int index)
        {
            int i;
            for (i = 0; i < LeftPanel.Children.Count; i++)
            {
                if (LeftPanel.Children[i] is CharacterEffectGrid eg && eg.Index == index)
                {
                    LeftPanel.Children.RemoveAt(i);
                    i = -1;
                    break;
                }
            }
            if (i == -1)
            {
                var chilren = EffectsPanel.Children;
                if (chilren.Count == 4 && chilren[3] is CharacterEffectGrid egm && egm.Index == -1)
                {
                    for (i = 0; i < 3; i++)
                    {
                        if (chilren[i] is CharacterEffectGrid eg && eg.Index == index)
                        {
                            chilren.RemoveAt(i);
                            if (chilren[1] is CharacterEffectGrid last_eg)
                            {
                                for (int j = last_eg.Index + 1; j < Effects.Count; j++)
                                {
                                    if (Effects[j].Uri != null)
                                    {
                                        chilren.Insert(2, Effects[j]);
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                    egm.AvailableTimes--;
                    if (egm.AvailableTimes == 1 && chilren[2] is CharacterEffectGrid last_eg1)
                    {
                        for (int j = last_eg1.Index + 1; j < Effects.Count; j++)
                        {
                            if (Effects[j].Uri != null)
                            {
                                chilren.RemoveAt(3);
                                chilren.Add(Effects[j]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (i = 0; i < chilren.Count; i++)
                    {
                        if (chilren[i] is CharacterEffectGrid eg && eg.Index == index)
                        {
                            chilren.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            Effects.RemoveAt(index);
            for (i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].Index >= 0)
                {
                    Effects[i].Index = i;
                }
            }
        }
    }
}
