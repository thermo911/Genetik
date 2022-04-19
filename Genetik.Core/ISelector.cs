namespace Genetik.Core;

public interface ISelector<TGene>
{
    /// <summary>
    /// Selects genomes from <c>population</c>
    /// (using <c>fitnessEvaluator</c>) that are supposed to be crossed.
    /// </summary>
    IEnumerable<Genome<TGene>> SelectGenomes(
        Population<TGene> population,
        IFitnessEvaluator<TGene> fitnessEvaluator);
}