using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using TCGBase;

namespace Prefab
{
    public class HelpRegion : UniformGrid
    {
        public List<HelpCardGrid> Cards { get; private set; }
        public HelpRegion(List<ReadonlyPersistent> ps)
        {
            Cards = ps.Select((c, index) => new HelpCardGrid(c.Name, index, c.Infos)).ToList();
            Cards.ForEach(c => Children.Add(c));
        }
        public void Update(List<ReadonlyPersistent> ps)
        {
            Children.Clear();
            Cards = ps.Select((c, index) => new HelpCardGrid(c.Name, index, c.Infos)).ToList();
            Cards.ForEach(c => Children.Add(c));
        }
    }
}
