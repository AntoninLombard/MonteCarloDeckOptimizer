namespace MonteCarlo;

public class DeckOptimizer
{
    private Random rand = new Random((int)DateTime.UtcNow.Ticks);
    private SetList setList = new SetList();

    private bool log = false;


    public void Init()
    {
        setList.CreateAllCards();
    }
    public void OptimizeDeck()
    {
        Deck deck0 = setList.GetRandomDeck();
        Deck deck1 = setList.GetRandomDeck();
        Deck deck2 = setList.GetRandomDeck();
        Deck deck3 = setList.GetRandomDeck();
        Deck deck4 = setList.GetRandomDeck();
        
        Trainning(deck0);
        Trainning(deck1);
        Trainning(deck2);
        Trainning(deck3);
        Trainning(deck4);

        Deck temp1 = deck1;
        Deck temp2 = deck2;
        deck1 = Deck.Fuse(deck2, deck4);
        deck2 = Deck.Fuse(temp1, deck3);
        deck3 = Deck.Fuse(temp2, deck3);
        deck4 = Deck.Fuse(temp1, deck4);
        
        Trainning(deck0,deck1,deck2,deck3, deck4);
        setList.OptimiseSetList();
        deck0 = setList.GetRandomDeck();
        Console.WriteLine("==========================================================================================================");
        Trainning(deck0);
        Trainning(deck1);
        Trainning(deck2);
        Trainning(deck3);
        Trainning(deck4);

        temp1 = deck1;
        temp2 = deck2;
        deck1 = Deck.Fuse(deck2, deck4);
        deck2 = Deck.Fuse(temp1, deck3);
        deck3 = Deck.Fuse(temp2, deck3);
        deck4 = Deck.Fuse(temp1, deck4);
        
        Trainning(deck0,deck1,deck2,deck3, deck4);
    }


    void Trainning(Deck deck0,Deck? deck1 = null,Deck? deck2 = null,Deck? deck3 = null,Deck? deck4 = null,bool useRandomDeck = true)
    {
        deck1 ??= setList.GetRandomDeck();
        deck2 ??= setList.GetRandomDeck();
        deck3 ??= setList.GetRandomDeck();
        deck4 ??= setList.GetRandomDeck();
        
        Game game1 = new Game();
        Game game2 = new Game();
        Game game3 = new Game();
        Game game4 = new Game();
        

        Thread thread1;
        Thread thread2;
        Thread thread3;
        Thread thread4;
        
        float initWinRate = 0f;
        float currentWinRate = 0f;
        float winRate = 0f;
        
        
        Log.Println("Initial deck "+deck0.ToString(),log);
        
        thread1 = new Thread(() => EvaluateDeck(game1,deck0.Copy(),deck1.Copy(),Parameters.fight_iterations_per_threads));
        thread2 = new Thread(() => EvaluateDeck(game2,deck0.Copy(),deck2.Copy(),Parameters.fight_iterations_per_threads));
        thread3 = new Thread(() => EvaluateDeck(game3,deck0.Copy(),deck3.Copy(),Parameters.fight_iterations_per_threads));
        thread4 = new Thread(() => EvaluateDeck(game4,deck0.Copy(),deck4.Copy(),Parameters.fight_iterations_per_threads));
                
        thread1.Start();
        thread2.Start();
        thread3.Start();
        thread4.Start();
        
        thread1.Join();
        thread2.Join();
        thread3.Join();
        thread4.Join();

        //EvaluateDeck(game1, deck0.Copy(), deck1.Copy(), Parameters.fight_iterations_per_threads);
        
        initWinRate = (game1.player1_winrate + game2.player1_winrate + game3.player1_winrate + game4.player1_winrate) / Parameters.nb_of_threads;
        currentWinRate = initWinRate;
        Log.Println("Initial win rate:" + initWinRate,true);
        Log.Println("Player 1 wins:" + (game1.player1_wins + game2.player1_wins + game3.player1_wins + game4.player1_wins),true);

        for (int k = 0; k < Parameters.training_iterations; k++)
        {
            for (int i = 0; i < Parameters.cardSwap_iterations; i++)
            {
                Deck baseDeck = deck0.Copy();
                Card card = deck0.cards[rand.Next(0, deck0.cards.Count)];
                deck0.cards.Remove(card);


                Card newCard;
                do
                {
                    newCard = setList.GetRandomCard();
                } while (deck0.cards.Count(x => x == newCard) < 2);


                deck0.cards.Add(newCard);
                
                
                thread1 = new Thread(() => EvaluateDeck(game1,deck0.Copy(),deck1.Copy(),Parameters.fight_iterations_per_threads));
                thread2 = new Thread(() => EvaluateDeck(game2,deck0.Copy(),deck2.Copy(),Parameters.fight_iterations_per_threads));
                thread3 = new Thread(() => EvaluateDeck(game3,deck0.Copy(),deck3.Copy(),Parameters.fight_iterations_per_threads));
                thread4 = new Thread(() => EvaluateDeck(game4,deck0.Copy(),deck4.Copy(),Parameters.fight_iterations_per_threads));
                
                thread1.Start();
                thread2.Start();
                thread3.Start();
                thread4.Start();
                
                thread1.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();

                //EvaluateDeck(game1, deck0.Copy(), deck1.Copy(), Parameters.fight_iterations_per_threads);

                winRate =
                    (game1.player1_winrate + game2.player1_winrate + game3.player1_winrate + game4.player1_winrate) / Parameters.nb_of_threads;;
                
                
                //game.Evaluate(deck1, deck2);
                //winRate = game.player1_winrate;
                Log.Println("New win rate:" + winRate, log);

                if (winRate <= currentWinRate)
                {
                    deck0 = baseDeck;
                    setList.UpdateCardImpact(card, true);
                    setList.UpdateCardImpact(newCard, false);
                }
                else
                {
                    currentWinRate = winRate;
                    setList.UpdateCardImpact(card, false);
                    setList.UpdateCardImpact(newCard, true);
                }

            }

            if (useRandomDeck)
            {
                deck1 = setList.GetRandomDeck();
                deck2 = setList.GetRandomDeck();
                deck3 = setList.GetRandomDeck();
                deck4 = setList.GetRandomDeck();
            }

        }

        Log.Println("Final Win rate:" + winRate,true);
        Log.Println("Final deck :" + deck0.ToString(),true);
    }

    private void EvaluateDeck(Game game,Deck deck1,Deck deck2,int iterations)
    {
        game.Evaluate(deck1, deck2, iterations);
        
    }
    
}