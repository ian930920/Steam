using UnityEngine;

public class Character_Summon : BaseCharacter
{
    protected override void checkFinishTurn()
    {

    }

    protected override void setTarget()
    {

    }

    public void UpdateTurn()
    {
        //턴 감소
        base.Damaged(new stDamage(1, false));
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.BattleScene.IsUserTurn == false) return;
        if(ProjectManager.Instance.BattleScene.IsUserClickable == false) return;

        //타겟 저장
        ProjectManager.Instance.BattleScene.User_AddTarget(this);
    }
}