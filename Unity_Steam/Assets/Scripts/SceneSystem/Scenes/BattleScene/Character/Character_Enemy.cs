using UnityEngine;

public class Character_Enemy : BaseCharacter
{
    public override void Init(uint charID)
    {
        TableData.TableData_Enemy dataEnemy = ProjectManager.Instance.Table.Enemy.GetData(charID);
        base.Stat = new CharacterStat(dataEnemy.hp, 0, dataEnemy.strength);
        base.m_nCurrHP = base.Stat.HP;

        base.Init(charID);
        base.m_renderer.sprite = ProjectManager.Instance.Table.Enemy.GetSprite(base.CharID);

        base.m_listSkill.Clear();
        for(int i = 0, nMax = dataEnemy.listSkillID.Count; i < nMax; ++i)
        {
            base.m_listSkill.Add(new Skill(dataEnemy.listSkillID[i], 0, base.getStat));
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
            case TableData.TableSkill.eTARGET_TYPE.Self:
            {
                //나 세팅
                this.m_listTarget.Add(this);
            }
            break;

            default:
            {
                ProjectManager.Instance.BattleScene?.User_AddTargetFromAttacker(this, this.m_currSkill.TargetType);
            }
            break;
        }
    }

    protected override void checkFinishTurn()
    {
        //턴 바꾸기~
        ProjectManager.Instance.BattleScene?.Enemy_NextAttack();
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