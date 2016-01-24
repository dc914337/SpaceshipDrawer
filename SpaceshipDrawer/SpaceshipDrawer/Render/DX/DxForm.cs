using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceshipDrawer.Render.DX
{
    public delegate bool MyWndProc(ref Message m);
    class DxForm : Form
    {
        public MyWndProc MyWndProc;
        protected override void WndProc(ref Message m)
        {
            if (MyWndProc != null)
            {
                if (MyWndProc(ref m)) return;
            }
            base.WndProc(ref m);
        }




    }
}
