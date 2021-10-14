using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeneticAlgorithm;
using System.Collections.Generic;

namespace GeneticAlgorithmTest
{
    [TestClass]
    public class PopulationTest
    {
        [TestMethod]
        public void Pair_WhenCalledOnNotNullPopulation_PairsGenomes()
        {
            Population population = new Population(size: 10, genomeSize: 8);
            population.pair();
            bool allPaired = true, isUnique = true;
            foreach (Genome genome in population._genomes)
            {
                if (genome.Pair == null)
                {
                    allPaired = false;
                    break;
                }
            }

            for (int i = 0; i < population._genomes.Count; i++)
            {
                for (int j = 0; j < population._genomes.Count; j++)
                {
                    if (i == j) continue;
                    if (population._genomes[i].Equals(population._genomes[j]))
                    {
                        isUnique = false;
                        break;
                    }
                }
            }
            Assert.IsTrue(allPaired && isUnique);
        }
    }
}
