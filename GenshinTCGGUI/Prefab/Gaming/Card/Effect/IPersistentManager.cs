
using TCGBase;

namespace Prefab
{
    public interface IPersistentManager
    {
        public void Add(ReadonlyPersistent rp);
        public void Trigger(int index, int availabletimes);
        public void RemoveAt(int index);
    }
}
