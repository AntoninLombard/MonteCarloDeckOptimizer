namespace MonteCarlo;

public class SetList
{
    private Random rand = new Random((int)DateTime.UtcNow.Ticks);
    private List<Card> cards = new List<Card>();
    static public int maxCost { get; private set; } = 6;


    public void CreateAllCards()
    {
        for (int attack = 0; attack <= maxCost*2-1; attack++)
        {
            for (int defence = 1; defence <= maxCost*2; defence++)
            {
                if (attack + defence <= maxCost*2)
                {
                    cards.Add(new Card(attack,defence));
                }
            }
        }
    }

    public Card GetRandomCard()
    {
        return cards[rand.Next(0,cards.Count)];
    }

    public Deck GetRandomDeck()
    {
        List<Card> deck_cards = new List<Card>();
        Dictionary<Card,int> possibleCards =  new Dictionary<Card,int>();
        foreach (var card in cards)
        {
            possibleCards.Add(card,0);
        }
        while(deck_cards.Count<Deck.nbCards)
        {
            Card card = GetRandomCard();
            int nb = possibleCards[card];
            if (nb < 2)
            {
                deck_cards.Add(card);
                possibleCards[card]++;
            }
        }
        return new Deck(deck_cards,possibleCards);
    }


}