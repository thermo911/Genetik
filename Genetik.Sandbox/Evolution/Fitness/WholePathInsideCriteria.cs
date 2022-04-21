using Genetik.Core;
using Genetik.Evolution.Blueprints.Fitness;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Fitness;

public class WholePathInsideCriteria : ICriteria<Vec2>
{
    private Field _field;

    public WholePathInsideCriteria(Field field)
    {
        _field = field;
    }

    public bool MeetCriteria(Genome<Vec2> genome)
    {
        var curr = _field.Start;
        for (int i = 0; i < genome.Length; i++)
        {
            curr += genome.Genes[i];
            if (OutOfField(curr))
                return false;
        }

        return true;
    }

    private bool OutOfField(Vec2 point)
    {
        return point.X < 0 || 
               point.Y < 0 || 
               point.X > _field.Width || 
               point.Y > _field.Height;
    }
}