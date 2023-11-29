using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinTCGGUI
{
    public interface IGuiCllient
    {
        public void GetCardNextValidTargets(int cardindex, int[] already_params, Action<List<int>>? callback);
    }
}
