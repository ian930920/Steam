using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ePOPUP_ID
{
    System = 40001,
    System_Timer,
    System_Confirm_Text,
    End
}

public class PopupSystem
{
    private readonly static int DEPTH_TOP = 2000;

    //팝업
    [SerializeField] private Dictionary<int, BasePopup> m_dicPopup = new Dictionary<int, BasePopup>();

    //Sort용
    private List<int> m_listSortingOrder = new List<int>();
    private int CurrSortingOrder { get => this.m_listSortingOrder.Count; }

    public BasePopup CurrPopup { get; private set; } = null;
    public bool IsPopupOpen { get => this.CurrPopup != null; }

    private void createPopup(int nPopupID)
    {
        //GameObject gobjPopup = ResourceManager.Instance.InstantiatePopup(nPopupID, GameSceneManager.Instance.CurrScene.UIRoot);
        GameObject gobjPopup = ProjectManager.Instance.Resource.InstantiatePopup(nPopupID, ProjectManager.Instance.transform);
        BasePopup popup = gobjPopup.GetComponent<BasePopup>();
        popup.InitPopup();
        this.m_dicPopup.Add(nPopupID, popup);
    }

    private bool isContainPopup(int nPopupID)
    {
        return this.m_dicPopup.ContainsKey(nPopupID);
    }

    public BasePopup GetPopup(int nPopupID)
    {
        if(this.isContainPopup(nPopupID) == false) return null;

        return this.m_dicPopup[nPopupID];
    }

    public BasePopup GetPopup(ePOPUP_ID ePopupID)
    {
        return this.GetPopup((int)ePopupID);
    }

    public T GetPopup<T>(ePOPUP_ID ePopupID) where T : class
    {
        return this.GetPopup((int)ePopupID) as T;
    }

    public bool IsOpenPopup(ePOPUP_ID ePopupID)
    {
        int nPopupID = (int)ePopupID;
        if(this.isContainPopup(nPopupID) == false) return false;

        return this.m_dicPopup[nPopupID].IsOpen;
    }

    public void OpenPopup(ePOPUP_ID ePopupID, UnityAction funcClose = null)
    {
        int nPopupID = (int)ePopupID;
        this.openPopup(nPopupID, funcClose);
    }

    public void OpenPopupByTab(ePOPUP_ID ePopupID, int nTab)
    {
        this.OpenAndGetPopup<ScrollPopup>(ePopupID, null)?.ChangeTab(nTab);
    }

    public T OpenAndGetPopup<T>(ePOPUP_ID ePopupID, UnityAction funcClose = null) where T : class
    {
        int nPopupID = (int)ePopupID;
        this.openPopup(nPopupID, funcClose);

        return this.m_dicPopup[nPopupID] as T;
    }

    private void openPopup(int nPopupID, UnityAction funcClose = null)
    {
        if(this.isContainPopup(nPopupID) == false) this.createPopup(nPopupID);

        if(this.m_dicPopup[nPopupID].IsOpen == false && this.m_dicPopup[nPopupID].PopupType == UIManager.eUI_TYPE.Popup_Main)
        {
            //뎁스 저장
            this.CurrPopup = this.m_dicPopup[nPopupID];
            this.addSortingOrder(nPopupID);
        }

        //열기~!
        this.m_dicPopup[nPopupID].OpenPopup(this.CurrSortingOrder, funcClose);
    }

    private void addSortingOrder(int nPopupID)
    {
        this.removeSortingOrder(nPopupID);

        this.m_listSortingOrder.Add(nPopupID);
    }

    private void removeSortingOrder(int nPopupID)
    {
        if(this.m_listSortingOrder.Contains(nPopupID) == false) return;

        this.m_listSortingOrder.Remove(nPopupID);
    }

    public bool AutoClosePopup()
    {
        if(this.CurrSortingOrder == 0) return false;

        //팝업 닫기
        int nPopupID = this.m_listSortingOrder[this.CurrSortingOrder - 1];
        this.GetPopup(nPopupID).OnCloseClicked();

        return true;
    }

    public void CloseAllPopup()
    {
        while(this.AutoClosePopup() == true)
        {
        }
    }

    public void ClosePopup(ePOPUP_ID ePopupID)
    {
        int nPopupID = (int)ePopupID;
        if(this.isContainPopup(nPopupID) == false) return;

        if(this.m_dicPopup[nPopupID].IsOpen == false) return;

        this.m_dicPopup[nPopupID].OnCloseClicked();
    }

    public void RemovePopup(ePOPUP_ID ePopupID)
    {
        int nPopupID = (int)ePopupID;
        if(this.isContainPopup(nPopupID) == false) return;

        this.removeSortingOrder(nPopupID);
        //ProjectManager.Instance.Log($"팝업뎁스 {this.CurrSortingOrder}");

        if(this.m_dicPopup[nPopupID].PopupType == UIManager.eUI_TYPE.Popup_Main)
        {
            this.CurrPopup = null;

            for(int i = this.CurrSortingOrder - 1; i >= 0; --i)
            {
                int nPrevPopupID = this.m_listSortingOrder[i];
                if(this.GetPopup(nPrevPopupID).PopupType == UIManager.eUI_TYPE.Popup_Main)
                {
                    this.CurrPopup = this.GetPopup(nPrevPopupID);
                    this.CurrPopup.transform.SetAsLastSibling();
                    break;
                }
            }
        }
    }

    public void RefreshPopup(ePOPUP_ID ePopupID)
    {
        this.GetPopup(ePopupID)?.RefreshPopup();
    }

    public void RefreshActivePopup()
    {
        Dictionary<int, BasePopup>.Enumerator enumPopup = this.m_dicPopup.GetEnumerator();
        while(enumPopup.MoveNext())
        {
            if(enumPopup.Current.Value.IsOpen == false) continue;

            enumPopup.Current.Value.RefreshPopup();
        }
    }

    #region System
    public void OpenSystemPopup(TableData.TableString.eID eStringID, UnityAction onClosed = null)
    {
        this.OpenSystemPopup(ProjectManager.Instance.Table.GetString(eStringID), onClosed);
    }

    public void OpenSystemPopup(string strDesc, UnityAction onClosed = null)
    {
        this.OpenAndGetPopup<Popup_System>(ePOPUP_ID.System, onClosed).SetDescription(strDesc);
    }

    public void OpenSystemConfirmPopup(TableData.TableString.eID eStringID, UnityAction onConfirm, UnityAction onClose = null)
    {
        this.OpenSystemConfirmPopup(ProjectManager.Instance.Table.GetString((int)eStringID), onConfirm, onClose);
    }

    public void OpenSystemConfirmPopup(string strDesc, UnityAction onConfirm, UnityAction onClose = null)
    {
        this.OpenAndGetPopup<Popup_System_Confirm_Text>(ePOPUP_ID.System_Confirm_Text, onClose).SetConfirm(strDesc, onConfirm);
    }

    public void OpenSystemTimerPopup(TableData.TableString.eID eStringID, UnityAction onClosed = null)
    {
        this.OpenAndGetPopup<Popup_System_Timer>(ePOPUP_ID.System_Timer, onClosed).SetDescription(ProjectManager.Instance.Table.GetString(eStringID));
    }

    public void OpenSystemTimerPopup(string strDesc, UnityAction onClosed = null)
    {
        this.OpenAndGetPopup<Popup_System_Timer>(ePOPUP_ID.System_Timer, onClosed).SetDescription(strDesc);
    }
    #endregion
}
