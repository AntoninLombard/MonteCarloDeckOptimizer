// See https://aka.ms/new-console-template for more information

using MonteCarlo;

DeckOptimizer deckOpt = new DeckOptimizer();
deckOpt.Init();
deckOpt.OptimizeDeck();

Console.WriteLine("Deck Optimizer:");