using Genetik.Core;

namespace Genetik.Evolution.Blueprints.Fitness;

public class ComplexFitnessEvaluator<TGene> : IFitnessEvaluator<TGene>
{
    private List<ICriteria<TGene>> _criteriaList;
    private readonly List<WeightedEvaluator> _weightedEvaluators;
    
    private ComplexFitnessEvaluator(
        List<ICriteria<TGene>> criteriaList,
        List<WeightedEvaluator> weightedEvaluators)
    {
        _criteriaList = criteriaList;
        _weightedEvaluators = weightedEvaluators;
    }
    public double GetFitness(Genome<TGene> genome)
    {
        return _criteriaList.Any(c => !c.MeetCriteria(genome)) 
            ? double.NegativeInfinity 
            : _weightedEvaluators.Sum(
                weighted => weighted.GetWeightedFitness(genome));
    }

    public class Builder
    {
        private readonly List<ICriteria<TGene>> _criteriaList = new();
        private readonly List<WeightedEvaluator> _weightedEvaluators = new();

        public Builder(IFitnessEvaluator<TGene> evaluator, double weight)
        {
            _weightedEvaluators.Add(new WeightedEvaluator(evaluator, weight));
        }

        public Builder AddCriteria(ICriteria<TGene> criteria)
        {
            _criteriaList.Add(criteria);
            return this;
        }
        
        public Builder AddWeightedEvaluator(IFitnessEvaluator<TGene> evaluator, double weight)
        {
            _weightedEvaluators.Add(new WeightedEvaluator(evaluator, weight));
            return this;
        }
        
        

        public ComplexFitnessEvaluator<TGene> Build()
        {
            NormalizeWeights();
            return new ComplexFitnessEvaluator<TGene>(_criteriaList, _weightedEvaluators);
        }

        private void NormalizeWeights()
        {
            double sum = _weightedEvaluators.Sum(c => Math.Abs(c.Weight));
            for (int i = 0; i < _weightedEvaluators.Count; i++)
            {
                double normalizedWeight = _weightedEvaluators[i].Weight / sum;
                _weightedEvaluators[i].Weight = normalizedWeight;
            }
        }
    }

    private class WeightedEvaluator
    {
        public WeightedEvaluator(IFitnessEvaluator<TGene> fitnessEvaluator, double weight)
        {
            FitnessEvaluator = fitnessEvaluator;
            Weight = weight;
        }

        public IFitnessEvaluator<TGene> FitnessEvaluator { get; }

        public double Weight { get; set; }

        public double GetWeightedFitness(Genome<TGene> genome)
            => FitnessEvaluator.GetFitness(genome) * Weight;
    }
}