namespace Genetik.Core;

public interface IEvolutionProcess<TGene>
{
    IReadOnlyList<Genome<TGene>> CurrGeneration { get; }
    Genome<TGene> BestGenome { get; }
    IReadOnlyList<Genome<TGene>> NextGeneration();
}