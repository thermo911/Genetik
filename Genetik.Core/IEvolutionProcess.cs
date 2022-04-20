namespace Genetik.Core;

public interface IEvolutionProcess<TGene>
{
    Population<TGene> CurrGeneration { get; }
    
    Population<TGene> NextGeneration();
}