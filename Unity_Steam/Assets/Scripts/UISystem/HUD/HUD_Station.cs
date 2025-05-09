using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Station : BaseHUD
{
    [SerializeField] private UI_RouteSelect m_uiRouteSelect = null;

    public override void Init()
    {
        base.Init();

        this.m_uiRouteSelect.gameObject.SetActive(false);
    }

    public override void RefreshUI()
    {

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

    public void OpenRouteSelect()
    {
        this.m_uiRouteSelect.Open();
    }
}