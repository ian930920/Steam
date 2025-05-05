using System;
using UnityEngine;

public class Summon
{
    public uint SummonID { get; private set; } = 0;

    public Skill Skill { get; private set; } = null;

    private UserData_Summon.SummonData m_data = null;

    public uint RemainTurn => this.Skill.RemainTurn;
    public ulong Cost => this.m_data.StatDefault.GetStat(Stat_Character.eTYPE.Mana);
    public ulong Damage => this.m_data.Damage;

    public Summon(uint summonID, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.SummonID = summonID;
        this.m_data = ProjectManager.Instance.UserData.Summon.GetSummon(this.SummonID);
        this.Skill = new Skill(this.m_data.SkillID, funcGetStatus);
    }

    public bool IsUseable(ulong currMana)
    {
        //쿨타임인지 확인
        if(this.Skill.RemainTurn > 0) return false;

        //마나 따라서~ 체크~
        return this.m_data.StatDefault.GetStat(Stat_Character.eTYPE.Mana) <= currMana;
    }

    public void UseSkill()
    {
        //스킬 사용
        this.Skill.UseSkill(this.m_data.StatDefault, this.m_data.StatAdditional);

        //룬 효과 있으면 사용
        if(ProjectManager.Instance.UserData.Summon.IsEquipRune(this.SummonID, (uint)TableData.TableRune.eID.Comfort) == true) this.doRune(TableData.TableRune.eID.Comfort);
    }

    private void doRune(TableData.TableRune.eID eRuneID)
    {
        var value = ProjectManager.Instance.Table.Rune.GetData((uint)eRuneID).value;
        switch(eRuneID)
        {
            case TableData.TableRune.eID.Comfort:
            {
                //유저 캐릭터 힐~
                ProjectManager.Instance.BattleScene?.User_Heal(new stDamage((ulong)value));
            }
            break;
        }
    }
}