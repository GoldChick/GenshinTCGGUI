using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TCGBase;

namespace Prefab
{
    public class CharacterRegion : UniformGrid, IUniformGridRegion
    {
        public List<CharacterCardGrid> Cards { get; }
        public int CurrCharacter { set; get; }

        IEnumerable<GamingSelectableGrid> IUniformGridRegion.Cards => Cards;

        public CharacterRegion(List<ReadonlyCharacter> chars)
        {
            CurrCharacter = -1;
            Cards = chars.Select((c, index) => new CharacterCardGrid(c.NameID, c.MaxHP, c.MaxMP, c.SkillCount, index)).ToList();
            Cards.ForEach(c => Children.Add(c));
        }
    }
}
