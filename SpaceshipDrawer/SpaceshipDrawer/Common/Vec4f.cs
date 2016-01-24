using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec4f : IEquatable<Vec4f>
    {
        public static readonly Vec4f Zero = new Vec4f(0, 0, 0, 0);

        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vec4f(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Vec4f operator +(Vec4f a, Vec4f b)
        {
            return new Vec4f(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }

        public static Vec4f operator -(Vec4f a, Vec4f b)
        {
            return new Vec4f(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        public static Vec4f operator *(Vec4f v, float scale)
        {
            return new Vec4f(v.X * scale, v.Y * scale, v.Z * scale, v.W * scale);
        }

        public static Vec4f operator /(Vec4f v, float scale)
        {
            return new Vec4f(v.X / scale, v.Y / scale, v.Z / scale, v.W / scale);
        }

        public static Vec4f operator *(float scale, Vec4f v)
        {
            return new Vec4f(v.X * scale, v.Y * scale, v.Z * scale, v.W * scale);
        }

        public static Vec4f operator /(float scale, Vec4f v)
        {
            return new Vec4f(v.X / scale, v.Y / scale, v.Z / scale, v.W / scale);
        }

        public float Length()
        {
            float y = this.Y;
            float x = this.X;
            float z = this.Z;
            float w = this.W;
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }

        public float LengthSquared()
        {
            float y = this.Y;
            float x = this.X;
            float z = this.Z;
            float w = this.W;
            return (float)((x * x) + (y * y) + (z * z) + (w * w));
        }

        public Vec4f Normalize()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            float num;
            float length = (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));

            if (length != 0f)
            {
                num = (float)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Vec4f(this.X * num, this.Y * num, this.Z * num, this.W * num);
        }

        public static bool operator ==(Vec4f left, Vec4f right)
        {
            return (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z) && (left.W == right.W);
        }

        public static bool operator !=(Vec4f left, Vec4f right)
        {
            return (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z) || (left.W != right.W);
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2} W:{3}", this.X, this.Y, this.Z, this.W);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
        }

        public bool Equals(Vec4f other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z) && (this.W == other.W);
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
            return this.Equals((Vec4f)obj);
        }

    }
}
