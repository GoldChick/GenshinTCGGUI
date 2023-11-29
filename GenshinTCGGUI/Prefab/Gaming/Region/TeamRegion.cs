using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using TCGBase;

namespace Prefab
{
    public interface IUniformGridRegion
    {
        public IEnumerable<GamingSelectableGrid> Cards { get; }
    }
    public class TeamRegion : IPersistentManager
    {
        private readonly Canvas _canvas;
        private readonly List<CharacterEffectGrid> _effects;
        private readonly StackPanel EffectsPanel;
        private int _currCharacter;

        public int CurrCharacter
        {
            get => _currCharacter; set
            {
                if (value != _currCharacter && value >= 0)
                {
                    if (_currCharacter >= 0)
                    {
                        var c = _characters.Cards[_currCharacter];
                        c.SecondImage.Effect = null;
                        c.Children.Remove(EffectsPanel);
                    }
                    //出战角色的光效
                    _characters.Cards[value].SecondImage.Effect = new DropShadowEffect()
                    {
                        BlurRadius = 30,
                        Color = (Color)ColorConverter.ConvertFromString("#97FF9700"),
                        ShadowDepth = 0
                    };
                    _characters.Cards[value].Children.Add(EffectsPanel);
                }
                _currCharacter = value;
            }
        }
        public readonly Grid BlackBlocker;
        public readonly SideRegion _supports;
        public readonly CharacterRegion _characters;
        public readonly SideRegion _summons;
        public TeamRegion(Canvas canvas, ReadonlyRegion me, bool isme = true)
        {
            EffectsPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 200, 0, -30),
                Width = 117,
            };
            _effects = new();
            _canvas = canvas;
            _characters = new(me.Characters)
            {
                Height = 200,
                Width = 576,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Rows = 1,
            };
            CurrCharacter = me.CurrCharacter;
            Canvas.SetLeft(_characters, 288);
            Canvas.SetTop(_characters, 60);
            _supports = new(true)
            {
                Height = 200,
                Width = 200,
                Rows = 2,
                Columns = 2
            };
            Canvas.SetLeft(_supports, 48);
            Canvas.SetTop(_supports, 60);
            _summons = new(false)
            {
                Height = 200,
                Width = 200,
                Rows = 2,
                Columns = 2
            };
            Canvas.SetLeft(_summons, 904);
            Canvas.SetTop(_summons, 60);

            BlackBlocker = new Grid()
            {
                Background = new SolidColorBrush(Colors.Black),
                Opacity = 0.5,
                Width = 1280,
                Height = 360,
                Visibility = Visibility.Hidden,
            };
            Canvas.SetLeft(BlackBlocker, -64);
            Canvas.SetTop(BlackBlocker, 40 * (isme ? 1 : -1));

            _canvas.Children.Add(_characters);
            _canvas.Children.Add(_supports);
            _canvas.Children.Add(_summons);
            _canvas.Children.Add(BlackBlocker);
        }
        public void Add(string nameSpace, string nameid, int variant, int availabletimes)
        {
            var p = new Persistent(new(nameSpace, nameid, variant, availabletimes));
            _effects.Add(new CharacterEffectGrid(p.Uri, _effects.Count, availabletimes));
            if (p.Variant >= 0 && p.Uri != null)
            {
                if (EffectsPanel.Children.Count < 4)
                {
                    EffectsPanel.Children.Add(_effects.Last());
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
        }

        public void Trigger(int index, int availabletimes)
        {
            _effects[index].AvailableTimes = availabletimes;
        }

        public void RemoveAt(int index)
        {
            var chilren = EffectsPanel.Children;
            if (chilren.Count == 4 && chilren[3] is CharacterEffectGrid egm && egm.Index == -1)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (chilren[i] is CharacterEffectGrid eg && eg.Index == index)
                    {
                        chilren.RemoveAt(i);
                        if (chilren[1] is CharacterEffectGrid last_eg)
                        {
                            for (int j = last_eg.Index + 1; j < _effects.Count; j++)
                            {
                                if (_effects[j].Uri != null)
                                {
                                    chilren.Insert(2, _effects[j]);
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
                    for (int j = last_eg1.Index + 1; j < _effects.Count; j++)
                    {
                        if (_effects[j].Uri != null)
                        {
                            chilren.RemoveAt(3);
                            chilren.Add(_effects[j]);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < chilren.Count; i++)
                {
                    if (chilren[i] is CharacterEffectGrid eg && eg.Index == index)
                    {
                        chilren.RemoveAt(i);
                        break;
                    }
                }
            }

            _effects.RemoveAt(index);
            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].Index >= 0)
                {
                    _effects[i].Index = i;
                }
            }
        }
    }
}
