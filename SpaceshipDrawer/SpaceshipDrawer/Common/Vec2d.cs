using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2d : IEquatable<Vec2d>
    {
        public static readonly Vec2d Zero = new Vec2d(0, 0);

        public readonly double X;
        public readonly double Y;

        public Vec2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vec3d Complement(double z = 0)
        {
            return new Vec3d(this.X, this.Y, z);
        }

        public static Vec2d operator +(Vec2d a, Vec2d b)
        {
            return new Vec2d(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2d operator -(Vec2d a, Vec2d b)
        {
            return new Vec2d(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2d operator *(Vec2d v, double scale)
        {
            return new Vec2d(v.X * scale, v.Y * scale);
        }

        public static Vec2d operator /(Vec2d v, double scale)
        {
            return new Vec2d(v.X / scale, v.Y / scale);
        }

        public static Vec2d operator *(double scale, Vec2d v)
        {
            return new Vec2d(v.X * scale, v.Y * scale);
        }

        public static Vec2d operator /(double scale, Vec2d v)
        {
            return new Vec2d(v.X / scale, v.Y / scale);
        }

        public double Length()
        {
            double y = this.Y;
            double x = this.X;
            return (double)Math.Sqrt((x * x) + (y * y));
        }

        public double LengthSquared()
        {
            double y = this.Y;
            double x = this.X;
            return (double)((x * x) + (y * y));
        }

        public Vec2d Normalize()
        {
            double y = this.Y;
            double x = this.X;
            double num;
            double length = (double)Math.Sqrt((x * x) + (y * y));

            if (length != 0f)
            {
                num = (double)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Vec2d(this.X * num, this.Y * num);
        }

        public static bool operator ==(Vec2d left, Vec2d right)
        {
            return (left.X == right.X) && (left.Y == right.Y);
        }

        public static bool operator !=(Vec2d left, Vec2d right)
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

        public bool Equals(Vec2d other)
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
            return this.Equals((Vec2d)obj);
        }
    }
}
