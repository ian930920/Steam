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
    [SerializeField] private UI_ManaGroup m_uiMana = null;
    [SerializeField] private UI_TurnInfo m_uiTurnInfo = null;

    [Space(5)][Header("루트 정보")]
    [SerializeField] private TextMeshProUGUI m_textRoutName = null;
    [SerializeField] private TextMeshProUGUI m_textStep = null;

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

    public void RefreshStageInfo(uint stageID, uint step)
    {
        this.m_textRoutName.text = $"루트 {stageID}";
        this.m_textStep.text = $"{step}/{5}";
    }

    public void SetMaxMana(int nMaxMana)
    {
        this.m_uiMana.SetMaxMana(nMaxMana);
    }

    public void RefreshMana(int nCurrMana)
    {
        this.m_uiMana.Refresh(nCurrMana);
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
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 가방 팝업");
    }

    public void OnMySummonClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 내 정령 팝업");
    }

    public void OnOptionClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 옵션 팝업");
    }
}