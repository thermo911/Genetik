using Genetik.Core;
using Genetik.Evolution.Tools;

namespace Genetik.Evolution.Blueprints.Processes;

public sealed class ElitismEvolution<TGene> : IEvolutionProcess<TGene>
{
    private int _elitismCount;
    private readonly IFitnessEvaluator<TGene> _fitnessEvaluator;
    private readonly ICrosser<TGene> _crosser;
    private readonly IMutator<TGene> _mutator;
    private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
    private Population<TGene> _currGeneration = null!;

    public ElitismEvolution(
        double elitismRate,
        int genomeLength,
        int populationSize,
        IGenomesGenerator<TGene> genomesGenerator,
        IFitnessEvaluator<TGene> fitnessEvaluator,
        ICrosser<TGene> crosser,
        IMutator<TGene> mutator)
    {
        _fitnessEvaluator = fitnessEvaluator;
        _crosser = crosser;
        _mutator = mutator;

        if (populationSize % 2 == 1)
            populationSize++;

        var genomes =  genomesGenerator.GenerateGenomes(genomeLength, populationSize);
        CurrGeneration = new Population<TGene>(genomes, fitnessEvaluator);
        
        InitElitismCount(elitismRate);
    }

    public Population<TGene> CurrGeneration
    {
        get => _currGeneration;
        private set
        {
            _currGeneration = value;
            BestGenome = _currGeneration.OrderedGenomes[0];
        }
    }

    public Genome<TGene> BestGenome { get; private set; } = null!;

    public Population<TGene> NextGeneration()
    {
        var orderedGenomes = CurrGeneration.OrderedGenomes;
        var newGenomes = new List<Genome<TGene>>(CurrGeneration.Size);

        MoveElites(orderedGenomes, newGenomes);           // save best genomes
        AddCrossedOrdinaries(orderedGenomes, newGenomes); // replace ordinaries with their children
        AddCrossedElites(orderedGenomes, newGenomes);     // replace worst with elites' children

        CurrGeneration = new Population<TGene>(newGenomes, _fitnessEvaluator);
        return CurrGeneration;
    }

    private void InitElitismCount(double elitismRate)
    {
        _elitismCount = (int)(elitismRate * CurrGeneration.Size);
        if (_elitismCount % 2 == 1)
            _elitismCount++;

        if (_elitismCount > CurrGeneration.Size / 2)
            throw new ArgumentOutOfRangeException(nameof(elitismRate), 
                $"Given value of {nameof(elitismRate)} leads to" +
                $"count of elite genomes that is more than a half of population size");
    }

    private void MoveElites(IReadOnlyList<Genome<TGene>> orderedGenomes, List<Genome<TGene>> newGenomes)
    {
        for (int i = 0; i < _elitismCount; i++)
        {
            newGenomes.Add(_mutator.Mutate(orderedGenomes[i]));
        }
    }

    private void AddCrossedOrdinaries(IReadOnlyList<Genome<TGene>> orderedGenomes, List<Genome<TGene>> newGenomes)
    {
        int pairsCount = orderedGenomes.Count - _elitismCount * 2;
        for (int i = 0; i < pairsCount; i++)
        {
            var indices = _random.NextDistinctPair(_elitismCount, orderedGenomes.Count - _elitismCount);
            var crossingResult = _crosser.CrossGenomes(
                orderedGenomes[indices.Item1], orderedGenomes[indices.Item2]);
            
            newGenomes.Add(_mutator.Mutate(crossingResult.Item1));
            newGenomes.Add(_mutator.Mutate(crossingResult.Item2));
        }
    }

    private void AddCrossedElites(IReadOnlyList<Genome<TGene>> orderedGenomes, List<Genome<TGene>> newGenomes)
    {
        for (int i = 0; i < _elitismCount / 2; i++)
        {
            var indices = _random.NextDistinctPair(_elitismCount);
            var crossingResult = _crosser.CrossGenomes(
                orderedGenomes[indices.Item1], orderedGenomes[indices.Item2]);
            
            newGenomes.Add(_mutator.Mutate(crossingResult.Item1));
            newGenomes.Add(_mutator.Mutate(crossingResult.Item2));
        }
    }
}