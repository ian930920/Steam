using System;
using System.Collections.Generic;

public class Skill
{
    private static readonly float CRITICAL_DMG = 2.0f;

    public uint SkillID { get; private set; } = 0;
    private TableData.TableData_Skill m_data = null;

    private List<BaseUnit> m_listTarget = new List<BaseUnit>();
    
    private Func<TableData.TableStatus.eID, Status> m_funcGetStatus = null;

    public uint RemainTurn { get; private set; } = 0;

    public Skill(uint skillID, Func<TableData.TableStatus.eID, Status> funcGetStatus)
    {
        this.SkillID = skillID;
        this.m_data = ProjectManager.Instance.Table.Skill.GetData(this.SkillID);
        this.m_funcGetStatus = funcGetStatus;
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
        switch(ProjectManager.Instance.Table.Skill.GetTargetType(this.SkillID))
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
                nTargetCount = 3;
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
        ProjectManager.Instance.BattleScene?.HUD.RefreshSummonUI();
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

        return acc >= UnityEngine.Random.Range(0, 1.0f);
    }

    private bool isCritical(Stat_Additional statAdditional)
    {
        if(this.m_funcGetStatus.Invoke(TableData.TableStatus.eID.Rage) != null) return true;

        //스킬 기본 치명타율
        float crit = this.m_data.crit;

        //룬 적용
        crit += statAdditional.GetStat(Stat_Additional.eTYPE.Crit);

        return UnityEngine.Random.Range(0, 1.0f) <= crit;
    }

    public stDamage GetResultDamage(Stat_Character statDefault, Stat_Additional statAdditional)
    {
        stDamage stDamage = new stDamage(ProjectManager.Instance.Table.Skill.GetDefaultDamage(this.SkillID, statDefault, statAdditional));

        if(this.isCritical(statAdditional) == true)
        {
            stDamage.IsCritical = true;
            stDamage.Value = (ulong)(stDamage.Value * CRITICAL_DMG);
        }

        if(this.isHit(statAdditional) == false)
        {
            stDamage.IsCritical = false;
            stDamage.eSkillType = stDamage.eSKILL_TYPE.Miss;
            stDamage.Value = 0;
        }

        return stDamage;
    }

    public void UseSkill(Stat_Character statDefault, Stat_Additional statAdditional)
    {
        switch((TableData.TableSkill.eTYPE)this.m_data.type)
        {
            case TableData.TableSkill.eTYPE.Attack:
            {
                for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                {
                    var target = this.m_listTarget[i];
                    ProjectManager.Instance.BattleScene.AddTurnEvent(target);
                    ProjectManager.Instance.ObjectPool.PlayEffect(this.m_data.resID, target.transform.position, () => target.Damaged(this.GetResultDamage(statDefault, statAdditional)));
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Heal:
            {
                for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                {
                    var target = this.m_listTarget[i];
                    ProjectManager.Instance.BattleScene.AddTurnEvent(target);
                    ProjectManager.Instance.ObjectPool.PlayEffect(this.m_data.resID, target.transform.position, () => target.Heal(this.GetResultDamage(statDefault, statAdditional)));
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Status:
            {
                //TODO 쉴드 ..?
                for(int i = 0, nMax = this.m_listTarget.Count; i < nMax; ++i)
                {
                    var target = this.m_listTarget[i];
                    ProjectManager.Instance.BattleScene.AddTurnEvent(target);
                    ProjectManager.Instance.ObjectPool.PlayEffect(this.m_data.resID, target.transform.position, () => target.AddShield(this.GetResultDamage(statDefault, statAdditional).Value));
                }
            }
            break;

            case TableData.TableSkill.eTYPE.Summon:
            {
                //TODO 생성!
                //ProjectManager.Instance.BattleScene?.User_AddSummonObj(1001, new Stat_Default(stDamage.Value, 0, 0, this.m_data.dur));
                ProjectManager.Instance.BattleScene.ChangeTurn();
            }
            break;
        }

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
                        ProjectManager.Instance.BattleScene?.UnitUser.AddStatus(enumStatus.Current.Key, status.Turn);
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
                        var listTarget = ProjectManager.Instance.BattleScene?.Enemy_GetTargetList(TableData.TableSkill.eTARGET_TYPE.Enemy_All);
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
        this.RemainTurn = this.m_data.cooldown;

        //타겟 다 지우기
        this.m_listTarget.Clear();
    }
}