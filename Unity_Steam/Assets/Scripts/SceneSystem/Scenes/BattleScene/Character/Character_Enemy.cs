using UnityEngine;

public class Character_Enemy : BaseCharacter
{

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

        //공격 선텍
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().Attack(this);
    }
}