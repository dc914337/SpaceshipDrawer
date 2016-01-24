using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3i
    {
        public int X;
        public int Y;
        public int Z;

        public Vec3i(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static explicit operator Vec3i(Vec3f v)
        {
            return new Vec3i((int)v.X, (int)v.Y, (int)v.Z);
        }


        public static Vec3i operator +(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3i operator -(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3i operator *(Vec3i v, int scale)
        {
            return new Vec3i(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Vec3i operator /(Vec3i v, int scale)
        {
            return new Vec3i(v.X / scale, v.Y / scale, v.Z / scale);
        }

        public static Vec3i operator %(Vec3i v, int scale)
        {
            return new Vec3i(v.X % scale, v.Y % scale, v.Z % scale);
        }

    }
}
