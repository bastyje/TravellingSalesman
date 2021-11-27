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
            Genome genome = new Genome(array);
            Assert.IsInstanceOfType(genome, typeof(Genome));
        }

        [TestMethod]
        public void Genome_GivenIntPositiveInt_CreatesGenomeObject()
        {
            Genome genome = new Genome(5);
            Assert.IsInstanceOfType(genome, typeof(Genome));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Genome_GivenArrayWithNotOnlyZerosAndOnes_ThrowsArgumentException()
        {
            int[] array = { 1, 2, 1, 0, 1 };
            new Genome(array);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Genome_GivenNegativeInt_ThrowsArgumentException()
        {
            new Genome(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Genome_GivenIntEqualZero_ThrowsArgumentException()
        {
            new Genome(0);
        }
    }
}
