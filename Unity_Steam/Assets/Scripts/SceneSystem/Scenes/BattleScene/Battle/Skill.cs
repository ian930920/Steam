using System;
using System.Collections.Generic;

public class Skill
{
    private static readonly float CRITICAL_DMG = 2.0f;

    public uint SkillID { get; private set; } = 0;
    private TableData.TableData_Skill m_data = null;

    private List<BaseUnit> m_listTarget = new List<BaseUnit>();
    
    private Func<TableData.TableStatus.eID, Status> m_funcGetStatus = null;

    public int RemainTurn { get; private set; } = 0;

    public Skill(uint skillID, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.SkillID = skillID;
        this.m_data = TableManager.Instance.Skill.GetData(this.SkillID);
        this.m_funcGetStatus = funcGetStatus;
        this.RemainTurn = 0;
    }

    public void ResetTarget()
    {
        this.m_listTarget.Clear();
    }

    public void AddTarget(BaseUnit target)
    {
        if(this.isTargetAddable() == false) return;
        if(this.m_listTarget.Contains(target) == true) return;

        this.m_listTarget.Add(target);
    }

    private bool isTargetAddable()
    {
        int nTargetCount = 0;
        switch(TableManager.Instance.Skill.GetTargetType(this.SkillID))
        {
            case TableData.TableSkill.eTARGET_TYPE.Self:
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_Select_1:
            {
                nTargetCount = 1;
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
            {
                nTargetCount = 2;
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            {
                nTargetCount = 5;
            }
            break;
        }

        return nTargetCount > this.m_listTarget.Count;
    }

    public void UpdateTurn()
    {
        if(this.RemainTurn < 1) return;

        this.RemainTurn--;

        //슬롯 갱신
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();
    }

    public void ResetTurn()
    {
        this.RemainTurn = 0;
    }

    private bool isHit(Stat_Additional statAdditional)
    {
        //상태이상 확인
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Dark) != null) return false;

        //스킬 기본 명중률
        float acc = this.m_data.acc;

        //상태이상 적용
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Dark) != null) acc -= 0.5f;

        //룬 적용
        acc += statAdditional.GetStat(Stat_Additional.eTYPE.Acc);

        var randomHit = UnityEngine.Random.Range(0.0f, 1.0f);
        ProjectManager.Instance.Log($"명중률 {acc} 랜덤 값 {randomHit}");
        return acc >= randomHit;
    }

    private bool isCritical(Stat_Additional statAdditional)
    {
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Rage) != null) return true;

        //스킬 기본 치명타율
        float crit = this.m_data.crit;

        //룬 적용
        crit += statAdditional.GetStat(Stat_Additional.eTYPE.Crit);

        return UnityEngine.Random.Range(0.0f, 1.0f) <= crit;
    }

    public stDamage GetResultDamage(Stat_Character statDefault, Stat_Additional statAdditional, bool isMissable = true)
    {
        stDamage stDamage = new stDamage(TableManager.Instance.Skill.GetDefaultDamage(this.SkillID, statDefault, statAdditional));

        var atk = 1.0f;
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Weakened_Atk) != null) atk -= 0.5f;
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Attack_Enhancement) != null) atk += 0.5f;
        stDamage.Value = (int)(stDamage.Value * atk);

        if(stDamage.Value > 0 && this.isCritical(statAdditional) == true)
        {
            stDamage.IsCritical = true;
            stDamage.Value = (int)(stDamage.Value * CRITICAL_DMG);
        }

        if(isMissable == true && this.isHit(statAdditional) == false)
        {
            stDamage.IsCritical = false;
            stDamage.eSkillType = stDamage.eSKILL_TYPE.Miss;
            stDamage.Value = 0;
        }

        return stDamage;
    }

    public void UseSkill(BaseUnit caster, Stat_Character statDefault, Stat_Additional statAdditional)
    {
        switch((TableData.TableSkill.eTYPE)this.m_data.type)
        {
            case TableData.TableSkill.eTYPE.Attack:
            {
                for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                {
                    var target = this.m_listTarget[i];
                    SceneManager.Instance.GetCurrScene<BattleScene>().AddTurnEvent(target);

                    var damage = this.GetResultDamage(statDefault, statAdditional);
                    ObjectPoolManager.Instance.PlayEffect(this.m_data.resID, target.transform.position, () =>
                    {
                        target.Damaged(damage);

                        //명중돼야 상태이상 적용
                        if(damage.eSkillType != stDamage.eSKILL_TYPE.Miss)
                        {
                            for(int j = 0, nStatusMax = this.m_data.listStatusID.Count; j < nStatusMax; ++j)
                            {
                                //TODO Status 명중타입따라서
                                target.AddStatus(this.m_data.listStatusID[j], this.m_data.dur);
                            }
                        }

                        //피해 흡혈 효과
                        if(this.m_data.listStatusID.Contains((uint)TableData.TableStatus.eID.Absorption) == true)
                        {
                            if(damage.eSkillType != stDamage.eSKILL_TYPE.Miss) caster.Heal(new stDamage((int)(damage.Value * 0.5f)));
                        }

                        //시전자에게 암흑디버프가있었다면 지우기
                        caster.UpdateStatus((uint)TableData.TableStatus.eID.Dark);

                        //턴 끝났다고 지우기
                        SceneManager.Instance.GetCurrScene<BattleScene>().RemoveTurnEvent(target);
                    });
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Heal:
            {
                for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                {
                    var target = this.m_listTarget[i];
                    SceneManager.Instance.GetCurrScene<BattleScene>().AddTurnEvent(target);

                    var damage = this.GetResultDamage(statDefault, statAdditional, true);
                    ObjectPoolManager.Instance.PlayEffect(this.m_data.resID, target.transform.position, () =>
                    {
                        target.Heal(damage);

                        //상태이상 적용
                        for(int j = 0, nStatusMax = this.m_data.listStatusID.Count; j < nStatusMax; ++j)
                        {
                            //TODO Status 명중타입따라서
                            target.AddStatus(this.m_data.listStatusID[j], this.m_data.dur);
                        }

                        //턴 끝났다고 지우기
                        SceneManager.Instance.GetCurrScene<BattleScene>().RemoveTurnEvent(target);
                    });
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Buff:
            {
                //TODO 쉴드 ..?
                for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                {
                    var target = this.m_listTarget[i];
                    SceneManager.Instance.GetCurrScene<BattleScene>().AddTurnEvent(target);
                    ObjectPoolManager.Instance.PlayEffect(this.m_data.resID, target.transform.position, () =>
                    {
                        target.AddShield(this.GetResultDamage(statDefault, statAdditional).Value);

                        //상태이상 적용
                        for(int j = 0, nStatusMax = this.m_data.listStatusID.Count; j < nStatusMax; ++j)
                        {
                            //TODO Status 명중타입따라서
                            target.AddStatus(this.m_data.listStatusID[j], this.m_data.dur);
                        }

                        //턴 끝났다고 지우기
                        SceneManager.Instance.GetCurrScene<BattleScene>().RemoveTurnEvent(target);
                    });
                }
            }
            break;

            default:
            {
                SceneManager.Instance.GetCurrScene<BattleScene>().ChangeTurn();
            }
            break;

            /*
            case TableData.TableSkill.eTYPE.Summon:
            {
                //TODO 생성!
                //SceneManager.Instance.GetCurrScene<BattleScene>().User_AddSummonObj(1001, new Stat_Default(stDamage.Value, 0, 0, this.m_data.dur));
                SceneManager.Instance.GetCurrScene<BattleScene>().ChangeTurn();
            }
            break;
            */
        }

        /*
        //상태이상 추가
        if(this.m_data.listStatusID.Count > 0)
        {
            for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
            {
                for(int j = 0, nStatusMax = this.m_data.listStatusID.Count; j < nStatusMax; ++j)
                {
                    this.m_listTarget[i].AddStatus(this.m_data.listStatusID[j], this.m_data.dur);
                }
            }
        }
        */

        //룬의 상태이상도 추가
        if(statAdditional.DicStatus.Count > 0)
        {
            var enumStatus = statAdditional.DicStatus.GetEnumerator();
            while(enumStatus.MoveNext())
            {
                var status = enumStatus.Current.Value;
                switch(status.eTargetType)
                {
                    case stStatus.eTARGET_TYPE.User:
                    {
                        SceneManager.Instance.GetCurrScene<BattleScene>().UnitUser.AddStatus(enumStatus.Current.Key, status.Turn);
                    }
                    break;
                    case stStatus.eTARGET_TYPE.Enemy:
                    {
                        for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                        {
                            this.m_listTarget[i].AddStatus(enumStatus.Current.Key, status.Turn);
                        }
                    }
                    break;
                    case stStatus.eTARGET_TYPE.EnemyAll:
                    {
                        var listTarget = SceneManager.Instance.GetCurrScene<BattleScene>().Enemy_GetTargetList(TableData.TableSkill.eTARGET_TYPE.Enemy_All);
                        for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                        {
                            listTarget[i].AddStatus(enumStatus.Current.Key, status.Turn);
                        }
                    }
                    break;
                }
            }
        }

        //쿨타임 추가
        this.RemainTurn = (int)UnityEngine.Mathf.Clamp(this.m_data.cooldown - statAdditional.GetStat(Stat_Additional.eTYPE.Cooldown), 0, this.m_data.cooldown);
    }
}