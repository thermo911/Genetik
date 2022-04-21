namespace Genetik.Core;

public interface IFitnessEvaluator<TGene>
{
    double GetFitness(Genome<TGene> genome);
}