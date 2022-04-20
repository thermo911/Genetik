// See https://aka.ms/new-console-template for more information

using Genetik.Evolution.Tools;

var random = new Random(Guid.NewGuid().GetHashCode());

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(random.NextDistinctPair(3, 6));
}