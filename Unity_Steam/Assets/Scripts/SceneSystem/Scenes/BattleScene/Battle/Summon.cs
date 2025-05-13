using System;

public class Summon
{
    public uint SummonID { get; private set; } = 0;

    public Skill Skill { get; private set; } = null;

    private UserData_Summon.MySummon m_data = null;

    public int RemainTurn => this.Skill.RemainTurn;
    public int Cost => this.m_data.StatDefault.GetStat(Stat_Character.eTYPE.Mana);
    public int Damage => this.m_data.Damage;
    public int Cooldown => this.m_data.Cooldown;

    public Summon(uint summonID, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.SummonID = summonID;
        this.m_data = new UserData_Summon.MySummon(UserDataManager.Instance.Summon.GetSummon(this.SummonID));
        this.Skill = new Skill(this.m_data.SkillID, funcGetStatus);

        //지금 스테이지에 따른 디버프
        if(SceneManager.Instance.GetCurrScene<BattleScene>().IsCurrDebuff(TableData.TableRoute.eDEBUFF.Summon_COE_Debuff))
        {
            this.m_data.StatAdditional.AddStat(Stat_Additional.eTYPE.Coe, -0.1f);
        }
        else if(SceneManager.Instance.GetCurrScene<BattleScene>().IsCurrDebuff(TableData.TableRoute.eDEBUFF.Weakened_Hit_Debuff))
        {
            this.m_data.StatAdditional.AddStat(Stat_Additional.eTYPE.Acc, -0.2f);
        }
        else if(SceneManager.Instance.GetCurrScene<BattleScene>().IsCurrDebuff(TableData.TableRoute.eDEBUFF.Summon_Cost_Debuff))
        {
            this.m_data.StatDefault.AddStat(Stat_Character.eTYPE.Mana, 1);
			this.m_data.StatDefault.SetEffectType(Stat_Character.eTYPE.Mana, TableData.TableStatus.eEFFECT_TYPE.Negative);
        }
    }

    public void ResetTurn()
    {
        this.Skill.ResetTurn();
    }

    public bool IsUseable(int currMana)
    {
        //쿨타임인지 확인
        if(this.Skill.RemainTurn > 0) return false;

        //마나 따라서~ 체크~
        return this.m_data.StatDefault.GetStat(Stat_Character.eTYPE.Mana) <= currMana;
    }

    public void UseSkill()
    {
        //스킬 사용
        this.Skill.UseSkill(SceneManager.Instance.GetCurrScene<BattleScene>().UnitUser, this.m_data.StatDefault, this.m_data.StatAdditional);

        //룬 효과 있으면 사용
        if(UserDataManager.Instance.Summon.IsEquipRune(this.SummonID, (uint)TableData.TableRune.eID.Comfort) == true) this.doRune(TableData.TableRune.eID.Comfort);
    }

    private void doRune(TableData.TableRune.eID eRuneID)
    {
        var value = TableManager.Instance.Rune.GetData((uint)eRuneID).value;
        switch(eRuneID)
        {
            case TableData.TableRune.eID.Comfort:
            {
                //유저 캐릭터 힐~
                SceneManager.Instance.GetCurrScene<BattleScene>().User_Heal(new stDamage((int)value));
            }
            break;
        }
    }

    public TableData.TableStatus.eEFFECT_TYPE GetStatEffectType(Stat_Character.eTYPE eStatType)
    {
        return this.m_data.StatDefault.GetEffectType(eStatType);
    }

    public TableData.TableStatus.eEFFECT_TYPE GetAdditionalStatEffectType(Stat_Additional.eTYPE eStatType)
    {
        return this.m_data.StatAdditional.GetEffectType(eStatType);
    }
}