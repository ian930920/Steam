using System;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

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

    private Func<CharacterStat> m_funcGetStat = null;
    private Func<TableData.TableStatus.eID, Status> m_funcGetStatus = null;
    public ulong Cost { get; private set; } = 0;

    public uint RemainTurn { get; private set; } = 0;

    public Skill(uint skillID, ulong cost, Func<CharacterStat> funcGetStat, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.Data = ProjectManager.Instance.Table.Skill.GetData(skillID);
        this.Cost = cost;
        this.m_funcGetStat = funcGetStat;
        this.m_funcGetStatus = funcGetStatus;
        this.RemainTurn = 0;
    }

    public ulong GetDefaultDamage()
    {
        var stat = this.m_funcGetStat.Invoke();
        return (ulong)(this.Data.coe * stat.Strength);
    }

    private stDamage getDamage()
    {
        var stat = this.m_funcGetStat.Invoke();
        stDamage damage = new stDamage();

        float fAcc = 1.0f;
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Weakened_Hit) != null) fAcc *= 0.5f;

        //명중률 확인
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Dark) == null && UnityEngine.Random.Range(0, 1.0f) <= this.Data.acc)
        {
            //치명타
            damage.IsCritical = UnityEngine.Random.Range(0, 1.0f) <= this.Data.crit;
            if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Rage) != null) damage.IsCritical = true;

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
                    var target = listTarget[i];
                    ProjectManager.Instance.BattleScene.AddTurnEvent(target);
                    ProjectManager.Instance.ObjectPool.PlayEffect(this.Data.resID, target.transform.position, () => target.Damaged(this.getDamage()));
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Heal:
            {
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    var target = listTarget[i];
                    ProjectManager.Instance.BattleScene.AddTurnEvent(target);
                    ProjectManager.Instance.ObjectPool.PlayEffect(this.Data.resID, target.transform.position, () => target.Heal(this.getDamage()));
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Status:
            {
                //TODO 쉴드 ..?
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    var target = listTarget[i];
                    ProjectManager.Instance.BattleScene.AddTurnEvent(target);
                    ProjectManager.Instance.ObjectPool.PlayEffect(this.Data.resID, target.transform.position, () => target.AddShield(this.GetDefaultDamage()));
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Summon:
            {
                //TODO 유저만 사용가능한 스킬인지 확인
                //생성!
                ProjectManager.Instance.BattleScene?.User_AddSummonObj(2001, new CharacterStat(this.GetDefaultDamage(), 0, 0, this.Data.dur));
                ProjectManager.Instance.BattleScene.ChangeTurn();
            }
            break;
        }

        //상태이상 추가
        if(this.Data.listStatusID.Count > 0)
        {
            for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
            {
                for(int j = 0, nStatusMax = this.Data.listStatusID.Count; j < nStatusMax; ++j)
                {
                    listTarget[i].AddStatus(this.Data.listStatusID[j], this.Data.dur);
                }
            }
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