namespace MonteCarlo;

public class Card
{
    public int manaCost
    {
        get { return (attack + defence +1)/2; }
        private set {}
    }

    public int attack { get; private set; }
    public int defence { get; private set; }

    
    public bool hasTaunt { get; private set; }
    public bool hasTrample { get; private set; }
    public bool hasDistortion { get; private set; }
    public bool hasFirstStrike { get; private set; }
    


    public Card(int attack, int defence, bool hasTaunt = false, bool hasTrample = false, bool hasDistortion = false, bool hasFirstStrike = false)
    {
        this.attack = attack;
        this.defence = defence;
    }

    public override string ToString()
    {
        return "{\n\"Cost\":" + manaCost + ",\n\"Attack\":" +attack +",\n\"Defence\":" + defence +",\n\"HasTaunt\":"+ hasTaunt +",\n\"HasTrample\":"+ hasTrample +",\n\"HasDistortion\":"+ hasDistortion +",\n\"HasFirstStrike\":"+ hasFirstStrike +"}";
    }
}