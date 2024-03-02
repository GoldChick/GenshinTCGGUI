using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using TCGBase;

namespace Prefab
{
    public class SideRegion : UniformGrid, IPersistentManager, IUniformGridRegion
    {
        public bool IsSupport { get; }
        public List<SideCardGrid> Cards { get; private set; }

        IEnumerable<GamingSelectableGrid> IUniformGridRegion.Cards => Cards;

        public SideRegion(bool issupport)
        {
            IsSupport = issupport;
            Cards = new();
        }
        //public void Update()
        //{
        //    Children.Clear();
        //    Cards = Persistents.Select((p, index) => new SideCardGrid(p.NameSpace, p.NameID, index, p.AvailableTimes)).ToList();
        //    Cards.ForEach(c => Children.Add(c));
        //}
        public void Add(ReadonlyPersistent rp)
        {
            Cards.Add(new(rp.NameSpace, rp.NameID, Cards.Count, rp.AvailableTimes, rp.Data));
            Children.Add(Cards.Last());
        }
        public void Trigger(int index, int availabletimes)
        {
            Cards[index].SetAvailableTimes(availabletimes);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
            Cards.RemoveAt(index);
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].Index = i;
            }
        }
    }
}
