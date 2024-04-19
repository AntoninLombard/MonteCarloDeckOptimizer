using System.Xml;

namespace MonteCarlo;

public class Game
{
    //Player 1:
    public Player player1 = new Player();
    public int player1_wins { get; private set; }
    public float player1_winrate { get ; private set; }
    public List<Card> player1_playedCards = new List<Card>();

    
    //Player 2:
    public Player player2 = new Player();
    public int player2_wins { get; private set; }
    public float player2_winrate { get; private set; }
    public List<Card> player2_playedCards = new List<Card>();

    public int turns = 0;
    
    
    public int iteration = 700;
    public int nbCardInHandMax;



    private bool logFights = false;
    
    
    public int Evaluate(Deck deck1,Deck deck2)
    {
        FullReset();
        player1.deck = deck1;
        player2.deck = deck2;
        Log.Println(deck1.ToString(),logFights);
        Log.Println(deck2.ToString(),logFights);
        for (int i = 0; i < iteration; i++)
        {
            int winner;
            if (i < iteration/2)
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

        player1_winrate = GetWinRate(iteration,player1_wins);
        player2_winrate = 1 - player1_winrate;
        return GetBest();
    }

    private int Play(int firstPlayer)
    {
        int currentPlayer = firstPlayer;
        player1.deck.InitAndShuffle();
        player2.deck.InitAndShuffle();
        while (player1.life > 0 && player2.life > 0)
        {
            int damage = 0;
            turns++;
            if (currentPlayer == 1)
            {
                PlayerTurn(player1, currentPlayer, player2, player1_playedCards);
            }
            else
            {
                PlayerTurn(player2, currentPlayer, player1, player2_playedCards);
            }
            
            //Log.Println("Player 1 life :" + player1.life,logFights);
            //Log.Println("Player 2 life :" + player2.life,logFights);

            currentPlayer = currentPlayer ==1 ? 2 : 1;
        }

        //Log.Println("Player " + (player1.life > 0 ? 1 : 2) + " wins.",logFights);
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


    private void PlayerTurn(Player currentPlayer, int currentPlayerNumber,Player enemy,List<Card> playerBoard)
    {
        int damage = 0;
        
        //Log.Print("[Player's " + currentPlayerNumber + " hand: ", logFights);
        // foreach ((Card card ,int priority) in currentPlayer.hand.UnorderedItems)
        // {
        //     Log.Print(card.ToString() +" of priority "+ priority.ToString(),logFights);
        // }
        // Log.Println( " Player's mana:"+ currentPlayer.mana +"]",logFights);
        
        playerBoard = new List<Card>(playerBoard.Concat(currentPlayer.PlayTurn()));
        
        //Log.Println("[Played cards by player " + currentPlayerNumber + " :",logFights);
        
        foreach (var card in playerBoard)
        {
            damage += card.attack;
            
            //Log.Print(card.ToString(),logFights);
        }
        
        //Log.Println("  Player's mana: " + currentPlayer.mana+"] The player "+ (currentPlayerNumber == 1 ? 2 : 1) +" take: " + damage + " damage",logFights);
        
        enemy.TakeDamage(damage);
    }

}