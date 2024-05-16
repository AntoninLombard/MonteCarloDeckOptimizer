namespace MonteCarlo;

public class Card
{
    public int manaCost
    {
        get { return (int)MathF.Ceiling((float)(attack + defence)/2f + (hasTaunt? 1.5f :0f) + (hasDistortion? 1f :0f) + (hasTrample? 1f :0f) + (hasFirstStrike? 1f :0f)); }
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
        this.hasTaunt = hasTaunt;
        this.hasTrample = hasTrample;
        this.hasFirstStrike = hasFirstStrike;
        this.hasDistortion = hasDistortion;
    }

    public override string ToString()
    {
        return "{\n\"Cost\":" + manaCost + ",\n\"Attack\":" +attack +",\n\"Defense\":" + defence +",\n\"HasTaunt\":"+ (hasTaunt? "true":"false") +",\n\"HasTrample\":"+ (hasTrample? "true":"false") +",\n\"HasDistortion\":"+ (hasDistortion? "true":"false") +",\n\"HasFirstStrike\":"+ (hasFirstStrike? "true":"false") +"\n}";
    }

    public void ToCSV(StreamWriter writer)
    {
        writer.Write("" +manaCost + ',' + attack + ',' + defence + ',' + (hasTaunt ? 1 : 0) + ',' + (hasTrample ? 1 : 0) + ',' + (hasDistortion ? 1 : 0) + ',' + (hasFirstStrike ? 1 : 0));
    }
}