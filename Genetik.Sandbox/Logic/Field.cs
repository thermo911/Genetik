namespace Genetik.Sandbox.Logic;

public class Field
{
    public Field(double height, double width, IEnumerable<Circle> circles)
    {
        Height = height;
        Width = width;
        Circles = circles.ToArray();
        Start = new Vec2();
        Finish = new Vec2(width, height);
    }

    public Field(double height, double width, Vec2 start, Vec2 finish, IEnumerable<Circle> circles)
        : this(height, width, circles)
    {
        Start = start;
        Finish = finish;
    }

    public double Height { get; }
    public double Width { get; }

    public Vec2 Start { get; }
    public Vec2 Finish { get; }

    public IEnumerable<Circle> Circles { get; }
}