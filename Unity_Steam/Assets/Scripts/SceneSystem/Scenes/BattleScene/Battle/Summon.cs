using UnityEngine;

public class Summon
{
    private uint m_summonID = 0;
    public Skill Skill { get; private set; } = null;
    public TableData.TableData_Summon Data { get; private set; } = null;
    protected CharecterStatus m_status = null;

    public Summon(uint summonID)
    {
        this.m_summonID = summonID;

        this.Data = ProjectManager.Instance.Table.Summon.GetData(this.m_summonID);
        this.Skill = new Skill(this.Data.skillID, new CharecterStatus(0, 1));
    }
}