using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public TableData.TableData_Skill Data { get; private set; } = null;
    public TableData.TableSkill.eTARGET_TYPE TargetType => (TableData.TableSkill.eTARGET_TYPE)this.Data.target;
    public int TargetCount
    {
        get
        {
            switch((TableData.TableSkill.eTARGET_TYPE)this.Data.target)
            {
                case TableData.TableSkill.eTARGET_TYPE.Self:
                case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
                case TableData.TableSkill.eTARGET_TYPE.Friendly_Select_1:
                return 1;
                
                case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
                case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
                return 2;

                case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
                case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
                return 3;
            }

            return 1;
        }
    }

    private CharacterStat m_status = null;
    private Func<CharacterStat> m_funcGetStat = null;
    public ulong Cost { get; private set; } = 0;

    public uint RemainTurn { get; private set; } = 0;

    public Skill(uint skillID, ulong cost, Func<CharacterStat> funcGetStat)
    {
        this.Data = ProjectManager.Instance.Table.Skill.GetData(skillID);
        this.Cost = cost;
        this.m_funcGetStat = funcGetStat;
        this.RemainTurn = 0;
    }

    public ulong GetDefaultDamage()
    {
        this.m_status = this.m_funcGetStat.Invoke();
        return (ulong)(this.Data.coe * this.m_status.Strength);
    }

    private stDamage getDamage()
    {
        this.m_status = this.m_funcGetStat.Invoke();
        stDamage damage = new stDamage();

        //명중률 확인
        if(UnityEngine.Random.Range(0, 1.0f) <= this.Data.acc)
        {
            //치명타
            damage.IsCritical = UnityEngine.Random.Range(0, 1.0f) <= this.Data.crit;
            float fCritical = 1.0f;
            if(damage.IsCritical) fCritical = 1.5f; //TODO 치명타 값

            switch((TableData.TableSkill.eTYPE)this.Data.type)
            {
                case TableData.TableSkill.eTYPE.Attack:
                case TableData.TableSkill.eTYPE.Heal:
                {
                    damage.Damage = (ulong)(this.GetDefaultDamage() * fCritical);
                }
                break;

                case TableData.TableSkill.eTYPE.Status:
                break;
                case TableData.TableSkill.eTYPE.Summon:
                break;
            }
        }

        return damage;
    }

    public bool isValiedTatget(BaseCharacter charTarget)
    {
        //TODO 아군 적군 확인

        return true;
    }

    public bool IsUseable(ulong nCurrMana)
    {
        if(this.RemainTurn > 0)
        {
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("쿨타임");
            return false;
        }

        if(this.Cost > nCurrMana)
        {
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("마나 부족");
            return false;
        }

        return true;
    }

    public bool UseSkill(List<BaseCharacter> listTarget)
    {
        switch((TableData.TableSkill.eTYPE)this.Data.type)
        {
            case TableData.TableSkill.eTYPE.Attack:
            {
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    listTarget[i].Damaged(this.getDamage());
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Heal:
            {
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    listTarget[i].Heal(this.getDamage());
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Status:
            break;

            case TableData.TableSkill.eTYPE.Summon:
            {
                //TODO 유저만 사용가능한 스킬인지 확인
                //생성!
                ProjectManager.Instance.BattleScene?.AddUserSummonObj(2001, new CharacterStat(this.GetDefaultDamage(), 0, 0, this.Data.dur));
            }
            break;
        }

        this.RemainTurn = this.Data.cooldown;

        //슬롯 갱신
        ProjectManager.Instance.BattleScene?.HUD.RefreshSummonSlot();

        return true;
    }

    public void UpdateTurn()
    {
        if(this.RemainTurn < 1) return;

        this.RemainTurn--;

        //슬롯 갱신
        ProjectManager.Instance.BattleScene?.HUD.RefreshSummonUI();
    }
}

public struct stDamage
{
    public ulong Damage;
    public bool IsCritical;
    public bool IsHeal;
    public bool IsMiss => this.Damage == 0;

    public stDamage(ulong damage, bool isCritical)
    {
        this.Damage = damage;
        this.IsCritical = isCritical;
        this.IsHeal = false;
    }
}