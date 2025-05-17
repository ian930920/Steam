using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit_Enemy : BaseUnit
{
    protected List<Skill> m_listSkill = new List<Skill>();
    private Stat_Additional m_statAdditional = new Stat_Additional();

    public override void Init(uint charID)
    {
        //이미지
        base.m_renderer.sprite = TableManager.Instance.Enemy.GetSprite(charID);

        //기본 스탯 및 스킬 설정
        this.initStat(charID);

        //기본 캐릭터 초기화
        base.Init(charID);
    }

    protected override void initStatusBar()
    {
        //캐릭터 상태바 세팅
        if(base.m_uiStatusBar == null) this.m_uiStatusBar = ObjectPoolManager.Instance.GetPoolObjectComponent<UI_CharacterStatusBar>(TableData.TableObjectPool.eID.UI_Unit_StatusBar);
        base.m_uiStatusBar.Init(Camera.main.WorldToScreenPoint(this.transform.position), this.DefaultStat.GetStat(Stat_Character.eTYPE.HP));
    }

    protected override void initStat(uint charID)
    {
        var dataEnemy = TableManager.Instance.Enemy.GetData(charID);
        base.DefaultStat.Reset();
        base.DefaultStat.SetStat(Stat_Character.eTYPE.HP, dataEnemy.hp);
        base.DefaultStat.SetStat(Stat_Character.eTYPE.Strength, dataEnemy.strength);

        //지금 스테이지에 따른 디버프
        if(SceneManager.Instance.GetCurrScene<BattleScene>().IsCurrDebuff(TableData.TableRoute.eDEBUFF.Enemy_COE_Buff))
        {
            base.DefaultStat.AddStat(Stat_Character.eTYPE.Strength, (int)(dataEnemy.strength * 0.2f));
        }

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

        //상태이상 업데이트
        base.UpdateStatus();

        //스킬턴 업데이트
        for(int i = 0, nMax = this.m_listSkill.Count; i < nMax; ++i)
        {
            this.m_listSkill[i].UpdateTurn();
        }

        //공격불가라면
        if(base.m_isAttackable == false || base.m_isDead == true)
        {
            //다음 턴으로 넘기기
            SceneManager.Instance.GetCurrScene<BattleScene>().ChangeTurn();
            return;
        }

        //랜덤 스킬~
        base.SetCurrSkill(this.getRandomSkill());

        //지금 설정된 스킬 사용
        StopAllCoroutines();
        StartCoroutine("coUseSkill");
    }

    private Skill getRandomSkill()
    {
        //쓸수있는 스킬 없으면 맨 앞에꺼 사용
        if(this.m_listSkill.Any(skill => skill.RemainTurn == 0) == false) return this.m_listSkill[0];

        var listUsableSkill = this.m_listSkill.Where(skill => skill.RemainTurn == 0).OrderByDescending(skill => TableManager.Instance.Skill.GetData(skill.SkillID).cooldown).ToList();

        //쿨타임 기반
        return listUsableSkill[0];
        //return listUsableSkill.OrderByDescending(skill => TableManager.Instance.Skill.GetData(skill.SkillID).cooldown).First();
    }

    private IEnumerator coUseSkill()
    {
        base.playAnim(eSTATE.Attack);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(0.2f);

        //UIManager.Instance.PopupSystem.OpenSystemTimerPopup(TableManager.Instance.Skill.GetString_Title(base.CurrSkill.SkillID));
        base.CurrSkill.UseSkill(this, this.DefaultStat, this.m_statAdditional);
    }

    protected override void setTarget()
    {
        //스킬타입에 따른 타겟 설정
        var targetType = TableManager.Instance.Skill.GetTargetType(this.CurrSkill.SkillID);
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
                this.AddTarget(SceneManager.Instance.GetCurrScene<BattleScene>().UnitUser);
            }
            break;

            default:
            {
                var listTarget = SceneManager.Instance.GetCurrScene<BattleScene>().Enemy_GetTargetList(targetType);
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
        SceneManager.Instance.GetCurrScene<BattleScene>().Enemy_RemoveChar(this);
    }

    private void OnMouseUp()
    {
        if(SceneManager.Instance.GetCurrScene<BattleScene>().IsUserTurn == false) return;
        if(SceneManager.Instance.GetCurrScene<BattleScene>().IsUserClickable == false) return;

        //타겟 저장
        SceneManager.Instance.GetCurrScene<BattleScene>().User_AddTarget(this);
    }
}