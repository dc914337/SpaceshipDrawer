﻿using System;
using System.Runtime.InteropServices;

namespace SpaceshipDrawer.Common
{
    using Math = System.Math;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3d : IEquatable<Vec3d>
    {
        public static readonly Vec3d Zero = new Vec3d(0, 0, 0);
        public static readonly Vec3d I = new Vec3d(1, 0, 0);

        public double X;
        public double Y;
        public double Z;

        public Vec3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vec4d Complement(double w = 0)
        {
            return new Vec4d(this.X, this.Y, this.Z, w);
        }
        
        public static Vec3d operator +(Vec3d a, Vec3d b)
        {
            return new Vec3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3d operator -(Vec3d a, Vec3d b)
        {
            return new Vec3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3d operator *(Vec3d v, double scale)
        {
            return new Vec3d(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Vec3d operator /(Vec3d v, double scale)
        {
            return new Vec3d(v.X / scale, v.Y / scale, v.Z / scale);
        }

        public static Vec3d operator *(double scale, Vec3d v)
        {
            return new Vec3d(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Vec3d operator /(double scale, Vec3d v)
        {
            return new Vec3d(v.X / scale, v.Y / scale, v.Z / scale);
        }

        public double Length()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            return (double)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public double LengthSquared()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            return (double)((x * x) + (y * y) + (z * z));
        }

        public Vec3d Normalize()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double num;
            double length = (double)Math.Sqrt((x * x) + (y * y) + (z * z));

            if (length != 0f)
            {
                num = (double)(1.0 / ((double)length));
            }
            else
            {
                num = 1;
            }

            return new Vec3d(this.X * num, this.Y * num, this.Z * num);
        }

        public static bool operator ==(Vec3d left, Vec3d right)
        {
            return (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z);
        }

        public static bool operator !=(Vec3d left, Vec3d right)
        {
            return (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z);
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2}", this.X, this.Y, this.Z);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public bool Equals(Vec3d other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);
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
            return this.Equals((Vec3d)obj);
        }


        //public static void Barycentric(ref Vec3d value1, ref Vec3d value2, ref Vec3d value3, double amount1, double amount2, out Vec3d result)
        //{
        //    Vec3d vector;
        //    vector.X = (((value2.X - value1.X) * amount1) + value1.X) + ((value3.X - value1.X) * amount2);
        //    vector.Y = (((value2.Y - value1.Y) * amount1) + value1.Y) + ((value3.Y - value1.Y) * amount2);
        //    vector.Z = (((value2.Z - value1.Z) * amount1) + value1.Z) + ((value3.Z - value1.Z) * amount2);
        //    result = vector;
        //}

        //public static Vec3d Barycentric(Vec3d value1, Vec3d value2, Vec3d value3, double amount1, double amount2)
        //{
        //    return new Vec3d { X = (((value2.X - value1.X) * amount1) + value1.X) + ((value3.X - value1.X) * amount2), Y = (((value2.Y - value1.Y) * amount1) + value1.Y) + ((value3.Y - value1.Y) * amount2), Z = (((value2.Z - value1.Z) * amount1) + value1.Z) + ((value3.Z - value1.Z) * amount2) };
        //}

        //public static void CatmullRom(ref Vec3d value1, ref Vec3d value2, ref Vec3d value3, ref Vec3d value4, double amount, out Vec3d result)
        //{
        //    double num = amount;
        //    double squared = (double)(num * num);
        //    double cubed = squared * amount;
        //    Vec3d r = new Vec3d {
        //        X = (double)((((((((value1.X * 2.0) - (value2.X * 5.0)) + (value3.X * 4.0)) - value4.X) * squared) + (((value3.X - value1.X) * amount) + (value2.X * 2.0))) + (((((value2.X * 3.0) - value1.X) - (value3.X * 3.0)) + value4.X) * cubed)) * 0.5),
        //        Y = (double)((((((((value1.Y * 2.0) - (value2.Y * 5.0)) + (value3.Y * 4.0)) - value4.Y) * squared) + (((value3.Y - value1.Y) * amount) + (value2.Y * 2.0))) + (((((value2.Y * 3.0) - value1.Y) - (value3.Y * 3.0)) + value4.Y) * cubed)) * 0.5),
        //        Z = (double)((((((((value1.Z * 2.0) - (value2.Z * 5.0)) + (value3.Z * 4.0)) - value4.Z) * squared) + (((value3.Z - value1.Z) * amount) + (value2.Z * 2.0))) + (((((value2.Z * 3.0) - value1.Z) - (value3.Z * 3.0)) + value4.Z) * cubed)) * 0.5)
        //    };
        //    result = r;
        //}

        //public static Vec3d CatmullRom(Vec3d value1, Vec3d value2, Vec3d value3, Vec3d value4, double amount)
        //{
        //    Vec3d vector = new Vector3();
        //    double num = amount;
        //    double squared = (double)(num * num);
        //    double cubed = squared * amount;
        //    vector.X = (double)((((((((value1.X * 2.0) - (value2.X * 5.0)) + (value3.X * 4.0)) - value4.X) * squared) + (((value3.X - value1.X) * amount) + (value2.X * 2.0))) + (((((value2.X * 3.0) - value1.X) - (value3.X * 3.0)) + value4.X) * cubed)) * 0.5);
        //    vector.Y = (double)((((((((value1.Y * 2.0) - (value2.Y * 5.0)) + (value3.Y * 4.0)) - value4.Y) * squared) + (((value3.Y - value1.Y) * amount) + (value2.Y * 2.0))) + (((((value2.Y * 3.0) - value1.Y) - (value3.Y * 3.0)) + value4.Y) * cubed)) * 0.5);
        //    vector.Z = (double)((((((((value1.Z * 2.0) - (value2.Z * 5.0)) + (value3.Z * 4.0)) - value4.Z) * squared) + (((value3.Z - value1.Z) * amount) + (value2.Z * 2.0))) + (((((value2.Z * 3.0) - value1.Z) - (value3.Z * 3.0)) + value4.Z) * cubed)) * 0.5);
        //    return vector;
        //}

        //public static void Clamp(ref Vec3d value, ref Vec3d min, ref Vec3d max, out Vec3d result)
        //{
        //    double num;
        //    double num2;
        //    double num3;
        //    double num4;
        //    double num5;
        //    double num6;
        //    Vec3d vector;
        //    double x = value.X;
        //    if (x > max.X)
        //    {
        //        num3 = max.X;
        //    }
        //    else
        //    {
        //        num3 = x;
        //    }
        //    if (num3 < min.X)
        //    {
        //        num6 = min.X;
        //    }
        //    else
        //    {
        //        num6 = num3;
        //    }
        //    double y = value.Y;
        //    if (y > max.Y)
        //    {
        //        num2 = max.Y;
        //    }
        //    else
        //    {
        //        num2 = y;
        //    }
        //    if (num2 < min.Y)
        //    {
        //        num5 = min.Y;
        //    }
        //    else
        //    {
        //        num5 = num2;
        //    }
        //    double z = value.Z;
        //    if (z > max.Z)
        //    {
        //        num = max.Z;
        //    }
        //    else
        //    {
        //        num = z;
        //    }
        //    if (num < min.Z)
        //    {
        //        num4 = min.Z;
        //    }
        //    else
        //    {
        //        num4 = num;
        //    }
        //    vector.X = num6;
        //    vector.Y = num5;
        //    vector.Z = num4;
        //    result = vector;
        //}

        //public static Vec3d Clamp(Vec3d value, Vec3d min, Vec3d max)
        //{
        //    double num;
        //    double num2;
        //    double num3;
        //    double num4;
        //    double num5;
        //    double num6;
        //    Vec3d vector;
        //    double x = value.X;
        //    if (x > max.X)
        //    {
        //        num3 = max.X;
        //    }
        //    else
        //    {
        //        num3 = x;
        //    }
        //    if (num3 < min.X)
        //    {
        //        num6 = min.X;
        //    }
        //    else
        //    {
        //        num6 = num3;
        //    }
        //    double y = value.Y;
        //    if (y > max.Y)
        //    {
        //        num2 = max.Y;
        //    }
        //    else
        //    {
        //        num2 = y;
        //    }
        //    if (num2 < min.Y)
        //    {
        //        num5 = min.Y;
        //    }
        //    else
        //    {
        //        num5 = num2;
        //    }
        //    double z = value.Z;
        //    if (z > max.Z)
        //    {
        //        num = max.Z;
        //    }
        //    else
        //    {
        //        num = z;
        //    }
        //    if (num < min.Z)
        //    {
        //        num4 = min.Z;
        //    }
        //    else
        //    {
        //        num4 = num;
        //    }
        //    vector.X = num6;
        //    vector.Y = num5;
        //    vector.Z = num4;
        //    return vector;
        //}

        //public static void Hermite(ref Vec3d value1, ref Vec3d tangent1, ref Vec3d value2, ref Vec3d tangent2, double amount, out Vec3d result)
        //{
        //    double num2 = amount;
        //    double squared = (double)(num2 * num2);
        //    double cubed = squared * amount;
        //    double num = squared * 3.0;
        //    double part1 = (double)(((cubed * 2.0) - num) + 1.0);
        //    double part2 = (double)((cubed * -2.0) + num);
        //    double part3 = (cubed - ((double)(squared * 2.0))) + amount;
        //    double part4 = cubed - squared;
        //    result.X = (((value2.X * part2) + (value1.X * part1)) + (tangent1.X * part3)) + (tangent2.X * part4);
        //    result.Y = (((value2.Y * part2) + (value1.Y * part1)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
        //    result.Z = (((value2.Z * part2) + (value1.Z * part1)) + (tangent1.Z * part3)) + (tangent2.Z * part4);
        //}

        //public static Vec3d Hermite(Vec3d value1, Vec3d tangent1, Vec3d value2, Vec3d tangent2, double amount)
        //{
        //    Vec3d vector = new Vector3();
        //    double num2 = amount;
        //    double squared = (double)(num2 * num2);
        //    double cubed = squared * amount;
        //    double num = squared * 3.0;
        //    double part1 = (double)(((cubed * 2.0) - num) + 1.0);
        //    double part2 = (double)((cubed * -2.0) + num);
        //    double part3 = (cubed - ((double)(squared * 2.0))) + amount;
        //    double part4 = cubed - squared;
        //    vector.X = (((value2.X * part2) + (value1.X * part1)) + (tangent1.X * part3)) + (tangent2.X * part4);
        //    vector.Y = (((value2.Y * part2) + (value1.Y * part1)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
        //    vector.Z = (((value2.Z * part2) + (value1.Z * part1)) + (tangent1.Z * part3)) + (tangent2.Z * part4);
        //    return vector;
        //}

        //public static void Lerp(ref Vec3d start, ref Vec3d end, double amount, out Vec3d result)
        //{
        //    result.X = ((end.X - start.X) * amount) + start.X;
        //    result.Y = ((end.Y - start.Y) * amount) + start.Y;
        //    result.Z = ((end.Z - start.Z) * amount) + start.Z;
        //}

        //public static Vec3d Lerp(Vec3d start, Vec3d end, double amount)
        //{
        //    return new Vec3d { X = ((end.X - start.X) * amount) + start.X, Y = ((end.Y - start.Y) * amount) + start.Y, Z = ((end.Z - start.Z) * amount) + start.Z };
        //}

        //public static void SmoothStep(ref Vec3d start, ref Vec3d end, double amount, out Vec3d result)
        //{
        //    double num;
        //    if (amount > 1f)
        //    {
        //        num = 1f;
        //    }
        //    else
        //    {
        //        double num2;
        //        if (amount < 0f)
        //        {
        //            num2 = 0f;
        //        }
        //        else
        //        {
        //            num2 = amount;
        //        }
        //        num = num2;
        //    }
        //    double num3 = num;
        //    amount = (double)((3.0 - (num * 2.0)) * (num3 * num3));
        //    result.X = ((end.X - start.X) * amount) + start.X;
        //    result.Y = ((end.Y - start.Y) * amount) + start.Y;
        //    result.Z = ((end.Z - start.Z) * amount) + start.Z;
        //}

        //public static Vec3d SmoothStep(Vec3d start, Vec3d end, double amount)
        //{
        //    double num;
        //    Vec3d vector = new Vector3();
        //    if (amount > 1f)
        //    {
        //        num = 1f;
        //    }
        //    else
        //    {
        //        double num2;
        //        if (amount < 0f)
        //        {
        //            num2 = 0f;
        //        }
        //        else
        //        {
        //            num2 = amount;
        //        }
        //        num = num2;
        //    }
        //    double num3 = num;
        //    amount = (double)((3.0 - (num * 2.0)) * (num3 * num3));
        //    vector.X = ((end.X - start.X) * amount) + start.X;
        //    vector.Y = ((end.Y - start.Y) * amount) + start.Y;
        //    vector.Z = ((end.Z - start.Z) * amount) + start.Z;
        //    return vector;
        //}

        //public static double Distance(Vec3d value1, Vec3d value2)
        //{
        //    double x = value1.X - value2.X;
        //    double y = value1.Y - value2.Y;
        //    double z = value1.Z - value2.Z;
        //    double num3 = y;
        //    double num2 = x;
        //    double num = z;
        //    return (double)Math.Sqrt(((num2 * num2) + (num3 * num3)) + (num * num));
        //}

        //public static double DistanceSquared(Vec3d value1, Vec3d value2)
        //{
        //    double x = value1.X - value2.X;
        //    double y = value1.Y - value2.Y;
        //    double z = value1.Z - value2.Z;
        //    double num3 = y;
        //    double num2 = x;
        //    double num = z;
        //    return (double)(((num2 * num2) + (num3 * num3)) + (num * num));
        //}

        //public static double Dot(Vec3d left, Vec3d right)
        //{
        //    return (((left.Y * right.Y) + (left.X * right.X)) + (left.Z * right.Z));
        //}

        //public static void Cross(ref Vec3d left, ref Vec3d right, out Vec3d result)
        //{
        //    Vec3d r = new Vec3d {
        //        X = (left.Y * right.Z) - (left.Z * right.Y),
        //        Y = (left.Z * right.X) - (left.X * right.Z),
        //        Z = (left.X * right.Y) - (left.Y * right.X)
        //    };
        //    result = r;
        //}

        //public static Vec3d Cross(Vec3d left, Vec3d right)
        //{
        //    return new Vec3d { X = (right.Z * left.Y) - (left.Z * right.Y), Y = (left.Z * right.X) - (right.Z * left.X), Z = (right.Y * left.X) - (left.Y * right.X) };
        //}

        //public static void Reflect(ref Vec3d vector, ref Vec3d normal, out Vec3d result)
        //{
        //    double dot = ((vector.Y * normal.Y) + (vector.X * normal.X)) + (vector.Z * normal.Z);
        //    double num = dot * 2.0;
        //    result.X = vector.X - ((double)(normal.X * num));
        //    result.Y = vector.Y - ((double)(normal.Y * num));
        //    result.Z = vector.Z - ((double)(normal.Z * num));
        //}

        //public static Vec3d Reflect(Vec3d vector, Vec3d normal)
        //{
        //    Vec3d result = new Vector3();
        //    double dot = ((vector.Y * normal.Y) + (vector.X * normal.X)) + (vector.Z * normal.Z);
        //    double num = dot * 2.0;
        //    result.X = vector.X - ((double)(normal.X * num));
        //    result.Y = vector.Y - ((double)(normal.Y * num));
        //    result.Z = vector.Z - ((double)(normal.Z * num));
        //    return result;
        //}

        //public static void Transform(ref Vec3d vector, ref Matrix4f transformation, out Vec4d result)
        //{
        //    result = new Vector4();
        //    result.X = (((vector.Y * transformation.M21) + (vector.X * transformation.M11)) + (vector.Z * transformation.M31)) + transformation.M41;
        //    result.Y = (((vector.Y * transformation.M22) + (vector.X * transformation.M12)) + (vector.Z * transformation.M32)) + transformation.M42;
        //    result.Z = (((vector.Y * transformation.M23) + (vector.X * transformation.M13)) + (vector.Z * transformation.M33)) + transformation.M43;
        //    result.W = (((transformation.M24 * vector.Y) + (transformation.M14 * vector.X)) + (vector.Z * transformation.M34)) + transformation.M44;
        //}

        //public static Vec4d Transform(Vec3d vector, Matrix4f transformation)
        //{
        //    return new Vec4d { X = (((transformation.M21 * vector.Y) + (transformation.M11 * vector.X)) + (transformation.M31 * vector.Z)) + transformation.M41, Y = (((transformation.M22 * vector.Y) + (transformation.M12 * vector.X)) + (transformation.M32 * vector.Z)) + transformation.M42, Z = (((transformation.M23 * vector.Y) + (transformation.M13 * vector.X)) + (transformation.M33 * vector.Z)) + transformation.M43, W = (((transformation.M24 * vector.Y) + (transformation.M14 * vector.X)) + (transformation.M34 * vector.Z)) + transformation.M44 };
        //}

        //public static void TransformCoordinate(ref Vec3d coordinate, ref Matrix4f transformation, out Vec3d result)
        //{
        //    Vec3d vector2;
        //    Vec4d vector = new Vec4d {
        //        X = (((coordinate.Y * transformation.M21) + (coordinate.X * transformation.M11)) + (coordinate.Z * transformation.M31)) + transformation.M41,
        //        Y = (((coordinate.Y * transformation.M22) + (coordinate.X * transformation.M12)) + (coordinate.Z * transformation.M32)) + transformation.M42,
        //        Z = (((coordinate.Y * transformation.M23) + (coordinate.X * transformation.M13)) + (coordinate.Z * transformation.M33)) + transformation.M43
        //    };
        //    double num = (double)(1.0 / ((((transformation.M24 * coordinate.Y) + (transformation.M14 * coordinate.X)) + (coordinate.Z * transformation.M34)) + transformation.M44));
        //    vector.W = num;
        //    vector2.X = vector.X * num;
        //    vector2.Y = vector.Y * num;
        //    vector2.Z = vector.Z * num;
        //    result = vector2;
        //}

        public static Vec3d TransformCoordinate(Vec3d coordinate, Matrix4f transformation)
        {
            double num = (1.0f / ((((transformation.M24 * coordinate.Y) + (transformation.M14 * coordinate.X)) + (transformation.M34 * coordinate.Z)) + transformation.M44));
            
            return new Vec3d(
                ((((transformation.M21 * coordinate.Y) + (transformation.M11 * coordinate.X)) + (transformation.M31 * coordinate.Z)) + transformation.M41) * num,
                ((((transformation.M22 * coordinate.Y) + (transformation.M12 * coordinate.X)) + (transformation.M32 * coordinate.Z)) + transformation.M42) * num,
                ((((transformation.M23 * coordinate.Y) + (transformation.M13 * coordinate.X)) + (transformation.M33 * coordinate.Z)) + transformation.M43) * num
            );
        }

        //public static void TransformNormal(ref Vec3d normal, ref Matrix4f transformation, out Vec3d result)
        //{
        //    result.X = ((normal.Y * transformation.M21) + (normal.X * transformation.M11)) + (normal.Z * transformation.M31);
        //    result.Y = ((normal.Y * transformation.M22) + (normal.X * transformation.M12)) + (normal.Z * transformation.M32);
        //    result.Z = ((normal.Y * transformation.M23) + (normal.X * transformation.M13)) + (normal.Z * transformation.M33);
        //}

        //public static Vec3d TransformNormal(Vec3d normal, Matrix4f transformation)
        //{
        //    return new Vec3d { X = ((transformation.M21 * normal.Y) + (transformation.M11 * normal.X)) + (transformation.M31 * normal.Z), Y = ((transformation.M22 * normal.Y) + (transformation.M12 * normal.X)) + (transformation.M32 * normal.Z), Z = ((transformation.M23 * normal.Y) + (transformation.M13 * normal.X)) + (transformation.M33 * normal.Z) };
        //}

        //public static void Project(ref Vec3d vector, double x, double y, double width, double height, double minZ, double maxZ, ref Matrix4f worldViewProjection, out Vec3d result)
        //{
        //    Vec3d v2 = new Vector3();
        //    Vec3d v = new Vector3();
        //    TransformCoordinate(ref vector, ref worldViewProjection, out v);
        //    v2.X = ((double)(((v.X + 1.0) * 0.5) * width)) + x;
        //    v2.Y = ((double)(((1.0 - v.Y) * 0.5) * height)) + y;
        //    v2.Z = (v.Z * (maxZ - minZ)) + minZ;
        //    result = v2;
        //}

        //public static Vec3d Project(Vec3d vector, double x, double y, double width, double height, double minZ, double maxZ, Matrix4f worldViewProjection)
        //{
        //    Vec3d v2;
        //    TransformCoordinate(ref vector, ref worldViewProjection, out vector);
        //    v2.X = ((double)(((vector.X + 1.0) * 0.5) * width)) + x;
        //    v2.Y = ((double)(((1.0 - vector.Y) * 0.5) * height)) + y;
        //    v2.Z = (vector.Z * (maxZ - minZ)) + minZ;
        //    return v2;
        //}

        //public static void Minimize(ref Vec3d value1, ref Vec3d value2, out Vec3d result)
        //{
        //    double z;
        //    double y;
        //    double x;
        //    if (value1.X < value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    result.X = x;
        //    if (value1.Y < value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    result.Y = y;
        //    if (value1.Z < value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    result.Z = z;
        //}

        //public static Vec3d Minimize(Vec3d value1, Vec3d value2)
        //{
        //    double z;
        //    double y;
        //    double x;
        //    Vec3d vector = new Vector3();
        //    if (value1.X < value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    vector.X = x;
        //    if (value1.Y < value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    vector.Y = y;
        //    if (value1.Z < value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    vector.Z = z;
        //    return vector;
        //}

        //public static void Maximize(ref Vec3d value1, ref Vec3d value2, out Vec3d result)
        //{
        //    double z;
        //    double y;
        //    double x;
        //    if (value1.X > value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    result.X = x;
        //    if (value1.Y > value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    result.Y = y;
        //    if (value1.Z > value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    result.Z = z;
        //}

        //public static Vec3d Maximize(Vec3d value1, Vec3d value2)
        //{
        //    double z;
        //    double y;
        //    double x;
        //    Vec3d vector = new Vector3();
        //    if (value1.X > value2.X)
        //    {
        //        x = value1.X;
        //    }
        //    else
        //    {
        //        x = value2.X;
        //    }
        //    vector.X = x;
        //    if (value1.Y > value2.Y)
        //    {
        //        y = value1.Y;
        //    }
        //    else
        //    {
        //        y = value2.Y;
        //    }
        //    vector.Y = y;
        //    if (value1.Z > value2.Z)
        //    {
        //        z = value1.Z;
        //    }
        //    else
        //    {
        //        z = value2.Z;
        //    }
        //    vector.Z = z;
        //    return vector;
        //}
    }
}