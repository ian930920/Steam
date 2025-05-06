using System.Collections;
using System.Collections.Generic;

public class Stat_Character
{
    public enum eTYPE
    {
        HP,
        Mana,
        Strength,
        Shield,
        Turn,
        End,
    }

    private Dictionary<eTYPE, ulong> m_dicStat = new Dictionary<eTYPE, ulong>();

    public Stat_Character()
    {
    }

    public Stat_Character(Stat_Character org)
    {
        this.m_dicStat = new Dictionary<eTYPE, ulong>(org.m_dicStat);
    }

    public void Reset()
    {
        this.m_dicStat.Clear();
    }

    public void SetStat(eTYPE eStatType, ulong value)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) this.m_dicStat.Add(eStatType, 0);

        this.m_dicStat[eStatType] = value;
    }

    public void AddStat(eTYPE eStatType, ulong value)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) this.m_dicStat.Add(eStatType, 0);

        this.m_dicStat[eStatType] += value;
    }

    public void RemoveStat(eTYPE eStatType, ulong value)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) this.m_dicStat.Add(eStatType, 0);

        this.m_dicStat[eStatType] -= value;
    }

    public ulong GetStat(eTYPE eStatType)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) return 0;

        return this.m_dicStat[eStatType];
    }
}

/// <summary>
/// 스킬용 추가 스탯
/// </summary>
public class Stat_Additional
{
    public enum eTYPE
    {
        Coe,
        Acc,
        Crit,
        Cooldown,
        End,
    }

    private Dictionary<eTYPE, float> m_dicStat = new Dictionary<eTYPE, float>();

    //Key : statusID, Value : turn
    public Dictionary<uint, stStatus> DicStatus { get; private set; } = new Dictionary<uint, stStatus>();

    public Stat_Additional()
    {
    }

    public Stat_Additional(Stat_Additional org)
    {
        this.m_dicStat = new Dictionary<eTYPE, float>(org.m_dicStat);
        this.DicStatus = new Dictionary<uint, stStatus>(org.DicStatus);
    }

    public void Reset()
    {
        this.m_dicStat.Clear();
    }

    public void AddStat(eTYPE eStatType, float fValue)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) this.m_dicStat.Add(eStatType, 0);

        this.m_dicStat[eStatType] += fValue;
    }

    public void RemoveStat(eTYPE eStatType, float fValue)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) this.m_dicStat.Add(eStatType, 0);

        this.m_dicStat[eStatType] -= fValue;
    }

    public float GetStat(eTYPE eStatType)
    {
        if(this.m_dicStat.ContainsKey(eStatType) == false) return 0;

        return this.m_dicStat[eStatType];
    }

    public void AddStatus(uint statusID, stStatus status)
    {
        if(this.DicStatus.ContainsKey(statusID) == true) return;

        this.DicStatus.Add(statusID, status);
    }

    public void RemoveStatus(uint statusID)
    {
        if(this.DicStatus.ContainsKey(statusID) == false) return;

        this.DicStatus.Remove(statusID);
    }
}

public struct stDamage
{
    public enum eSKILL_TYPE
    {
        Attack,
        Heal,
        Shield,
        Miss,
    }

    public eSKILL_TYPE eSkillType;
    public ulong Value;
    public bool IsCritical;

    public stDamage(ulong damage)
    {
        this.eSkillType = eSKILL_TYPE.Attack; //기본은 공격
        this.Value = damage;
        this.IsCritical = false;
    }
}

public struct stStatus
{
    public enum eTARGET_TYPE
    {
        User,
        Enemy,
        EnemyAll,
    }

    public eTARGET_TYPE eTargetType;
    public ulong Turn;

    public stStatus(eTARGET_TYPE eTargetType, ulong turn)
    {
        this.eTargetType = eTargetType;
        this.Turn = turn;
    }
}