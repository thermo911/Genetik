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
    private List<Genome<TGene>> _currGeneration = null!;

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

        InitCurrGeneration(genomesGenerator, genomeLength, populationSize);
        InitElitismCount(elitismRate);
    }

    public IReadOnlyList<Genome<TGene>> CurrGeneration => _currGeneration;

    public Genome<TGene> BestGenome { get; private set; } = null!;

    public IReadOnlyList<Genome<TGene>> NextGeneration()
    {
        var newGeneration = new List<Genome<TGene>>(CurrGeneration.Count);

        MoveElites(_currGeneration, newGeneration);           // save best genomes
        AddCrossedOrdinaries(_currGeneration, newGeneration); // replace ordinaries with their children
        AddCrossedElites(_currGeneration, newGeneration);     // replace worst with elites' children

        newGeneration.Sort(GenomesDescComparison);
        _currGeneration = newGeneration;

        if (GenomesDescComparison(BestGenome, _currGeneration[0]) > 0)
            BestGenome = _currGeneration[0];
        
        return _currGeneration;
    }

    private int GenomesDescComparison(Genome<TGene> genome1, Genome<TGene> genome2)
    {
        if (_fitnessEvaluator.GetFitness(genome1) > _fitnessEvaluator.GetFitness(genome2))
            return -1;
        return 1;
    }

    private void InitCurrGeneration(IGenomesGenerator<TGene> generator, int genomeLength, int populationSize)
    {
        _currGeneration = generator.GenerateGenomes(genomeLength, populationSize)
            .OrderByDescending(_fitnessEvaluator.GetFitness)
            .ToList();

        BestGenome = _currGeneration[0];
    }

    private void InitElitismCount(double elitismRate)
    {
        _elitismCount = (int)(elitismRate * CurrGeneration.Count);
        if (_elitismCount % 2 == 1)
            _elitismCount++;

        if (_elitismCount > CurrGeneration.Count / 2)
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
        int pairsCount = orderedGenomes.Count / 2 - _elitismCount;
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