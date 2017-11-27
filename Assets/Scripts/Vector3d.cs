using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
    public struct Vector3d
    {
        public const float kEpsilon = 1E-05f;
        public double x;
        public double y;
        public double z;

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    default:
                        throw new IndexOutOfRangeException("Invalid index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3d index!");
                }
            }
        }

        public Vector3d normalized
        {
            get
            {
                return Vector3d.Normalize(this);
            }
        }

        public double magnitude
        {
            get
            {
                return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        

        public Vector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        
        public Vector3d(Vector3 v3)
        {
            this.x = (double)v3.x;
            this.y = (double)v3.y;
            this.z = (double)v3.z;
        }

        public Vector3d(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.z = 0d;
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3d operator -(Vector3d a)
        {
            return new Vector3d(-a.x, -a.y, -a.z);
        }

        public static Vector3d operator *(Vector3d a, double d)
        {
            return new Vector3d(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3d operator *(double d, Vector3d a)
        {
            return new Vector3d(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3d operator /(Vector3d a, double d)
        {
            return new Vector3d(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3d lhs, Vector3d rhs)
        {
            return (double)Vector3d.SqrMagnitude(lhs - rhs) < 0.0 / 1.0;
        }

        public static bool operator !=(Vector3d lhs, Vector3d rhs)
        {
            return (double)Vector3d.SqrMagnitude(lhs - rhs) >= 0.0 / 1.0;
        }

        public static explicit operator Vector3(Vector3d vector3d)
        {
            return new Vector3((float)vector3d.x, (float)vector3d.y, (float)vector3d.z);
        }
        

        public void Set(double new_x, double new_y, double new_z)
        {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
        }
        

        public static Vector3d Cross(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3d))
                return false;
            Vector3d vector3d = (Vector3d)other;
            if (this.x.Equals(vector3d.x) && this.y.Equals(vector3d.y))
                return this.z.Equals(vector3d.z);
            else
                return false;
        }
        
        public override string ToString()
        {
            return "(" + this.x + " - " + this.y + " - " + this.z + ")";
        }

        public static double Dot(Vector3d lhs, Vector3d rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static double Angle(Vector3d from, Vector3d to)
        {
            return Mathd.Acos(Mathd.Clamp(Vector3d.Dot(from.normalized, to.normalized), -1d, 1d)) * 57.29578d;
        }
        
    }
}