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
    }
}
