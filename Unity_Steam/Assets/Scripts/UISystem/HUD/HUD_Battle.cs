using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Battle : BaseHUD
{
    [Space(5)][Header("소환수")]
    [SerializeField] private UI_Battle_SummonGroup m_uiSummon = null;
    [SerializeField] private UI_SummonSkill m_uiSummonSkill = null;
    [SerializeField] private UI_Battle_SummonInfo m_uiSummonInfo = null;

    [Space(5)][Header("전투 정보")]
    [SerializeField] private GameObject m_gobjBattle = null;
    [SerializeField] private UI_ManaGroup m_uiMana = null;
    [SerializeField] private UI_TurnInfo m_uiTurnInfo = null;

    [Space(5)][Header("루트 정보")]
    [SerializeField] private TextMeshProUGUI m_textRoutName = null;
    [SerializeField] private TextMeshProUGUI m_textStep = null;
    [SerializeField] private Transform m_transRouteInfo = null;
    [SerializeField] private UI_LevelInfo m_uiLevelInfo = null;

    public int SelectedSummonIdx => this.m_uiSummon.SelectedIdx;

    public override void Init()
    {
        base.Init();

        //시작할때 끄기
        this.m_uiSummonInfo.Inactive();
    }

    public override void RefreshUI()
    {

    }

    public void InitSummonUI(List<Summon> listSummon)
    {
        this.m_uiSummon.Init(listSummon);
    }

    public void RefreshSummonGroupUI()
    {
        this.m_uiSummon.Refresh();
    }

    public void RefreshSummonGroupSlot()
    {
        this.m_uiSummon.RefreshSlot();
    }

    public void RefreshStageInfo(uint routeID, int step)
    {
        this.m_textRoutName.text = TableManager.Instance.Route.GetString(routeID);
        this.m_textStep.text = $"{step + 1}/{5}";

        this.m_uiLevelInfo.Refresh(TableManager.Instance.Route.GetData(routeID).level);
    }

    public void SetMaxMana(int nMaxMana)
    {
        this.m_uiMana.SetMaxMana(nMaxMana);
    }

    public void RefreshMana(int nCurrMana)
    {
        this.m_uiMana.Refresh(nCurrMana);

        //마나 값 바뀌면 다시 세팅
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();
    }

    public void SetActiveBattleUI(bool isActive)
    {
        if(this.m_gobjBattle.activeSelf == isActive) return;

        this.m_gobjBattle.SetActive(isActive);

        if(isActive == false) this.m_uiTurnInfo.ResetUI();
    }

    public float ActiveSummonSkill(uint summonID)
    {
        return this.m_uiSummonSkill.Init(summonID);
    }

    public void SetTurn(bool isPlayerTurn)
    {
        this.m_uiTurnInfo.SetTurn(isPlayerTurn);
    }

    public void OpenSummonInfo(uint summonID)
    {
        this.m_uiSummonInfo.Active(summonID);
    }

    public void CloseSummonInfo()
    {
        this.m_uiSummonInfo.Inactive();
    }

    public void OnInventoryClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.Inventory);
    }

    public void OnMySummonClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.Summon);
    }

    public void OnOptionClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO 옵션 팝업");
    }

    public void OnRouteInfo(bool isActive)
    {
        if(isActive == true) UIManager.Instance.PopupSystem.OpenAndGetPopup<Popup_RouteInfo>(ePOPUP_ID.RouteInfo).SetRouteInfo(UserDataManager.Instance.Session.RouteID, this.m_transRouteInfo.position);
        else UIManager.Instance.PopupSystem.ClosePopup(ePOPUP_ID.RouteInfo);
    }
}