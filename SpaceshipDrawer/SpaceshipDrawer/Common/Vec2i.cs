using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2i : IEquatable<Vec2i>
    {
        public static readonly Vec2i Zero = new Vec2i(0, 0);

        public int X;
        public int Y;

        public Vec2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator Vec2f(Vec2i v)
        {
            return new Vec2f(v.X, v.Y);
        }

        public static explicit operator Vec2i(Vec2f v)
        {
            return new Vec2i((int)v.X, (int)v.Y);
        }

        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2i operator *(Vec2i v, int scale)
        {
            return new Vec2i(v.X * scale, v.Y * scale);
        }

        public static Vec2i operator /(Vec2i v, int scale)
        {
            return new Vec2i(v.X / scale, v.Y / scale);
        }

        public static Vec2i operator *(int scale, Vec2i v)
        {
            return new Vec2i(v.X * scale, v.Y * scale);
        }

        public static Vec2i operator /(int scale, Vec2i v)
        {
            return new Vec2i(v.X / scale, v.Y / scale);
        }

        public float Length()
        {
            float y = this.Y;
            float x = this.X;
            return (float)Math.Sqrt((x * x) + (y * y));
        }

        public float LengthSquared()
        {
            float y = this.Y;
            float x = this.X;
            return (float)((x * x) + (y * y));
        }

        public static bool operator ==(Vec2i left, Vec2i right)
        {
            return (left.X == right.X) && (left.Y == right.Y);
        }

        public static bool operator !=(Vec2i left, Vec2i right)
        {
            return (left.X != right.X) || (left.Y != right.Y);
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1}", this.X, this.Y);
        }

        public override int GetHashCode()
        {
            return this.Y.GetHashCode() ^ this.X.GetHashCode();
        }

        public bool Equals(Vec2i other)
        {
            return (this.X == other.X) && (this.Y == other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            return this.Equals((Vec2i)obj);
        }
    }
}
