using UnityEngine;

public class UI_RouteSelect : MonoBehaviour
{
    public void Open()
    {
        //TODO 글자 애니
        this.gameObject.SetActive(true);
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