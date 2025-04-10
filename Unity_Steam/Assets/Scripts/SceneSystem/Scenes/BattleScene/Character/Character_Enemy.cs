using UnityEngine;

public class Character_Enemy : BaseCharacter
{
    public override void Init(uint charID)
    {
        TableData.TableData_Enemy dataEnemy = ProjectManager.Instance.Table.Enemy.GetData(charID);
        base.m_stat = new Character_Stat(dataEnemy.hp, 0, dataEnemy.strength);
        base.m_nCurrHP = base.m_stat.HP;

        base.Init(charID);
        base.m_renderer.sprite = ProjectManager.Instance.Table.Enemy.GetSprite(base.CharID);

        base.m_listSkill.Clear();
        for(int i = 0, nMax = dataEnemy.listSkillID.Count; i < nMax; ++i)
        {
            base.m_listSkill.Add(new Skill(dataEnemy.listSkillID[i], base.getStat));
        }
    }

    public override void SetMyTurn()
    {
        base.SetMyTurn();

        //랜덤 스킬~
        base.SetCurrSkill(Random.Range(0, base.m_listSkill.Count));

        //스킬따라서 타겟도 정해버리기~
    }

    protected override void setTarget()
    {
        //스킬타입에 따른 타겟 설정
        switch(this.m_currSkill.TargetType)
        {
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_Select_1:
            {
                //일단 유저 캐릭터 세팅
                base.AddTarget(ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().CharUser);
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

    protected override void checkFinishTurn()
    {
        //턴 바꾸기~
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().ChangeTurn();
    }

    protected override void death()
    {
        base.death();

        //지우기
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().RemoveEnemy(this);
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().IsUserTurn == false) return;
        if(ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().IsUserAttackable == false) return;

        //타겟 저장
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().AddTarget(this);

        //유저 스킬 사용
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().CharUser.UseSkill();
    }
}