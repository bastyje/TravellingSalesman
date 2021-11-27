using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GeneticAlgorithm
{
    class Launcher
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument<int>(
                    "pop-size",
                    description: "Size of population. Only integer values allowed."
                ),
                new Argument<double>(
                    "gen-size",
                    description: "Size of genome. Number of genes in genome. Has to be greater than 0."
                ),
                new Argument<double>(
                    "mut-prob",
                    description: "Probability of mutation of single gene. Only values" +
                    " between 0 and 1 allowed."
                ),
                new Argument<int>(
                    "end-cond",
                    description: "Number of generations of population without progress (the best adaptation of genome has " +
                    "not risen since end-cond number of generations."
                    )
            };
            rootCommand.Description =
                "Apps uses genetic algorithm to find maximum of function " +
                "of one variable defined for integers.";
            rootCommand.Handler = CommandHandler.Create<int, int, double, int>(Search);
            return rootCommand.InvokeAsync(args).Result;
        }

        public static void Search(int popSize, int genomeSize, double mutProb, int endCond)
        {
            Population population = new Population(popSize, genomeSize, mutProb);

        }
    }
}
