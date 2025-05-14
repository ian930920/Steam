using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Popup_RouteSelect : BasePopup
{
    [SerializeField] private UI_RouteSelectSlot[] m_arrSlot = null;
    [SerializeField] private Button_ChangeState m_csbtnSelect = null;

    private List<TableData.TableData_Route> m_listRoute = new List<TableData.TableData_Route>();

    private int m_nSelectIdx = -1;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.m_nSelectIdx = -1;

        var list = UserDataManager.Instance.Session.ListRoute;
        this.m_listRoute.Clear();
        for(int i = 0, nMax = list.Count; i < nMax; ++i)
        {
            this.m_listRoute.Add(TableManager.Instance.Route.GetData(list[i]));
        }

        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            this.m_arrSlot[i].Init(this.m_listRoute[i].tableID);
            this.m_arrSlot[i].RefreshSelect(false);
        }

        //선택안했으니까 안했다고
        this.m_csbtnSelect.RefreshActive(false);

        return this;
    }

    public void OnSelectSlot(int nIdx)
    {
        if(this.m_csbtnSelect.State == UIManager.eUI_BUTTON_STATE.Inactive) this.m_csbtnSelect.RefreshActive(true);

        if(this.m_nSelectIdx >= 0) this.m_arrSlot[this.m_nSelectIdx].RefreshSelect(false);

        this.m_nSelectIdx = nIdx;
        this.m_arrSlot[this.m_nSelectIdx].RefreshSelect(true);
    }

    public void OnSelectClicked()
    {
        if(this.m_csbtnSelect.State == UIManager.eUI_BUTTON_STATE.Inactive)
        {
            this.m_csbtnSelect.OnClicked();
            return;
        }

        //팝업 닫고
        this.OnCloseClicked();

        //유저 데이터에 루트 저장
        UserDataManager.Instance.Session.SaveRoute(this.m_listRoute[this.m_nSelectIdx].tableID);

        //게임 진행~!
        SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Battle);
    }

    public void OnInactiveClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("루트를 선택하세요");
    }
}