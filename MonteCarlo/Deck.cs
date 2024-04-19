namespace MonteCarlo;

public class Deck
{
    private Random rand = new Random((int)DateTime.UtcNow.Ticks);
    public List<Card> cards { get; private set; }= new List<Card>();
    private Stack<Card> cards_stack = new Stack<Card>();
    public Dictionary<Card, int> possibleCards = new Dictionary<Card, int>();
    public static int nbCards { get; private set; } = 30;


    public Deck(List<Card> cards,Dictionary<Card,int> possibleCards)
    {
        this.cards = cards;
        this.possibleCards = possibleCards;
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
        cards_stack = new Stack<Card>(cards.OrderBy(_ => rand.Next()));
    }

    public override string ToString()
    {
        String str = "DECK [\n";
        foreach (var card in cards)
        {
            str+= card.ToString() + "\n";
        }

        str += "]";
        return str;
    }

    public Deck Copy()
    {
        Deck copy = new Deck(new List<Card>(this.cards),new Dictionary<Card,int>(this.possibleCards));
        copy.InitAndShuffle();
        return copy;
    }
}