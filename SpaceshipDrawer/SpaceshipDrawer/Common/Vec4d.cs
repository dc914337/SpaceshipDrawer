using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec4d : IEquatable<Vec4d>
    {
        public static readonly Vec4d Zero = new Vec4d(0, 0, 0, 0);

        public double X;
        public double Y;
        public double Z;
        public double W;

        public Vec4d(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Vec4d operator +(Vec4d a, Vec4d b)
        {
            return new Vec4d(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }

        public static Vec4d operator -(Vec4d a, Vec4d b)
        {
            return new Vec4d(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        public static Vec4d operator *(Vec4d v, double scale)
        {
            return new Vec4d(v.X * scale, v.Y * scale, v.Z * scale, v.W * scale);
        }

        public static Vec4d operator /(Vec4d v, double scale)
        {
            return new Vec4d(v.X / scale, v.Y / scale, v.Z / scale, v.W / scale);
        }

        public static Vec4d operator *(double scale, Vec4d v)
        {
            return new Vec4d(v.X * scale, v.Y * scale, v.Z * scale, v.W * scale);
        }

        public static Vec4d operator /(double scale, Vec4d v)
        {
            return new Vec4d(v.X / scale, v.Y / scale, v.Z / scale, v.W / scale);
        }

        public double Length()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            return (double)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }

        public double LengthSquared()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            return (double)((x * x) + (y * y) + (z * z) + (w * w));
        }

        public Vec4d Normalize()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            double num;
            double length = (double)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));

            if (length != 0f)
            {
                num = (double)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Vec4d(this.X * num, this.Y * num, this.Z * num, this.W * num);
        }

        public static bool operator ==(Vec4d left, Vec4d right)
        {
            return (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z) && (left.W == right.W);
        }

        public static bool operator !=(Vec4d left, Vec4d right)
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

        public bool Equals(Vec4d other)
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
            return this.Equals((Vec4d)obj);
        }

    }
}
