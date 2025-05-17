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
        SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);
    }

    public void OpenRouteSelect()
    {
        this.m_uiRouteSelect.Open();
    }
}