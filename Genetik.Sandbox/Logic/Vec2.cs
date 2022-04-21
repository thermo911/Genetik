namespace Genetik.Sandbox.Logic;

public struct Vec2
{
    public Vec2()
    {
        X = 0.0;
        Y = 0.0;
    }
    
    public Vec2(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }

    public double Y { get; }

    public double Length => Math.Sqrt(X * X + Y * Y);

    public double Dist(Vec2 other)
    {
        return Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
    }

    public static Vec2 operator +(Vec2 v1, Vec2 v2)
    {
        return new Vec2(v1.X + v2.X, v1.Y + v2.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}