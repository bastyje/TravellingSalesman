using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeneticAlgorithm;
using System.Collections.Generic;

namespace GeneticAlgorithmTest
{
    [TestClass]
    public class PopulationTest
    {
        [TestMethod]
        public void Population_WhenCalledWithListOfGenomesAndProbabilityPassed_CreatesPopulationWithAllFieldsNotNull()
        {
            List<Genome> genomes = new List<Genome>();
            for (int i = 0; i < 10; i++) genomes.Add(new Genome(8));
            Population population = new Population(genomes, 0.0);
            bool allIsNotNull = true;
            if (population.Genomes == null) allIsNotNull = false;
            foreach (Genome genome in population.Genomes)
            {
                //egenome.
            }
        }
    }
}
