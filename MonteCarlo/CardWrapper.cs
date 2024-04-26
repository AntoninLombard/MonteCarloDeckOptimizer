namespace MonteCarlo;

public class CardWrapper
{
    public Card card { get; private set; }
    public int currentLife = 0;
    public int attack {
        get { return card.attack; }
        private set {}
    }

    public int defence
    {
        get { return card.defence;}
        private set {}
    }

    public int manaCost
    {
        get { return card.manaCost;}
        private set {}
    }

    public bool hasTaunt
    {
        get { return card.hasTaunt; }
        private set { }
    }

    public bool hasTrample
    {
        get { return card.hasTrample; }
        private set { }
    }

    public bool hasDistortion
    {
        get { return card.hasDistortion; }
        private set { }
    }

    public bool hasFirstStrike
    {
        get { return card.hasFirstStrike; }
        private set { }
    }


    
    
    public CardWrapper(Card card)
    {
        this.card = card;
        this.currentLife = card.defence;
    }

    public void Reset()
    {
        this.currentLife = this.card.defence;
    }

    public override string ToString()
    {
        return card.ToString();
    }
}