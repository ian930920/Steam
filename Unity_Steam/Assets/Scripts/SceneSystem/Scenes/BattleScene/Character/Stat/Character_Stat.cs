public class Character_Stat
{
    public ulong HP { get; private set; } = 0;
    public ulong MP { get; private set; } = 0;
    public ulong Strength { get; private set; } = 1;

    public Character_Stat()
    {
        this.HP = 0;
        this.MP = 0;
        this.Strength = 1;
    }

    public Character_Stat(ulong nHP, ulong nMP, ulong nStrength)
    {
        this.HP = nHP;
        this.MP = nMP;
        this.Strength = nStrength;
    }
}