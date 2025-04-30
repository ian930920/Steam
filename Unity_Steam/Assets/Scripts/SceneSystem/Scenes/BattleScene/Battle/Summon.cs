using System;
using UnityEngine;

public class Summon
{
    public uint SummonID { get; private set; } = 0;

    public Skill Skill { get; private set; } = null;
    private Stat_Character m_statDefault = new Stat_Character();
    private Stat_Additional m_statAdditional = new Stat_Additional();

    public uint RemainTurn => this.Skill.RemainTurn;
    public ulong Cost => this.m_statDefault.GetStat(Stat_Character.eTYPE.Mana);
    public ulong Damage => this.Skill.GetDefaultDamage(this.m_statDefault, this.m_statAdditional);

    public Summon(uint summonID, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.SummonID = summonID;

        var dataSummon = ProjectManager.Instance.Table.Summon.GetData(this.SummonID);
        this.Skill = new Skill(dataSummon.skillID, funcGetStatus);

        //기본 스탯
        this.m_statDefault.Reset();
        this.m_statDefault.SetStat(Stat_Character.eTYPE.Strength, 1);
        this.m_statDefault.SetStat(Stat_Character.eTYPE.Mana, dataSummon.cost);

        //스탯에 룬 적용
        var listRune = ProjectManager.Instance.UserData.User.GetRuneList(this.SummonID);
        for(int i = 0, nMax = listRune.Count; i < nMax; ++i)
        {
            this.addRuneStat(listRune[i]);
        }
    }

    public bool IsUseable(ulong currMana)
    {
        //쿨타임인지 확인
        if(this.Skill.RemainTurn > 0) return false;

        //마나 따라서~ 체크~
        return this.m_statDefault.GetStat(Stat_Character.eTYPE.Mana) <= currMana;
    }

    public void UseSkill()
    {
        //스킬 사용
        this.Skill.UseSkill(this.m_statDefault, this.m_statAdditional);

        //룬 효과 있으면 사용
        if(ProjectManager.Instance.UserData.User.IsContainsRune(this.SummonID, (uint)TableData.TableRune.eID.Comfort) == true) this.doRune(TableData.TableRune.eID.Comfort);
    }

    private void addRuneStat(uint runeID)
    {
        var value = ProjectManager.Instance.Table.Rune.GetData(runeID).value;
        switch((TableData.TableRune.eID)runeID)
        {
            case TableData.TableRune.eID.Anger:
            {
                this.m_statAdditional.AddStat(Stat_Additional.eTYPE.Coe, value);
            }
            break;

            case TableData.TableRune.eID.Focus:
            {
                this.m_statAdditional.AddStat(Stat_Additional.eTYPE.Acc, value);
            }
            break;

            case TableData.TableRune.eID.Bold:
            {
                this.m_statAdditional.AddStat(Stat_Additional.eTYPE.Crit, value);
            }
            break;

            case TableData.TableRune.eID.Vitality:
            {
                this.m_statAdditional.AddStat(Stat_Additional.eTYPE.Cooldown, value);
            }
            break;

            case TableData.TableRune.eID.Calm:
            {
                var cost = this.m_statDefault.GetStat(Stat_Character.eTYPE.Mana);
                this.m_statDefault.SetStat(Stat_Character.eTYPE.Mana, cost - (ulong)value);
            }
            break;

            case TableData.TableRune.eID.Bonding:
            {
                //정령 소환 시 { value } 턴 동안 내 캐릭터에게 방어력 증가를 부여합니다.
                var statusID = ProjectManager.Instance.Table.Rune.GetData(runeID).statusID;
                this.m_statAdditional.AddStatus(statusID, new stStatus(stStatus.eTARGET_TYPE.User, (ulong)value));
            }
            break;

            case TableData.TableRune.eID.Disgust:
            {
                //정령 소환 시 { value } 턴 동안 모든 적에게 방어력 약화를 부여합니다.
                var statusID = ProjectManager.Instance.Table.Rune.GetData(runeID).statusID;
                this.m_statAdditional.AddStatus(statusID, new stStatus(stStatus.eTARGET_TYPE.EnemyAll, (ulong)value));
            }
            break;

            case TableData.TableRune.eID.Comfort:
            {
                //행동형~
            }
            break;
        }
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