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
        this.m_data = UserDataManager.Instance.Summon.GetSummon(this.SummonID);
        this.Skill = new Skill(this.m_data.SkillID, funcGetStatus);
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
        this.Skill.UseSkill(this.m_data.StatDefault, this.m_data.StatAdditional);

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