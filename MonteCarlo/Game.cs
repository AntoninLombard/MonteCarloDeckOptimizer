using System.Xml;

namespace MonteCarlo;

public class Game
{
    //Player 1:
    public Player player1 = new Player();
    public int player1_wins { get; private set; }
    public float player1_winrate { get ; private set; }
    public List<CardWrapper> player1_playedCards = new List<CardWrapper>();

    
    //Player 2:
    public Player player2 = new Player();
    public int player2_wins { get; private set; }
    public float player2_winrate { get; private set; }
    public List<CardWrapper> player2_playedCards = new List<CardWrapper>();

    public int turns = 0;
    
    
    //public int iteration = Parameters.training_iterations;



    private bool logFights = false;
    
    
    public int Evaluate(Deck deck1,Deck deck2,int iterations)
    {
        FullReset();
        player1.deck = deck1;
        player2.deck = deck2;
        //Log.Println(deck1.ToString(),logFights);
        //Log.Println(deck2.ToString(),logFights);
        for (int i = 0; i < iterations; i++)
        {
            int winner;
            if (i < iterations/2)
            {
                winner = Play(1);
            }
            else
            {
                winner = Play(2);
            }

            Reset();

            if (winner == 1)
            {
                player1_wins++;
            }
            else
            {
                player2_wins++;
            }
        }

        player1_winrate = GetWinRate(iterations,player1_wins);
        player2_winrate = 1 - player1_winrate;
        return GetBest();
    }

    private int Play(int firstPlayer)
    {
        int currentPlayer = firstPlayer;
        player1.deck.InitAndShuffle();
        player2.deck.InitAndShuffle();
        player1.DrawMutlipleCards(4);
        player2.DrawMutlipleCards(4);
        while (player1.life > 0 && player2.life > 0)
        {
            int damage = 0;
            turns++;
            if (currentPlayer == 1)
            {
                PlayerTurn(player1, currentPlayer, player2, player1_playedCards,player2_playedCards);
            }
            else
            {
                PlayerTurn(player2, currentPlayer, player1, player2_playedCards,player1_playedCards);
            }
            
            Log.Println("Player 1 life :" + player1.life,logFights);
            Log.Println("Player 2 life :" + player2.life,logFights);

            currentPlayer = currentPlayer ==1 ? 2 : 1;
        }

        Log.Println("Player " + (player1.life > 0 ? 1 : 2) + " wins.",logFights);
        return player1.life > 0 ? 1 : 2;
    }

    private int GetBest()
    {
        if (player1_wins < player2_wins)
        {
            return 2;
        }
        return 1;
    }

    private float GetWinRate(int nbGames, int nbWins)
    {
        return (float)nbWins / (float)nbGames;
    }

    private void Reset()
    {
        player1.Reset();
        player2.Reset();
        player1_playedCards.Clear();
        player2_playedCards.Clear();
        turns = 0;
    }

    private void FullReset()
    {
        Reset();
        player1_winrate = 0f;
        player2_winrate = 0f;
        player1_wins = 0;
        player2_wins = 0;
    }


    private void PlayerTurn(Player currentPlayer, int currentPlayerNumber,Player enemy,List<CardWrapper> playerBoard,List<CardWrapper> opponnentBoard)
    {
        int damage = 0;
        
        if(logFights)
            Log.Print("[Player's " + currentPlayerNumber + " hand: ", logFights);
        
        foreach ((Card card ,int priority) in currentPlayer.hand.UnorderedItems)
        { 
            if(logFights)
                Log.Print(card.ToString() +" of priority "+ priority.ToString(),logFights);
        }
        if(logFights)
            Log.Println( " Player's mana:"+ currentPlayer.mana +"]",logFights);
        
        //playerBoard = new List<Card>(playerBoard.Concat(currentPlayer.PlayTurn()));
        //playerBoard.AddRange(currentPlayer.PlayTurn());
        foreach (Card card in currentPlayer.PlayTurn())
        {
            playerBoard.Add(new CardWrapper(card));
        }

        if(logFights && false)
            Log.Println("[Played cards by player " + currentPlayerNumber + " :",logFights);
        
        foreach (var card in playerBoard)
        {
            //damage += card.attack;
            if(logFights && false)
                Log.Print(card.ToString(),logFights);
        }
        
        if(logFights)
            Log.Println("  Player's mana: " + currentPlayer.mana+"]\nThe player "+ (currentPlayerNumber == 1 ? 2 : 1) +" take: " + damage + " damage",logFights);
        
        ResolveFight(enemy, playerBoard, opponnentBoard);
        EndTurn();
    }

    void ResolveFight(Player opponentPlayer, List<CardWrapper> currentBoard,List<CardWrapper> opponentBoard)
    {
        foreach (CardWrapper card in currentBoard.ToList())
        {
            IEnumerable<CardWrapper> possibleOpponent = opponentBoard.Where(x => x.hasTaunt);
            if (possibleOpponent.Count() == 0)
            {
                opponentPlayer.TakeDamage(card.attack);
            }
            else
            {
                CardWrapper enemyCard = possibleOpponent.First();
                int overflow = 0;
                if (card.hasDistortion)
                {
                    possibleOpponent = possibleOpponent.Where(x => x.hasDistortion);
                    
                    if (possibleOpponent.Count() == 0)
                    {
                        opponentPlayer.TakeDamage(card.attack);
                    }
                    else
                    {
                        enemyCard = possibleOpponent.First();
                        overflow = CardFight(card,enemyCard);
                    }
                }
                else
                {
                    overflow = CardFight(card,enemyCard);
                }

                if (card.hasTrample && overflow > 0)
                {
                    opponentPlayer.TakeDamage(card.attack);
                }
                
                if (card.currentLife <= 0)
                {
                    currentBoard.Remove(card);
                }
                if (enemyCard.currentLife <= 0)
                {
                    opponentBoard.Remove(card);
                }
            }
            
        }
        
    }

    //Resolve card fight and return the damage overflow for Trample
    int CardFight(CardWrapper currentCard, CardWrapper enemyCard)
    {
        if (currentCard.hasFirstStrike == enemyCard.hasFirstStrike)
        {
            currentCard.currentLife -= enemyCard.attack;
            enemyCard.currentLife -= currentCard.attack;
        }
        else if(currentCard.hasFirstStrike && !enemyCard.hasFirstStrike)
        {
            enemyCard.currentLife -= currentCard.attack;
            if (enemyCard.currentLife > 0)
            {
                currentCard.currentLife -= enemyCard.attack;
            }
        }
        else
        {
            currentCard.currentLife -= enemyCard.attack;
            if (currentCard.currentLife > 0)
            {
                enemyCard.currentLife -= enemyCard.attack;
            }
        }
        
        if (enemyCard.currentLife <= 0)
        {
            return -enemyCard.currentLife;
        }

        return 0;
    }
    
    void EndTurn()
    {
        foreach (CardWrapper card in player1_playedCards)
        {
            card.Reset();
        }
        foreach (CardWrapper card in player2_playedCards)
        {
            card.Reset();
        }
    }

}