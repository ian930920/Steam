using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public abstract void ResetTeam();
    public abstract void TurnStart();
    public abstract bool IsTurnFinish();
    public abstract void CheckTurnFinish();

    protected void turnFinish()
    {
        ProjectManager.Instance.BattleScene?.ChangeTurn();
    }
    
    public abstract void AddTarget(BaseCharacter charTarget);
    public abstract void AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget);
}