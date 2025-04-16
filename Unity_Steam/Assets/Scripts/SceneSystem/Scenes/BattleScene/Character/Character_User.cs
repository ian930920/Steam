using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Character_User : BaseCharacter
{
    private readonly static uint COUNT_MANA_MAX = 5;

    private List<Summon> m_listSummon = new List<Summon>();

    public override void Init(uint charID)
    {
        TableData.TableData_User dataChar = ProjectManager.Instance.Table.User.GetData(charID);
        base.m_stat = new Character_Stat(dataChar.hp, COUNT_MANA_MAX, 0); //TODO Mana
        base.m_nCurrHP = base.m_stat.HP;
        base.m_nCurrMana = base.m_stat.MP;

        base.Init(charID);
        base.m_renderer.sprite = ProjectManager.Instance.Table.User.GetSprite(base.CharID);
        base.m_renderer.flipX = true;

        //소환수 저장
        this.m_listSummon.Clear();
        List<UserData_User.SummonData> listSummon = ProjectManager.Instance.UserData.Summon.GetSummonDataByList();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            this.m_listSummon.Add(new Summon(listSummon[i].SummonID));
        }

        //소환수 스킬이 내 스킬
        base.m_listSkill.Clear();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            base.m_listSkill.Add(this.m_listSummon[i].Skill);
        }
        
        //UI 세팅
        ProjectManager.Instance.BattleScene.HUD.InitSummonUI(this.m_listSummon);
        ProjectManager.Instance.BattleScene.HUD.InitManaUI(this.m_nCurrMana);
    }

    public bool IsFinishTurn()
    {
        if(this.m_nCurrMana == 0) return true;
        if(this.m_listSummon.Any(summon => summon.Skill.IsUseable(base.m_nCurrMana)) == false) return true;
        
        return false;
    }

    protected override void checkFinishTurn()
    {
        //코스트 확인~
        if(this.IsFinishTurn() == false)
        {
            //스킬 사용할 수 있다고 세팅
            ProjectManager.Instance.BattleScene.SetUserClickable(true);
            return;
        }

        //턴 바꾸기~
        ProjectManager.Instance.BattleScene.ChangeTurn();
    }

    protected override void useCurrSkill()
    {
        base.useCurrSkill();

        this.m_nCurrMana -= base.m_currSkill.Cost;
        ProjectManager.Instance.BattleScene.HUD.RefreshMana(this.m_nCurrMana);
    }

    protected override void death()
    {
        base.death();

        //패배
        ProjectManager.Instance.BattleScene.StageDefeat();
    }

    public override void SetMyTurn()
    {
        base.SetMyTurn();

        ProjectManager.Instance.BattleScene.SetUserClickable(true);

        //마나 채우기
        base.m_nCurrMana = base.m_stat.MP;
        ProjectManager.Instance.BattleScene.HUD.RefreshMana(base.m_nCurrMana);

        //선택돼있는 스킬로
        base.SetCurrSkill(ProjectManager.Instance.BattleScene.HUD.SelectedSkillIdx);
    }

    protected override void setTarget()
    {
        //스킬타입에 따른 타겟 설정
        switch(this.m_currSkill.TargetType)
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
                this.m_listTarget.Add(this);
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            {
                //지금 스킬이 전체 스킬이면 타겟 모두 저장해
                ProjectManager.Instance.BattleScene.User_AddFriendlyTarget();
            }
            break;

            default:
            {
                ProjectManager.Instance.BattleScene.Enemy_AddTargetFromAttacker(this, this.m_currSkill.TargetType);
            }
            break;
        }
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.BattleScene.IsUserTurn == false) return;
        if(ProjectManager.Instance.BattleScene.IsUserClickable == false) return;

        //타겟 저장
        ProjectManager.Instance.BattleScene.User_AddTarget(this);
    }
}