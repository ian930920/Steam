using UnityEngine;

public class Summon
{
    private uint m_summonID = 0;
    public Skill Skill { get; private set; } = null;
    public TableData.TableData_Summon Data { get; private set; } = null;
    protected Character_Stat m_stat = null;

    public Summon(uint summonID)
    {
        this.m_summonID = summonID;

        this.Data = ProjectManager.Instance.Table.Summon.GetData(this.m_summonID);
        this.Skill = new Skill(this.Data.skillID, this.getStat);
        this.m_stat = new Character_Stat();
    }

    private Character_Stat getStat()
    {
        //TODO 나중에 여기에서 룬 + 버프 추가 적용해서 return
        return this.m_stat;
    }
}