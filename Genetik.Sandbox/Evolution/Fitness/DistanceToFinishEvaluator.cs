using Genetik.Core;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Fitness;

public class DistanceToFinishEvaluator : IFitnessEvaluator<Vec2>
{
    private Field _field;

    public DistanceToFinishEvaluator(Field field)
    {
        _field = field;
    }

    public double GetFitness(Genome<Vec2> genome)
    {
        var curr = _field.Start;
        for (int i = 0; i < genome.Length; i++)
        {
            curr += genome.Genes[i];
        }

        return -curr.Dist(_field.Finish);
    }
}