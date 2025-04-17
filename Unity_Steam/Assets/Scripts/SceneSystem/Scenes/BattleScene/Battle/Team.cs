using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public abstract void ResetTeam();
    public abstract void TurnStart();
    protected abstract bool isTurnFinish();
    public abstract void CheckTurnFinish();

    protected void turnFinish()
    {
        ProjectManager.Instance.BattleScene?.ChangeTurn();
    }
    
    public abstract void AddTarget(BaseCharacter charTarget);
    public abstract void AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget);
}