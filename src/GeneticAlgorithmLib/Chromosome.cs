using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Chromosome
    {
        public List<int> Genes { get; }
        public int Length { get; }

        /// <summary>
        ///     Creates random genome with random arrangement of ZEROs and ONEs.
        /// </summary>
        /// <param name="length">
        ///     Length of genome to be created.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown when passed "length" argument is equal or less than 0.
        /// </exception>
        public Chromosome(int length)
        {
            if (!(length > 0)) throw new ArgumentException("Chromosome length has to be greater than 0.");
            this.Length = length;

            Genes = new List<int>(length);

            for (int i = 0; i < length; i++)
                Genes.Insert(i, i);

            Genes = Genes.OrderBy(a => Guid.NewGuid()).ToList();
        }
        public void SwapGenes(int index1, int index2)
        {
            int tmp = Genes[index1];
            Genes[index1] = Genes[index2];
            Genes[index2] = tmp;
        }

        public override string ToString()
        {
            String chain = "";
            for (int i = 0; i < Length; i++)
                chain += Genes[i].ToString();

            return chain;
        }
    }
}