using UnityEngine;

public class Unit_User : BaseUnit
{
    public override void Init(uint charID)
    {
        //이미지
        base.m_renderer.sprite = ProjectManager.Instance.Table.User.GetSprite(charID);

        //기본 스탯 설정
        this.initStat(charID);

        //기본 캐릭터 초기화
        base.Init(charID);
    }

    protected override void initStat(uint charID)
    {
        var dataChar = ProjectManager.Instance.Table.User.GetData(charID);
        base.DefaultStat.Reset();
        base.DefaultStat.SetStat(Stat_Character.eTYPE.HP, dataChar.hp);
        base.DefaultStat.SetStat(Stat_Character.eTYPE.Mana, dataChar.maxMana);

        base.CurrStat.Reset();
        base.CurrStat.SetStat(Stat_Character.eTYPE.HP, base.DefaultStat.GetStat(Stat_Character.eTYPE.HP));
        base.CurrStat.SetStat(Stat_Character.eTYPE.Mana, base.DefaultStat.GetStat(Stat_Character.eTYPE.Mana));
        
        //HUD 세팅
        ProjectManager.Instance.BattleScene?.HUD.SetMaxMana(base.CurrStat.GetStat(Stat_Character.eTYPE.Mana));
    }

    public bool IsFinishTurn()
    {
        //마나 체크
        if(base.CurrStat.GetStat(Stat_Character.eTYPE.Mana) == 0) return true;
        
        //상태이상 체크
        if(base.GetStatus(TableData.TableStatus.eID.Fainting) != null) return true;

        return false;
    }

    protected override void death()
    {
        base.death();

        //패배
        ProjectManager.Instance.BattleScene?.StageDefeat();
    }

    public override void SetMyTurn()
    {
        base.SetMyTurn();

        ProjectManager.Instance.BattleScene?.User_SetClickable(true);

        //마나 채우기
        base.resetMana();
    }

    protected override void setTarget()
    {
        //스킬타입에 따른 타겟 설정
        var targetType = ProjectManager.Instance.Table.Skill.GetTargetType(base.CurrSkill.SkillID);
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
                //TODO
                ProjectManager.Instance.BattleScene?.Enemy_AddTargetFromAttacker(this, targetType);
            }
            break;
        }
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.BattleScene?.IsUserTurn == false) return;
        if(ProjectManager.Instance.BattleScene?.IsUserClickable == false) return;
        if(ProjectManager.Instance.Table.Skill.IsFriendlyTarget(base.CurrSkill.SkillID) == false) return;

        //타겟 저장
        ProjectManager.Instance.BattleScene?.User_AddTarget(this);
    }
}