using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Station : BaseHUD
{
    public override void Init()
    {
        base.Init();
    }

    public override void RefreshUI()
    {

    }

    public void OnInventoryClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 가방 팝업");
    }

    public void OnMySummonClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenPopup(ePOPUP_ID.Summon);
    }

    public void OnOptionClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 옵션 팝업");
    }

    public void OnShopClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 상점 팝업");
    }

    public void OnRouteClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 루트 선택 팝업");
    }
}