using UnityEngine;
using System.Collections.Generic;
using static UserData_User;

public class Character_User : BaseCharacter
{
    //TODO 소환수 가지고있어야지~
    private List<Summon> m_listSummon = new List<Summon>();

    public override void Init(uint charID)
    {
        TableData.TableData_User dataChar = ProjectManager.Instance.Table.User.GetData(charID);
        base.m_stat = new Character_Stat(dataChar.hp, 0, 0); //TODO Mana
        base.m_nCurrHP = base.m_stat.HP;

        base.Init(charID);
        base.m_renderer.sprite = ProjectManager.Instance.Table.User.GetSprite(base.CharID);
        base.m_renderer.flipX = true;

        base.m_listSkill.Clear();
        //TODO Delete 유저는 스킬없는듯
        //for(int i = 0, nMax = dataChar.listSkillID.Count; i < nMax; ++i)
        //{
        //    base.m_listSkill.Add(new Skill(dataChar.listSkillID[i]));
        //}

        //소환수 저장
        List<SummonData> listSummon = ProjectManager.Instance.UserData.Summon.GetSummonDataByList();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            this.m_listSummon.Add(new Summon(listSummon[i].SummonID));
        }
        //소환수 스킬이 내 스킬
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            base.m_listSkill.Add(this.m_listSummon[i].Skill);
        }
        
        //UI 세팅
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().HUD.InitSummonUI(this.m_listSummon);
    }

    protected override void checkFinishTurn()
    {
        //TODO 코스트 확인~

        //턴 바꾸기~
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().ChangeTurn();
    }

    protected override void death()
    {
        base.death();

        //패배
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().StageDefeat();
    }

    public override void SetMyTurn()
    {
        base.SetMyTurn();

        //TODO 선택돼있는 스킬로
        base.SetCurrSkill(0);
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
                
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
            {
                //TODO 랜덤 세팅
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            {
                //지금 스킬이 전체 스킬이면 타겟 모두 저장해
                ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().AddAllEnemyTarget();
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            {
                //지금 스킬이 전체 스킬이면 타겟 모두 저장해
                ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().AddAllFriendlyTarget();
            }
            break;
        }
    }
}