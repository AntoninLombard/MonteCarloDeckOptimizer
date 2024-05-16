namespace MonteCarlo;

public static class Parameters
{
    //Each Threads train against a different deck when evaluating the win rate of the deck we want to optimize. 
    
    
    public static int nb_of_threads = 4; //TODO make the number of thread dynamic based on this number. Until then keep it at 4 !
    
    
    public static int fight_iterations_per_threads = 250;
    public static int cardSwap_iterations = 300;
    public static int training_iterations = 10;


    public static int maxManaCost = 8;
    public static int maxCardNb = 30;

    public static int maxPlayerLife = 20;

    public static int nbOptimizedCards = 200; // Number of cards we keep from the set list when we optimize it.



    public static string winrate_CSV_path = "./";
    public static string winrate_CSV_filename = "winrates";
    public static string deck_CSV_path = "./";
    public static string deck_CSV_filename = "deck";
    public static string setlist_CSV_path = "./";
    public static string setlist_CSV_filename = "setlist";
}