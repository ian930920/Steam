using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private User m_teamUser = null;
    public Unit_User UnitUser => this.m_teamUser.Unit;

    [SerializeField] private Enemy m_teamEnemy = null;

    public bool IsUserTurn { get; private set; }  = true;
    public bool IsUserClickable { get; private set; }  = true;

    public HUD_Battle HUD => base.BaseHUD as HUD_Battle;

    //텀용 이벤트
    private List<BaseUnit> m_listTurnEvent = new List<BaseUnit>();

    public override void OnSceneStart()
    {
        base.OnSceneStart();

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
        this.m_teamEnemy.InitStage(1);
    }

    public void StageWin()
    {
        //우리팀 마무리
        this.m_teamUser.BattleFinish();

        //적팀 마무리
        this.m_teamEnemy.BattleFinish();

        UIManager.Instance.PopupSystem.OpenSystemPopup("승리!", () =>
        {
            //TODO 다음 스탭으로
            //일단 마을로 보내기
            UserDataManager.Instance.Session.SetSessionType(eSESSION_TYPE.Station);
            SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Station);
        });
    }

    public void StageDefeat()
    {
        //적팀 마무리
        this.m_teamEnemy.BattleFinish();

        //세션 끝남 ㅠ
        UserDataManager.Instance.Session.FinishSession();

        UIManager.Instance.PopupSystem.OpenSystemPopup("패배!", () =>
        {
            //타이틀로 돌아가기..
            SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);
        });

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

    public void AddTurnEvent(BaseUnit character)
    {
        //이미 있으면 ㄴㄴ
        if(this.m_listTurnEvent.Contains(character) == true) return;

        //추가
        this.m_listTurnEvent.Add(character);
    }

    public void RemoveTurnEvent(BaseUnit character)
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

        if(this.IsUserClickable == true) this.m_teamUser.SelectSkill();
    }

    public void User_SelectSkill()
    {
        this.m_teamUser.SelectSkill();
    }

    public void User_AddTarget(BaseUnit charTarget)
    {
        this.m_teamUser.AddTarget(charTarget);
        this.m_teamUser.UseSkill();
    }

    public void User_Heal(stDamage stDamage)
    {
        this.m_teamUser.Unit.Heal(stDamage);
    }
#endregion

#region Enemy
    public List<Unit_Enemy> Enemy_GetTargetList(TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        return this.m_teamEnemy.GetTargetList(eTarget);
    }

    public void Enemy_RemoveChar(Unit_Enemy charEnemy)
    {
        this.m_teamEnemy.RemoveChar(charEnemy);
    }
#endregion
}