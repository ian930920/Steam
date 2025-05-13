using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private SpriteRenderer m_rendererBG = null;

    [SerializeField] private User m_teamUser = null;
    public Unit_User UnitUser => this.m_teamUser.Unit;

    [SerializeField] private Enemy m_teamEnemy = null;

    public bool IsBattleStart { get; private set; }  = false;
    public bool IsUserTurn { get; private set; }  = true;
    public bool IsUserClickable { get; private set; }  = true;

    public HUD_Battle HUD => base.BaseHUD as HUD_Battle;

    //텀용 이벤트
    private List<BaseUnit> m_listTurnEvent = new List<BaseUnit>();

    [SerializeField] private Button_ChangeState m_csbtnChangeTurn = null;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        //스테이지
        this.m_rendererBG.sprite = TableManager.Instance.Route.GetSprite(UserDataManager.Instance.Session.RouteID);

        //유저 세팅
        this.m_teamUser.InitStage();

        //스텝 종류에 따른 진행
        this.startCurrStep();
    }

#region Stage
    private void startCurrStep()
    {
        if(UserDataManager.Instance.Session.Step >= TableData.TableRouteStep.MAX_STEP)
        {
            //역으로 보내기
            UserDataManager.Instance.Session.SetSessionType(eSESSION_TYPE.Station);
            SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Station);
            return;
        }

        //스텝
        this.HUD.RefreshStageInfo(UserDataManager.Instance.Session.RouteID, UserDataManager.Instance.Session.Step);
        this.HUD.SetActiveBattleUI(false);

        //스텝 종류에 따른 진행
        var currStep = UserDataManager.Instance.Session.GetCurrStep();
        switch(currStep)
        {
            case TableData.TableRouteStep.eSTEP_TYPE.Battle:
            case TableData.TableRouteStep.eSTEP_TYPE.Battle_Box:
            case TableData.TableRouteStep.eSTEP_TYPE.Boss:
            {
                //전투 정보 노출
                this.HUD.SetActiveBattleUI(true);

                //전투 시작
                this.BattleStart();
            }
            break;

            case TableData.TableRouteStep.eSTEP_TYPE.Battle_Event:
            {
                uint eventID = TableManager.Instance.Event.GetRandomBattleEvent();

                if(TableData.TableEvent.INVAILD_EVENT == eventID) this.BattleStart(); //모든 이벤트 했다면 전투
                else UIManager.Instance.PopupSystem.OpenEventPopup(eventID); //이벤트 시작
            }
            break;

            case TableData.TableRouteStep.eSTEP_TYPE.Event:
            {
                uint eventID = TableManager.Instance.Event.GetRandomEvent();

                if(TableData.TableEvent.INVAILD_EVENT == eventID) this.BattleStart(); //모든 이벤트 했다면 전투
                else UIManager.Instance.PopupSystem.OpenEventPopup(eventID); //이벤트 시작
            }
            break;

            case TableData.TableRouteStep.eSTEP_TYPE.Box:
            {
                //보물상자 팝업
                UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.RewardBox);
            }
            break;
        }
    }

    public void BattleStart()
    {
        this.IsBattleStart = true;

        //스테이지 세팅
        this.initStage();

        //전투 변수 초기화
        this.m_listTurnEvent.Clear();

        Popup_BattleStart.eTYPE eType = Popup_BattleStart.eTYPE.Normal;
        switch(UserDataManager.Instance.Session.GetCurrStep())
        {
            case TableData.TableRouteStep.eSTEP_TYPE.Boss:
            {
                eType = Popup_BattleStart.eTYPE.Boss;
            }
            break;
        }
        //연출 후 전투시작
        UIManager.Instance.PopupSystem.OpenBattleStartPopup(eType, () => this.setTurn(true));
    }

    private void initStage()
    {
        this.m_teamUser.ResetTeam();

        switch(UserDataManager.Instance.Session.GetCurrStep())
        {
            case TableData.TableRouteStep.eSTEP_TYPE.Battle:
            case TableData.TableRouteStep.eSTEP_TYPE.Battle_Box:
            {
                //적 세팅
                var level = TableManager.Instance.Route.GetData(UserDataManager.Instance.Session.RouteID).level;
                this.m_teamEnemy.InitStage(level);
            }
            break;

            case TableData.TableRouteStep.eSTEP_TYPE.Boss:
            {
                //보스 세팅
                this.m_teamEnemy.InitBoss();
            }
            break;
        }
    }

    public void StageWin()
    {
        this.IsBattleStart = false;

        //우리팀 마무리
        this.m_teamUser.BattleFinish();

        //적팀 마무리
        this.m_teamEnemy.BattleFinish();

        //결과 팝업
        UIManager.Instance.PopupSystem.OpenBattleResultPopup(Popup_BattleResult.eRESULT.Win);
    }

    public void StageDefeat()
    {
        this.IsBattleStart = false;

        //적팀 마무리
        this.m_teamEnemy.BattleFinish();

        //세션 끝남 ㅠ
        UserDataManager.Instance.Session.FinishSession();

        //결과 팝업
        UIManager.Instance.PopupSystem.OpenBattleResultPopup(Popup_BattleResult.eRESULT.Defeat);
    }

    public void BattleFinish()
    {
        switch(UserDataManager.Instance.Session.GetCurrStep())
        {
            case TableData.TableRouteStep.eSTEP_TYPE.Battle_Box:
            {
                //보물상자 팝업
                UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.RewardBox);
            }
            break;

            case TableData.TableRouteStep.eSTEP_TYPE.Battle_Event:
            {
                uint eventID = TableManager.Instance.Event.GetRandomEvent();

                if(TableData.TableEvent.INVAILD_EVENT == eventID) this.NextStep(); //모든 이벤트 했다면 다음꺼
                else UIManager.Instance.PopupSystem.OpenEventPopup(eventID); //이벤트 시작
            }
            break;

            default:
            {
                this.NextStep();
            }
            break;
        }
    }

    public void NextStep()
    {
        StartCoroutine("coNextStep");
    }

    private IEnumerator coNextStep()
    {
        //다음 스텝 진행
        UserDataManager.Instance.Session.NextStep();

        SceneManager.Instance.FadeStart(new UI_SceneFade.stFadeInfo(UI_SceneFade.eFADE_TYPE.In, 1.0f, Color.black, null));

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1.0f);

        this.startCurrStep();

        SceneManager.Instance.FadeStart(new UI_SceneFade.stFadeInfo(UI_SceneFade.eFADE_TYPE.Out, 1.0f, Color.black, null));
    }

    public bool IsCurrDebuff(TableData.TableRoute.eDEBUFF eDebuff)
    {
        return (TableData.TableRoute.eDEBUFF)UserDataManager.Instance.Session.RouteID == eDebuff;
    }
    #endregion

