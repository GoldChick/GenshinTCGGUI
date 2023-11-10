using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TCGBase;

namespace Prefab
{
    public class CharacterRegion : UniformGrid
    {
        public List<CharacterCardGrid> Cards { get; }
        public CharacterRegion(List<ReadonlyCharacter> chars)
        {
            Cards = chars.Select((c, index) => new CharacterCardGrid(c.Name, c.MaxHP, c.MaxMP, index)).ToList();
            Cards.ForEach(c => Children.Add(c));
        }
        public void Update(List<ReadonlyCharacter> chars)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].Update(chars[i]);
            }
        }
        public void UpdateCurrCharacter(int CurrCharacter)
        {

        }
    }
}
