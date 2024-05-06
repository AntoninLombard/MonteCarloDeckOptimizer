namespace MonteCarlo;

public class Player
{
    public int life { get; private set; }
    public int mana { get; private set; }
    public int manaMax;
    public Deck? deck;
    public PriorityQueue<Card,int> hand { get; private set; } = new PriorityQueue<Card,int>();
    static private int maxLife = Parameters.maxPlayerLife;

    
    public List<Card> PlayBest()
    {
        List<Card> cards = new List<Card>();
        PriorityQueue<Card, int> unplayableCards = new PriorityQueue<Card, int>();

        while (mana > 0 && hand.Count > 0)
        {
            Card card = hand.Peek();
            if (mana >= card.manaCost)
            {
                hand.Dequeue();
                cards.Add(card);
                mana -= card.manaCost;
            }
            else
            {
                unplayableCards.Enqueue(hand.Dequeue(),-card.manaCost);
            }
        }

        while (unplayableCards.Count > 0)
        {
            Card card = unplayableCards.Dequeue();
            hand.Enqueue(card,-card.manaCost);
        }
        return cards;
    }

    public void DrawCard()
    {
        Card card = deck.DrawCard();
        if (card == null)
        {
            return;
        }
        hand.Enqueue(card,-card.manaCost);
    }

    public void DrawMutlipleCards(int nbCards)
    {
        for (int i = 0; i < nbCards; i++)
        {
            DrawCard();
        }
    }

    public List<Card> PlayTurn()
    {
        manaMax++;
        mana = manaMax;
        DrawCard();
        List<Card> card = PlayBest();
        return card;
    }

    // public void SortHand()
    // {
    //     hand.OrderBy(_ => _.manaCost);
    // }

    public void TakeDamage(int dmg)
    {
        life -= dmg;
    }

    public void Reset()
    {
        hand.Clear();
        manaMax = 0;
        mana = manaMax;
        life = maxLife;
    }
}