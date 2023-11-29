using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace Prefab
{
    public class SideRegion : UniformGrid, IPersistentManager,IUniformGridRegion
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
        public void Add(string nameSpace, string nameid, int variant, int availabletimes)
        {
            Cards.Add(new(nameSpace, nameid, Cards.Count, variant, availabletimes));
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
