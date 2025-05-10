using Febucci.UI;
using UnityEngine;

public class UI_RouteSelect : MonoBehaviour
{
    [SerializeField] private TypewriterByCharacter m_textDesc = null;

    public void Open()
    {
        //TODO 글자 애니
        this.gameObject.SetActive(true);

        this.m_textDesc.ShowText("이곳은 잃어버린 기억들이 흘러들어\n도착하는 정착역이기도 하지요.");
    }

    public void OnShopClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.StationShop);
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