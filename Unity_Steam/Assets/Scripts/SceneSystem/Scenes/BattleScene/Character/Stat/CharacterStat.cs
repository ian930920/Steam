public class CharacterStat
{
    public ulong HP { get; private set; } = 0;
    public ulong MP { get; private set; } = 0;
    public ulong Strength { get; private set; } = 1;
    public ulong Turn { get; private set; } = 0;

    public CharacterStat()
    {
        this.HP = 0;
        this.MP = 0;
        this.Strength = 1;
        this.Turn = 0;
    }

    public CharacterStat(ulong nHP, ulong nMP, ulong nStrength, ulong nTurn = 0)
    {
        this.HP = nHP;
        this.MP = nMP;
        this.Strength = nStrength;
        this.Turn = nTurn;
    }
}