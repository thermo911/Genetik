using Genetik.Core;
using Genetik.Evolution.Blueprints.Fitness;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Fitness;

public class NoCollisionCriteria : ICriteria<Vec2>
{
    private Field _field;

    public NoCollisionCriteria(Field field)
    {
        _field = field;
    }

    public bool MeetCriteria(Genome<Vec2> genome)
    {
        var curr = _field.Start;
        var next = curr;

        for (int i = 0; i < genome.Length; i++)
        {
            next += genome.Genes[i];
            if (_field.Circles.Any(c => Collision(curr, next, c)))
                return false;
            curr = next;
        }

        return true;
    }

    private bool Collision(Vec2 v1, Vec2 v2, Circle circle)
    {
        return SegmentPointDist(v1, v2, circle.Center) < circle.Radius;
    }
    
    private static double SegmentPointDist(Vec2 v1, Vec2 v2, Vec2 p)
    {
        double t = ((p.X - v1.X) * (v2.X - v1.X) 
                    + (p.Y - v1.Y) * (v2.Y - v1.Y)) 
                   / Math.Pow(v1.Dist(v2), 2);

        if (t < 0.0) t = 0.0;
        if (t > 1.0) t = 1.0;

        return Math.Sqrt(
            Math.Pow(v1.X - p.X + (v2.X - v1.X) * t, 2) +
            Math.Pow(v1.Y - p.Y + (v2.Y - v1.Y) * t, 2));
    }
}