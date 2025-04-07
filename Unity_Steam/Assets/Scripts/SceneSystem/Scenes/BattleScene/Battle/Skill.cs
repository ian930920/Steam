using UnityEngine;

public class Skill
{
    private TableData.TableData_Skill m_data = null;

    public Skill(uint skillID)
    {
        this.m_data = ProjectManager.Instance.Table.Skill.GetData(skillID);
    }

    public ulong GetDamage(ulong nDmg)
    {
        return this.m_data.coe * nDmg;
    }
}