
using System.Collections.Generic;
using System.Windows.Documents;
using TCGBase;

namespace Prefab
{
    public interface IPersistentManager
    {
        public void Add(string nameSpace, string nameid, int variant, int availabletimes);
        public void Trigger(int index, int availabletimes);
        public void RemoveAt(int index);
    }
}
