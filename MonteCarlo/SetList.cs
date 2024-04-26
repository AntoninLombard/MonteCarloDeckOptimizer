namespace MonteCarlo;

public class SetList
{
    public static Random rand = new Random((int)DateTime.UtcNow.Ticks);
    private List<Card> cards = new List<Card>();
    private Dictionary<Card, int> cardImpact = new Dictionary<Card, int>();
    static public int maxCost { get; private set; } = Parameters.maxManaCost;


    public void CreateAllCards()
    {
        for(int special = 0 ; special < 16; special++)
            for (int attack = 0; attack <= maxCost*2-1; attack++)
            {
                for (int defence = 1; defence <= maxCost*2; defence++)
                {
                    bool hasTaunt = (special & 1) != 0;
                    bool hasDistortion = (special & 2) != 0;
                    bool hasTrample = (special & 4) != 0;
                    bool hasFirstStrike = (special & 8) != 0;
                    if (GetCost(attack,defence,hasTaunt,hasTrample,hasDistortion,hasFirstStrike) <= maxCost)
                    {
                        cards.Add(new Card(attack,defence,hasTaunt,hasTrample,hasDistortion,hasFirstStrike));
                    }
                }
            }

        foreach (Card card in cards)
        {
            cardImpact.Add(card,0);
        }
        Console.WriteLine("Set list: nb cards:" + cards.Count);
    }

    public Card GetRandomCard()
    {
        return cards[rand.Next(0,cards.Count)];
    }

    public Deck GetRandomDeck()
    {
        List<Card> deck_cards = new List<Card>();
        while(deck_cards.Count<Deck.nbCards)
        {
            Card card = GetRandomCard();

            if (cards.Count(x => x == card) < 2)
            {
                deck_cards.Add(card);
            }
        }
        return new Deck(deck_cards);
    }

    public int GetCost(int attack,int defence,bool hasTaunt = false, bool hasTrample = false,bool hasDistortion = false,bool hasFirstStrike = false)
    {
        return (int)MathF.Ceiling((float)(attack + defence) / 2f + (hasTaunt ? 1.5f : 0f) + (hasTrample ? 1f : 0f) +
                                  (hasDistortion ? 1f : 0f) + (hasFirstStrike ? 1f : 0f));
    }


    public void UpdateCardImpact(Card card, bool hasGoodImpact)
    {
        cardImpact[card] += (hasGoodImpact ? 1 : -1);
    }

    public void OptimiseSetList()
    {
        Dictionary<Card, int> bestCards = new Dictionary<Card, int>(cardImpact.OrderByDescending(x => x.Value));
        cards = bestCards.Keys.ToList().GetRange(0,Parameters.nbOptimizedCards);
    }


}