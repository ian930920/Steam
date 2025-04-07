using UnityEngine;

public class Character_User : BaseCharacter
{
    //TODO 소환수 가지고있어야지~

    protected override void checkFinishTurn()
    {
        //TODO 코스트 확인~

        //턴 바꾸기~
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().ChangeTurn();
    }

    public override void Init(uint charID)
    {
        base.Init(charID);

        this.m_renderer.flipX = true;
    }

    protected override void death()
    {
        base.death();

        //패배
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().StageDefeat();
    }
}