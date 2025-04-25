using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Character_User : BaseCharacter
{
    private readonly static uint COUNT_MANA_MAX = 5;

    protected List<Summon> m_listSummon = new List<Summon>();

    public override void Init(uint charID)
    {
        TableData.TableData_User dataChar = ProjectManager.Instance.Table.User.GetData(charID);
        base.Stat = new CharacterStat(dataChar.hp, COUNT_MANA_MAX, 0); //TODO Mana
        base.m_nCurrHP = base.Stat.HP;
        base.m_nCurrMana = base.Stat.MP;

        base.Init(charID);
        base.m_renderer.sprite = ProjectManager.Instance.Table.User.GetSprite(base.CharID);
        base.m_renderer.flipX = true;

        //소환수 저장
        this.m_listSummon.Clear();
        List<UserData_User.SummonData> listSummon = ProjectManager.Instance.UserData.User.GetSummonDataByList();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            this.m_listSummon.Add(new Summon(listSummon[i].SummonID, base.getStatus));
        }

        //소환수 스킬이 내 스킬
        base.m_listSkill.Clear();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            base.m_listSkill.Add(this.m_listSummon[i].Skill);
        }
        
        //UI 세팅
        ProjectManager.Instance.BattleScene?.HUD.InitSummonUI(this.m_listSummon);
        ProjectManager.Instance.BattleScene?.HUD.InitManaUI(this.m_nCurrMana);
    }

    public bool IsFinishTurn()
    {
        if(this.m_nCurrMana == 0) return true;
        if(base.getStatus(TableData.TableStatus.eID.Fainting) != null) return true;

        bool hasUsableSkill = false;
        for(int i = 0; i < this.m_listSummon.Count; i++)
        {
            if(this.m_listSummon[i].Skill.IsUseable(base.m_nCurrMana) == false) continue;

            hasUsableSkill = true;
            break;
        }
        return hasUsableSkill == false;
    }

    /*TODO Delete
    public override void CheckFinishTurn()
    {
        //코스트 확인~
        if(this.IsFinishTurn() == true) return;

        //아직 안끝났다면 스킬 사용할 수 있다고 세팅
        ProjectManager.Instance.BattleScene?.User_SetClickable(true);
    }
    */

    protected override void useCurrSkill()
    {
        base.useCurrSkill();

        //스킬 이펙트
        ProjectManager.Instance.BattleScene?.HUD.ActiveSummonSkill(this.m_listSummon[ProjectManager.Instance.BattleScene.HUD.SelectedSkillIdx].Data.tableID);

        this.UseMana(base.m_currSkill.Cost);
        ProjectManager.Instance.BattleScene?.HUD.RefreshMana(this.m_nCurrMana);
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
        base.m_nCurrMana = base.Stat.MP;
        ProjectManager.Instance.BattleScene?.HUD.RefreshMana(base.m_nCurrMana);

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
                ProjectManager.Instance.BattleScene?.User_AddFriendlyTarget();
            }
            break;

            default:
            {
                ProjectManager.Instance.BattleScene?.Enemy_AddTargetFromAttacker(this, this.m_currSkill.TargetType);
            }
            break;
        }
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.BattleScene?.IsUserTurn == false) return;
        if(ProjectManager.Instance.BattleScene?.IsUserClickable == false) return;
        if(ProjectManager.Instance.Table.Skill.IsFriendlyTarget(base.m_currSkill.Data.tableID) == false) return;

        //타겟 저장
        ProjectManager.Instance.BattleScene?.User_AddTarget(this);
    }
}