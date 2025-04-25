using System;
using UnityEngine;

public class Summon
{
    private uint m_summonID = 0;
    public Skill Skill { get; private set; } = null;
    public TableData.TableData_Summon Data { get; private set; } = null;
    protected CharacterStat m_stat = null;

    public uint RemainTurn => this.Skill.RemainTurn;

    public Summon(uint summonID, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.m_summonID = summonID;

        this.Data = ProjectManager.Instance.Table.Summon.GetData(this.m_summonID);
        this.Skill = new Skill(this.Data.skillID, this.Data.cost, this.getStat, funcGetStatus);
        this.m_stat = new CharacterStat();
    }

    private CharacterStat getStat()
    {
        //TODO 나중에 여기에서 룬 + 버프 추가 적용해서 return
        return this.m_stat;
    }
}