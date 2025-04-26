using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private User m_teamUser = null;
    [SerializeField] private Enemy m_teamEnemy = null;

    public bool IsUserTurn { get; private set; }  = true;
    public bool IsUserClickable { get; private set; }  = true;

    public HUD_Battle HUD => base.BaseHUD as HUD_Battle;

    //텀용 이벤트
    private List<BaseCharacter> m_listTurnEvent = new List<BaseCharacter>();

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        //세팅~
        ProjectManager.Instance.InitInBattleScene();

        //일단 소환수 모두 추가
        Dictionary<uint, TableData.TableData_Summon>.Enumerator enumSummon = ProjectManager.Instance.Table.Summon.GetEnumerator();
        while(enumSummon.MoveNext())
        {
            ProjectManager.Instance.UserData.User.AddSummon(enumSummon.Current.Key);
        }

        //TODO Delete
        for(int i = 0; i < 3; ++i)
        {
            ProjectManager.Instance.UserData.User.AddRune(1, (uint)(i + 4));
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

        this.m_listTurnEvent.Clear();

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

#region TurnEvent
    public void ChangeTurn()
    {
        //아직 공격중이라면 ㄴㄴ
        if(this.m_listTurnEvent.Count > 0) return;

        if(this.IsUserTurn == true) //유저턴이 라면
        {
            //턴이 안끝났다면
            if(this.m_teamUser.IsTurnFinish() == false)
            {
                //스킬 사용할 수 있다고 세팅
                this.User_SetClickable(true);
                return;
            }
        }
        else //적 턴이라면
        { 
            //턴이 안끝났다면
            if(this.m_teamEnemy.IsTurnFinish() == false)
            {
                //다음 적이 공격
                this.m_teamEnemy.CheckTurnFinish();
                return;
            }
        }

        StartCoroutine("coSetTurn", !this.IsUserTurn);
    }

    private IEnumerator coSetTurn(bool isUserTuren)
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1.0f);

        //턴 바꾸기
        this.setTurn(isUserTuren);
    }

    private void setTurn(bool isUserTuren)
    {
        this.IsUserTurn = isUserTuren;
        if(this.IsUserTurn == true)
        {
            this.m_teamUser.TurnStart();
        }
        else
        {
            this.m_teamEnemy.TurnStart();
        }

        //UI 갱신
        this.HUD.SetTurn(this.IsUserTurn);
    }

    public void AddTurnEvent(BaseCharacter character)
    {
        //이미 있으면 ㄴㄴ
        if(this.m_listTurnEvent.Contains(character) == true) return;

        //추가
        this.m_listTurnEvent.Add(character);
    }

    public void RemoveTurnEvent(BaseCharacter character)
    {
        //없으면 ㄴㄴ
        if(this.m_listTurnEvent.Contains(character) == false) return;

        //추가
        this.m_listTurnEvent.Remove(character);

        this.ChangeTurn();
    }
#endregion

#region User
    public void User_SetClickable(bool isClickable)
    {
        this.IsUserClickable = isClickable;

        if(this.IsUserClickable == true) this.User_SelectSkill(this.HUD.SelectedSkillIdx);
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
    /*TODO Delete
    public void Enemy_NextAttack()
    {
        this.m_teamEnemy.CheckTurnFinish();
    }
    */

    public void Enemy_AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        this.m_teamEnemy.AddTargetFromAttacker(charAttacker, eTarget);
    }

    public void Enemy_RemoveChar(Character_Enemy charEnemy)
    {
        this.m_teamEnemy.RemoveChar(charEnemy);
    }
#endregion
}