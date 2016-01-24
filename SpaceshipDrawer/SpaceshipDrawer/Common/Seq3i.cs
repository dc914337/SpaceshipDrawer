using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Seq3i
    {
        public int A;
        public int B;
        public int C;

        public Seq3i(int a, int b, int c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }
    }
}
