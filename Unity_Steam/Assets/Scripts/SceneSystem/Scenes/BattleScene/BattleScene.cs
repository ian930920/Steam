using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private User m_teamUser = null;
    [SerializeField] private Enemy m_teamEnemy = null;
    [SerializeField] private SummonSkill m_summonSkill = null;

    public bool IsUserTurn { get; private set; }  = true;
    public bool IsUserClickable { get; private set; }  = true;

    public HUD_Battle HUD => base.BaseHUD as HUD_Battle;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        //세팅~
        ProjectManager.Instance.InitInBattleScene();

        //일단 소환수 모두 추가
        Dictionary<uint, TableData.TableData_Summon>.Enumerator enumSummon = ProjectManager.Instance.Table.Summon.GetEnumerator();
        while(enumSummon.MoveNext())
        {
            ProjectManager.Instance.UserData.Summon.AddSummon(enumSummon.Current.Key);
        }

        //스테이지 리셋
        this.resetStage();
    }

#region Stage
    private void resetStage()
    {
        //유저 초기화
        this.m_teamUser.ResetTeam();

        //적 초기화
        this.m_teamEnemy.ResetTeam();

        //스테이지 세팅
        this.InitStage();
        
        //전투 변수 초기화
        this.setTurn(true);
    }

    public void InitStage()
    {
        //유저 세팅
        this.m_teamUser.InitStage();

        //적 세팅
        this.m_teamEnemy.InitStage(3);
    }

    public void StageWin()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemPopup("승리!");

        //스테이지 리셋
        this.resetStage();
    }

    public void StageDefeat()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemPopup("패배!");

        //스테이지 리셋
        this.resetStage();
    }
    #endregion
    public void ChangeTurn()
    {
        StartCoroutine("coChangeTurn");
    }

    private IEnumerator coChangeTurn()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        this.setTurn(!this.IsUserTurn);
    }

    private void setTurn(bool isUserTuren)
    {
        this.IsUserTurn = isUserTuren;
        if(this.IsUserTurn == true)
        {
            this.m_teamUser.TurnStart();
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("유저 턴");
        }
        else
        {
            this.m_teamEnemy.TurnStart();
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("상대 턴");
        }
    }

#region User
    public void User_SetClickable(bool isClickable)
    {
        this.IsUserClickable = isClickable;
    }

    public void User_SelectSkill(int nSkillIdx)
    {
        this.m_teamUser.CharUser.SetCurrSkill(nSkillIdx);
    }

    public void User_AddTarget(BaseCharacter charTarget)
    {
        this.m_teamUser.AddTarget(charTarget);
        this.m_teamUser.UseSkill();
    }

    public void User_AddFriendlyTarget()
    {
        this.m_teamUser.AddFriendlyTarget();
    }

    public void User_AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        this.m_teamUser.AddTargetFromAttacker(charAttacker, eTarget);
    }

    public void User_AddSummonObj(uint summonID, CharacterStat stat)
    {
        this.m_teamUser.AddSummonObject(summonID, stat);
    }

    public void User_RemoveSummonObj(Character_SummonObj summonObj)
    {
        this.m_teamUser.RemoveSummonObject(summonObj);
    }
#endregion

#region Enemy
    public void Enemy_NextAttack()
    {
        this.m_teamEnemy.CheckTurnFinish();
    }

    public void Enemy_AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        this.m_teamEnemy.AddTargetFromAttacker(charAttacker, eTarget);
    }

    public void Enemy_RemoveChar(Character_Enemy charEnemy)
    {
        this.m_teamEnemy.RemoveChar(charEnemy);
    }
#endregion

    public void ActiveSummonSkill(uint summonID)
    {
        this.m_summonSkill.Init(summonID);
    }
}