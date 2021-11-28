using GeneticAlgorithm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithmTest
{
    [TestClass]
    public class GenomeTest
    {
        [TestMethod]
        public void Genome_GivenArrayWithZerosAndOnesOnly_CreatesGenomeObject()
        {
            int[] array = { 1, 0, 1, 1, 0 };
            Chromosome genome = new Chromosome(array);
            Assert.IsInstanceOfType(genome, typeof(Chromosome));
        }

        [TestMethod]
        public void Genome_GivenIntPositiveInt_CreatesGenomeObject()
        {
            Chromosome genome = new Chromosome(5);
            Assert.IsInstanceOfType(genome, typeof(Chromosome));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Genome_GivenArrayWithNotOnlyZerosAndOnes_ThrowsArgumentException()
        {
            int[] array = { 1, 2, 1, 0, 1 };
            new Chromosome(array);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Genome_GivenNegativeInt_ThrowsArgumentException()
        {
            new Chromosome(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Genome_GivenIntEqualZero_ThrowsArgumentException()
        {
            new Chromosome(0);
        }
    }
}
