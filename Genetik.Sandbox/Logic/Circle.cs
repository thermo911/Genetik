namespace Genetik.Sandbox.Logic;

public class Circle
{
    public Circle(Vec2 center, double radius)
    {
        Center = center;
        Radius = radius;
    }

    public Vec2 Center { get; }
    
    public double Radius { get; }
}