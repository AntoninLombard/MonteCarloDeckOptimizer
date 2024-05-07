namespace MonteCarlo;

public class Deck
{
    private Random rand = new Random((int)DateTime.UtcNow.Ticks);
    public List<Card> cards { get; private set; }= new List<Card>();
    private Stack<Card> cards_stack = new Stack<Card>();
    public static int nbCards { get; private set; } = Parameters.maxCardNb;


    public Deck(List<Card> cards)
    {
        this.cards = cards;
    }
    public Card DrawCard()
    {
        if (cards_stack.Count == 0)
        {
            return null;
        }
        return cards_stack.Pop();
    }

    public void InitAndShuffle()
    {
        //cards_stack = new Stack<Card>(cards.OrderBy(_ => rand.Next()));
        cards_stack.Clear();
        foreach (Card card in cards.OrderBy(_ => rand.Next()))
        {
            cards_stack.Push(card);
        }
    }

    public override string ToString()
    {
        String str = "{ \"Deck\": [\n";
        for (var index = 0; index < cards.Count; index++)
        {
            var card = cards[index];
            str += card.ToString();
            if(index < (cards.Count -1))
            {
                str += ",\n";
            }
        }

        str += "]\n}";
        return str;
    }

    public Deck Copy()
    {
        Deck copy = new Deck(new List<Card>(this.cards));
        copy.InitAndShuffle();
        return copy;
    }


    public static Deck Fuse(Deck deck1, Deck deck2)
    {
        List<Card> newCards = new List<Card>();

        newCards = new List<Card>(deck1.cards.Concat(deck2.cards).OrderBy(_ => SetList.rand.Next()));
        foreach (var grouping in newCards.GroupBy(x => x))
        {
            if (grouping.Count() > 2)
            {
                newCards.Remove(grouping.Key);
            }
        }

        newCards = newCards.GetRange(0, Deck.nbCards);

        return new Deck(newCards);
    }


    public void ToCSV(StreamWriter writer)
    {
        writer.WriteLine("ManaCost,Attack,Defence,Taunt,Trample,Distortion,FirstStrike");
        foreach(var card in this.cards)
        {
            card.ToCSV(writer);
            writer.WriteLine();
        }
    }
}