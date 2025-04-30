using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Enemy : BaseUnit
{
    protected List<Skill> m_listSkill = new List<Skill>();
    private Stat_Additional m_statAdditional = new Stat_Additional();

    public override void Init(uint charID)
    {
        //이미지
        base.m_renderer.sprite = ProjectManager.Instance.Table.Enemy.GetSprite(charID);

        //기본 스탯 및 스킬 설정
        this.initStat(charID);

        //기본 캐릭터 초기화
        base.Init(charID);
    }

    protected override void initStatusBar()
    {
        //캐릭터 상태바 세팅
        if(base.m_uiStatusBar == null) this.m_uiStatusBar = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<UI_CharacterStatusBar>(TableData.TableObjectPool.eID.UI_Unit_StatusBar);
        base.m_uiStatusBar.Init(Camera.main.WorldToScreenPoint(this.transform.position), this.DefaultStat.GetStat(Stat_Character.eTYPE.HP));
    }

    protected override void initStat(uint charID)
    {
        var dataEnemy = ProjectManager.Instance.Table.Enemy.GetData(charID);
        base.DefaultStat.Reset();
        base.DefaultStat.SetStat(Stat_Character.eTYPE.HP, dataEnemy.hp);
        base.DefaultStat.SetStat(Stat_Character.eTYPE.Strength, dataEnemy.strength);

        base.CurrStat.Reset();
        base.CurrStat.SetStat(Stat_Character.eTYPE.HP, base.DefaultStat.GetStat(Stat_Character.eTYPE.HP));

        this.m_listSkill.Clear();
        for(int i = 0, nMax = dataEnemy.listSkillID.Count; i < nMax; ++i)
        {
            this.m_listSkill.Add(new Skill(dataEnemy.listSkillID[i], this.GetStatus));
        }
    }

    public override void SetMyTurn()
    {
        base.SetMyTurn();

        //랜덤 스킬~
        base.SetCurrSkill(this.m_listSkill[Random.Range(0, this.m_listSkill.Count)]);
        base.CurrSkill.UseSkill(this.DefaultStat, this.m_statAdditional);

        //지금 설정된 스킬 사용
        StartCoroutine("coUseSkill");
    }

    private IEnumerator coUseSkill()
    {
        base.playAnim(eSTATE.Attack);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(0.2f);

        base.CurrSkill.UseSkill(this.DefaultStat, this.m_statAdditional);
    }

    protected override void setTarget()
    {
        //스킬타입에 따른 타겟 설정
        var targetType = ProjectManager.Instance.Table.Skill.GetTargetType(this.CurrSkill.SkillID);
        switch(targetType)
        {
            case TableData.TableSkill.eTARGET_TYPE.Self:
            {
                //나 세팅
                base.CurrSkill.AddTarget(this);
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            {
                this.AddTarget(ProjectManager.Instance.BattleScene.UnitUser);
            }
            break;

            default:
            {
                var listTarget = ProjectManager.Instance.BattleScene?.Enemy_GetTargetList(targetType);
                for(int i = 0, nMax = listTarget.Count; i < nMax; ++i)
                {
                    this.AddTarget(listTarget[i]);
                }
            }
            break;
        }
    }

    protected override void death()
    {
        base.death();

        //지우기
        ProjectManager.Instance.BattleScene?.Enemy_RemoveChar(this);
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.BattleScene?.IsUserTurn == false) return;
        if(ProjectManager.Instance.BattleScene?.IsUserClickable == false) return;

        //타겟 저장
        ProjectManager.Instance.BattleScene?.User_AddTarget(this);
    }
}