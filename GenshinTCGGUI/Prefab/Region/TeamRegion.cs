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
using TCGBase;

namespace Prefab
{
    public class TeamRegion
    {
        public int CurrCharacter { set; get; }
        private readonly Canvas _canvas;
        private readonly HelpRegion _supports;
        private readonly CharacterRegion _characters;
        private readonly HelpRegion _summons;
        public TeamRegion(Canvas canvas, ReadonlyRegion me)
        {
            _canvas = canvas;
            _characters = new(me.Characters)
            {
                Height = 200,
                Width = 576,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Rows = 1,
            };
            CurrCharacter = -1;
            Canvas.SetLeft(_characters, 288);
            Canvas.SetTop(_characters, 60);
            _supports = new(me.Supports)
            {
                Height = 200,
                Width = 200,
                Rows = 2,
                Columns = 2
            };
            Canvas.SetLeft(_supports, 48);
            Canvas.SetTop(_supports, 60);
            _summons = new(me.Summons)
            {
                Height = 200,
                Width = 200,
                Rows = 2,
                Columns = 2
            };
            Canvas.SetLeft(_summons, 904);
            Canvas.SetTop(_summons, 60);

            _canvas.Children.Add(_characters);
            _canvas.Children.Add(_supports);
            _canvas.Children.Add(_summons);
        }
        public void Update(ReadonlyRegion me)
        {
            if (me.CurrCharacter != CurrCharacter)
            {
                if (CurrCharacter >= 0)
                {
                    var c = _characters.Cards[CurrCharacter];
                    c.SecondImage.Effect = null;
                    c.UpdateTeamEffects(null);
                }
                _characters.Cards[me.CurrCharacter].UpdateTeamEffects(me.Effects);
                _characters.Cards[me.CurrCharacter].SecondImage.Effect = new DropShadowEffect()
                {
                    BlurRadius = 30,
                    Color = (Color)ColorConverter.ConvertFromString("#97FF9700"),
                    ShadowDepth = 0
                };

                CurrCharacter = me.CurrCharacter;
            }
            _characters.Update(me.Characters);
            _supports.Update(me.Supports);
            _summons.Update(me.Summons);
        }
    }
}
