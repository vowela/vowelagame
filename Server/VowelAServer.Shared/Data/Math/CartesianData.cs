namespace VowelAServer.Shared.Data.Math
{
    using System;

    public class CartesianData { }

    public class Vector
    {
        public double X;
        public double Y;
        public double Z;
        public Point Origin = new Point();

        public Vector(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point End() => Origin + this;

        public double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public class Point
    {
        public double X;
        public double Y;
        public double Z;

        public Point(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point operator +(Point p, Vector v) => new Point(p.X + v.X, p.Y + v.Y, p.Z + v.Z);

        public double Distance(Point p2) => Math.Sqrt(Math.Pow(p2.X - X, 2) + Math.Pow(p2.Y - Y, 2));
    }
}
