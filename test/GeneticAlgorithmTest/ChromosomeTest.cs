using GeneticAlgorithm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithmTest
{
    [TestClass]
    public class ChromosomeTest
    {
        [TestMethod]
        public void Chromosome_GivenArrayWithZerosAndOnesOnly_CreatesGenomeObject()
        {
            int[] array = { 1, 0, 1, 1, 0 };
            Chromosome chromosome = new Chromosome(array);
            Assert.IsInstanceOfType(chromosome, typeof(Chromosome));
        }

        [TestMethod]
        public void Chromosome_GivenIntPositiveInt_CreatesGenomeObject()
        {
            Chromosome genome = new Chromosome(5);
            Assert.IsInstanceOfType(genome, typeof(Chromosome));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Chromosome_GivenArrayWithNotOnlyZerosAndOnes_ThrowsArgumentException()
        {
            int[] array = { 1, 2, 1, 0, 1 };
            new Chromosome(array);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Chromosome_GivenNegativeInt_ThrowsArgumentException()
        {
            new Chromosome(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Chromosome_GivenIntEqualZero_ThrowsArgumentException()
        {
            new Chromosome(0);
        }

        [TestMethod]
        public void Decode_Called_ReturnsActualChromosomeValueInDecimalSystem()
        {
            int[] arr = { 1, 0, 1 };
            Assert.AreEqual(new Chromosome(arr).Decode(0, 7), 5);
        }
    }
}
