using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public abstract void ResetTeam();
    public abstract void TurnStart();
    public abstract bool IsTurnFinish();
    public abstract void CheckTurnFinish();
    public abstract void BattleFinish();

    protected void turnFinish()
    {
        SceneManager.Instance.GetCurrScene<BattleScene>().ChangeTurn();
    }
    
    public abstract void AddTarget(BaseUnit charTarget);
}