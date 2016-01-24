using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2f : IEquatable<Vec2f>
    {
        public static readonly Vec2f Zero = new Vec2f(0, 0);

        public float X;
        public float Y;

        public Vec2f(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vec3f Complement(float z = 0)
        {
            return new Vec3f(this.X, this.Y, z);
        }

        public static Vec2f operator +(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2f operator -(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2f operator *(Vec2f v, float scale)
        {
            return new Vec2f(v.X * scale, v.Y * scale);
        }

        public static Vec2f operator /(Vec2f v, float scale)
        {
            return new Vec2f(v.X / scale, v.Y / scale);
        }

        public static Vec2f operator *(float scale, Vec2f v)
        {
            return new Vec2f(v.X * scale, v.Y * scale);
        }

        public static Vec2f operator /(float scale, Vec2f v)
        {
            return new Vec2f(v.X / scale, v.Y / scale);
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

        public Vec2f Normalize()
        {
            double y = this.Y;
            double x = this.X;
            float num;
            float length = (float)Math.Sqrt((x * x) + (y * y));

            if (length != 0f)
            {
                num = (float)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Vec2f(this.X * num, this.Y * num);
        }

        public static bool operator ==(Vec2f left, Vec2f right)
        {
            return (left.X == right.X) && (left.Y == right.Y);
        }

        public static bool operator !=(Vec2f left, Vec2f right)
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

        public bool Equals(Vec2f other)
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
            return this.Equals((Vec2f)obj);
        }
    }
}