#region TurnEvent
    public void OnChangeTurnClicked()
    {
        if(this.m_csbtnChangeTurn.State == UIManager.eUI_BUTTON_STATE.Inactive) return;

        //스킵
        this.SkipUserTurn();
    }

    public void SkipUserTurn()
    {
        //스킬 사용할 수 없다고 세팅
        this.User_SetClickable(false);

        //적턴으로 바꾸기
        this.setTurn(false);
    }

    public void ChangeTurn()
    {
        //전투 시작안했으면ㄴㄴ
        if(this.IsBattleStart == false) return;

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
        if(this.IsBattleStart == false) return;

        this.IsUserTurn = isUserTuren;
        if(this.IsUserTurn == true) this.m_teamUser.TurnStart();
        else this.m_teamEnemy.TurnStart();

        //UI 갱신
        this.HUD.SetTurn(this.IsUserTurn);
        this.m_csbtnChangeTurn.RefreshActive(this.IsUserTurn == true);
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
    public void User_InitSummon()
    {
        this.m_teamUser.InitSummon();
    }

    public int User_GetSummonDamage(uint summonID)
    {
        return this.m_teamUser.GetSummonDamage(summonID);
    }

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

        //TODO 보상
        if(Random.Range(0, 1.0f) > 0.5f) UserDataManager.Instance.Inventory.AddItem(new stItem(TableData.TableItem.eID.Ticket, Random.Range(1, 5)));
    }
    #endregion
}