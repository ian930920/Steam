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
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO 가방 팝업");
    }

    public void OnMySummonClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.Summon);
    }

    public void OnOptionClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO 옵션 팝업");
    }

    public void OnShopClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO 상점 팝업");
    }

    public void OnRouteClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO 루트 선택 팝업");

        //TODO 루트 선택 팝업

        //TODO Delete
        UserDataManager.Instance.Session.SetSessionType(eSESSION_TYPE.Battle);
        SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Battle);
    }
}