using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public abstract void ResetTeam();
    public abstract void TurnStart();
    protected abstract bool isTurnFinish();
    public abstract bool CheckTurnFinish();
    public abstract void BattleFinish();

    public abstract void AddTarget(BaseUnit charTarget);
}