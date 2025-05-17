using TableData;
using UnityEngine;

public class Unit_User : BaseUnit
{
    public override void Init(uint charID)
    {
        //이미지
        base.m_renderer.sprite = TableManager.Instance.User.GetSprite(charID);

        //기본 스탯 설정
        this.initStat(charID);

        //기본 캐릭터 초기화
        base.Init(charID);
    }

    protected override void initStatusBar()
    {
        //캐릭터 상태바 세팅
        if(base.m_uiStatusBar == null) this.m_uiStatusBar = ObjectPoolManager.Instance.GetPoolObjectComponent<UI_CharacterStatusBar>(TableData.TableObjectPool.eID.UI_User_StatusBar);
        base.m_uiStatusBar.Init(Camera.main.WorldToScreenPoint(this.transform.position), this.DefaultStat.GetStat(Stat_Character.eTYPE.HP));
        base.m_uiStatusBar.RefreshGauge(this.CurrStat.GetStat(Stat_Character.eTYPE.HP));
    }

    protected override void initStat(uint charID)
    {
        //기본 스탯
		var dataChar = TableManager.Instance.User.GetData((int)TableUser.eID.User);
        base.DefaultStat.Reset();
        base.DefaultStat.SetStat(Stat_Character.eTYPE.HP, dataChar.hp);
        base.DefaultStat.SetStat(Stat_Character.eTYPE.Mana, dataChar.maxMana);

        base.CurrStat = new Stat_Character(UserDataManager.Instance.Session.DefaultStat);
        
        //HUD 세팅
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.SetMaxMana(base.CurrStat.GetStat(Stat_Character.eTYPE.Mana));
    }

    public bool IsFinishTurn()
    {
        //마나 체크
        if(base.CurrStat.GetStat(Stat_Character.eTYPE.Mana) == 0) return true;
        
        //상태이상 체크
        if(base.m_isAttackable == false) return true;

        return false;
    }

    protected override void death()
    {
        base.death();

        //패배
        SceneManager.Instance.GetCurrScene<BattleScene>().StageDefeat();
    }

    public override void SetMyTurn()
    {
        base.SetMyTurn();

        //마나 채우기
        base.resetMana();

        //공격불가라면
        if(base.m_isAttackable == false)
        {
            //다음 턴으로 넘기기
            SceneManager.Instance.GetCurrScene<BattleScene>().SkipUserTurn();
            return;
        }

        SceneManager.Instance.GetCurrScene<BattleScene>().User_SetClickable(true);
    }

    protected override void setTarget()
    {
        //스킬타입에 따른 타겟 설정
        var targetType = TableManager.Instance.Skill.GetTargetType(base.CurrSkill.SkillID);
        switch(targetType)
        {
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_Select_1:
            {
                //직접 세팅
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Self:
            {
                //나 세팅
                this.CurrSkill.AddTarget(this);
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            {
                //이런 스킬 없음
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

    private void OnMouseUp()
    {
        if(SceneManager.Instance.GetCurrScene<BattleScene>().IsUserTurn == false) return;
        if(SceneManager.Instance.GetCurrScene<BattleScene>().IsUserClickable == false) return;
        if(TableManager.Instance.Skill.IsFriendlyTarget(base.CurrSkill.SkillID) == false) return;

        //타겟 저장
        SceneManager.Instance.GetCurrScene<BattleScene>().User_AddTarget(this);
    }
}