namespace Genetik.Core;

public interface IPopulationGenerator<TGene>
{
    Population<TGene> GeneratePopulation();
}