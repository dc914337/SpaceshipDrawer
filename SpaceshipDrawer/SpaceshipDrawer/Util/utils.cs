using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipDrawer.Util
{
    class utils
    {
        public static void ReleaseCom<T>(ref T x) where T : class, IDisposable
        {
            if (x != null)
            {
                x.Dispose();
                x = null;
            }
        }
    }
}
