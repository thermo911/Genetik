using Genetik.Core;
using Genetik.Sandbox.Logic;

namespace Genetik.Sandbox.Evolution.Fitness;

public class PathLengthEvaluator : IFitnessEvaluator<Vec2>
{
    private Field _field;

    public PathLengthEvaluator(Field field)
    {
        _field = field;
    }

    public double GetFitness(Genome<Vec2> genome)
    {
        double length = 0.0;
        for (int i = 0; i < genome.Length; i++)
        {
            length += genome.Genes[i].Length;
        }

        return -length;
    }
}