// See https://aka.ms/new-console-template for more information

using Genetik.Evolution.Blueprints.Fitness;
using Genetik.Evolution.Blueprints.Processes;
using Genetik.Sandbox.Evolution.Crossing;
using Genetik.Sandbox.Evolution.Fitness;
using Genetik.Sandbox.Evolution.Generation;
using Genetik.Sandbox.Evolution.Mutation;
using Genetik.Sandbox.Logic;

int generationSize = 250;
int genomeLength = 10;

double elitismRate = 0.15;

double w = 100;
double maxAbsX = w / 5;



var field = new Field(w, w, Array.Empty<Circle>());

var generator = new RandomGenerator(maxAbsX, maxAbsX);

var evaluator = new ComplexFitnessEvaluator<Vec2>
    .Builder(new PathLengthEvaluator(field), 1.0)
    .AddWeightedEvaluator(new DistanceToFinishEvaluator(field), 2.0)
    .AddCriteria(new WholePathInsideCriteria(field))
    .AddCriteria(new NoCollisionCriteria(field))
    .Build();

var crosser = new RandomSinglePointCrosser();

var mutator = new RandomMutator(0.1, maxAbsX, maxAbsX);

var evolution = new ElitismEvolution<Vec2>(
    elitismRate,
    genomeLength,
    generationSize,
    generator,
    evaluator,
    crosser,
    mutator);

for (int i = 0; i < 1000; i++)
{
    evolution.NextGeneration();
    // Console.WriteLine(i);
}

var genome = evolution.BestGenome;

var curr = field.Start;
for (int i = 0; i < genome.Length; i++)
{
    curr += genome.Genes[i];
}

Console.WriteLine(curr);
