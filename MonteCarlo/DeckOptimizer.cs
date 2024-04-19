namespace MonteCarlo;

public class DeckOptimizer
{
    private Random rand = new Random((int)DateTime.UtcNow.Ticks);
    private int iteration = 500;
    private SetList setList = new SetList();

    private bool log = false;


    public void Init()
    {
        setList.CreateAllCards();
    }
    public void OptimizeDeck()
    {
        Deck deck1 = setList.GetRandomDeck();
        Deck deck2 = setList.GetRandomDeck();
        Game game = new Game();
        
        float initWinRate = 0f;
        float currentWinRate = 0f;
        float winRate = 0f;
        
        
        Log.Println("Initial deck "+deck1.ToString(),log);
        
        int winner = game.Evaluate(deck1,deck2);
        initWinRate = game.player1_winrate;
        currentWinRate = initWinRate;
        Log.Println("Initial win rate:" + initWinRate,true);
        Log.Println("Player 1 wins:" + game.player1_wins,true);

        for (int k = 0; k < 50; k++)
        {
            for (int i = 0; i < iteration; i++)
            {
                Deck baseDeck = deck1.Copy();
                Card card = deck1.cards[rand.Next(0, deck1.cards.Count)];
                deck1.cards.Remove(card);
                deck1.possibleCards[card]--;


                Card newCard;
                do
                {
                    newCard = setList.GetRandomCard();
                } while (deck1.possibleCards[newCard = setList.GetRandomCard()] >= 2);


                deck1.cards.Add(newCard);
                deck1.possibleCards[newCard]++;



                game.Evaluate(deck1, deck2);
                winRate = game.player1_winrate;
                //Log.Println("New win rate:" + winRate, false);

                if (winRate <= currentWinRate)
                {
                    deck1 = baseDeck;
                }
                else
                {
                    currentWinRate = winRate;
                }

            }

            deck2 = setList.GetRandomDeck();

        }

        Log.Println("Final Win rate:" + winRate,true);
        Log.Println("Final deck :" + deck1.ToString(),true);
    }
    
}