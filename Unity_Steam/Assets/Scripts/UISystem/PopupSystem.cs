using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ePOPUP_ID
{
    System = 40001,
    System_Timer,
    System_Confirm,

    SummonSelect = 41001,
    Summon,
    RuneEquip,
    Inventory,
    StationShop,
    RouteSelect,
    Event,
    RewardBox,
    Reward_Item,
    Reward_Summon,
    BattleResult,

    StatusInfo = 42001,
    RuneInfo,
    RouteInfo,
    BattleStart,
    End
}

public class PopupSystem
{
    private readonly static int DEPTH_TOP = 2000;

    //팝업
    [SerializeField] private Dictionary<uint, BasePopup> m_dicPopup = new Dictionary<uint, BasePopup>();

    //Sort용
    private List<uint> m_listSortingOrder = new List<uint>();
    private int CurrSortingOrder { get => this.m_listSortingOrder.Count; }

    public BasePopup CurrPopup { get; private set; } = null;
    public bool IsAnyPopupOpen { get => this.CurrPopup != null; }

    private void createPopup(uint popupID)
    {
        GameObject gobjPopup = ResourceManager.Instance.InstantiatePopup(popupID, ProjectManager.Instance.transform);
        this.m_dicPopup.Add(popupID, gobjPopup.GetComponent<BasePopup>());
        this.m_dicPopup[popupID].InitPopup();
    }

    private bool isContainPopup(uint popupID)
    {
        return this.m_dicPopup.ContainsKey(popupID);
    }

    public BasePopup GetPopup(uint popupID)
    {
        if(this.isContainPopup(popupID) == false) return null;

        return this.m_dicPopup[popupID];
    }

    public BasePopup GetPopup(ePOPUP_ID ePopupID)
    {
        return this.GetPopup((uint)ePopupID);
    }

    public T GetPopup<T>(ePOPUP_ID ePopupID) where T : class
    {
        return this.GetPopup((uint)ePopupID) as T;
    }

    public bool IsOpenPopup(ePOPUP_ID ePopupID)
    {
        uint popupID = (uint)ePopupID;
        if(this.isContainPopup(popupID) == false) return false;

        return this.m_dicPopup[popupID].IsOpen;
    }

    public void OpenPopup(ePOPUP_ID ePopupID, UnityAction funcClose = null)
    {
        uint popupID = (uint)ePopupID;
        this.openPopup(popupID, funcClose);
    }

    public void OpenPopupByTab(ePOPUP_ID ePopupID, int nTab)
    {
        this.OpenAndGetPopup<ScrollPopup>(ePopupID, null)?.ChangeTab(nTab);
    }

    public T OpenAndGetPopup<T>(ePOPUP_ID ePopupID, UnityAction funcClose = null) where T : class
    {
        uint popupID = (uint)ePopupID;
        this.openPopup(popupID, funcClose);

        return this.m_dicPopup[popupID] as T;
    }

    private void openPopup(uint popupID, UnityAction funcClose = null)
    {
        if(this.isContainPopup(popupID) == false) this.createPopup(popupID);

        if(this.m_dicPopup[popupID].IsOpen == false && this.m_dicPopup[popupID].PopupType == UIManager.eUI_TYPE.Popup_Main)
        {
            //뎁스 저장
            this.CurrPopup = this.m_dicPopup[popupID];
            this.addSortingOrder(popupID);
        }

        //열기~!
        this.m_dicPopup[popupID].OpenPopup(this.CurrSortingOrder, funcClose);
    }

    private void addSortingOrder(uint popupID)
    {
        this.removeSortingOrder(popupID);

        this.m_listSortingOrder.Add(popupID);
    }

    private void removeSortingOrder(uint popupID)
    {
        if(this.m_listSortingOrder.Contains(popupID) == false) return;

        this.m_listSortingOrder.Remove(popupID);
    }

    public bool AutoClosePopup()
    {
        if(this.CurrSortingOrder == 0) return false;

        //팝업 닫기
        uint popupID = this.m_listSortingOrder[this.CurrSortingOrder - 1];
        this.GetPopup(popupID).OnCloseClicked();

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
        uint popupID = (uint)ePopupID;
        if(this.isContainPopup(popupID) == false) return;

        if(this.m_dicPopup[popupID].IsOpen == false) return;

        this.m_dicPopup[popupID].OnCloseClicked();
    }

    public void RemovePopup(ePOPUP_ID ePopupID)
    {
        uint popupID = (uint)ePopupID;
        if(this.isContainPopup(popupID) == false) return;

        this.removeSortingOrder(popupID);
        //ProjectManager.Instance.Log($"팝업뎁스 {this.CurrSortingOrder}");

        if(this.m_dicPopup[popupID].PopupType == UIManager.eUI_TYPE.Popup_Main)
        {
            this.CurrPopup = null;

            for(int i = this.CurrSortingOrder - 1; i >= 0; --i)
            {
                uint prevPopupID = this.m_listSortingOrder[i];
                if(this.GetPopup(prevPopupID).PopupType == UIManager.eUI_TYPE.Popup_Main)
                {
                    this.CurrPopup = this.GetPopup(prevPopupID);
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
        Dictionary<uint, BasePopup>.Enumerator enumPopup = this.m_dicPopup.GetEnumerator();
        while(enumPopup.MoveNext())
        {
            if(enumPopup.Current.Value.IsOpen == false) continue;

            enumPopup.Current.Value.RefreshPopup();
        }
    }

    #region System
    public void OpenSystemPopup(TableData.TableString.eID eStringID, UnityAction onClosed = null)
    {
        this.OpenSystemPopup(TableManager.Instance.String.GetString(eStringID), onClosed);
    }

    public void OpenSystemPopup(string strDesc, UnityAction onClosed = null)
    {
        this.OpenAndGetPopup<Popup_System>(ePOPUP_ID.System, onClosed).SetDescription(strDesc);
    }

    public void OpenSystemConfirmPopup(TableData.TableString.eID eStringID, UnityAction onConfirm, UnityAction onClose = null)
    {
        this.OpenSystemConfirmPopup(TableManager.Instance.String.GetString((uint)eStringID), onConfirm, onClose);
    }

    public void OpenSystemConfirmPopup(string strDesc, UnityAction onConfirm, UnityAction onClose = null)
    {
        this.OpenAndGetPopup<Popup_System_Confirm>(ePOPUP_ID.System_Confirm, onClose).SetConfirm(strDesc, onConfirm);
    }

    public void OpenSystemTimerPopup(TableData.TableString.eID eStringID, UnityAction onClosed = null)
    {
        this.OpenAndGetPopup<Popup_System_Timer>(ePOPUP_ID.System_Timer, onClosed).SetDescription(TableManager.Instance.String.GetString(eStringID));
    }

    public void OpenSystemTimerPopup(string strDesc, UnityAction onClosed = null)
    {
        this.OpenAndGetPopup<Popup_System_Timer>(ePOPUP_ID.System_Timer, onClosed).SetDescription(strDesc);
    }
    #endregion

    public void OpenStatusInfoPopup(uint statusID, Vector3 vecPos)
    {
        this.OpenAndGetPopup<Popup_StatusInfo>(ePOPUP_ID.StatusInfo).SetStatusInfo(statusID, vecPos);
    }

    public void OpenRuneInfoPopup(uint runeID, Vector3 vecPos)
    {
        this.OpenAndGetPopup<Popup_RuneInfo>(ePOPUP_ID.RuneInfo).SetRuneInfo(runeID, vecPos);
    }

    public void OpenRuneEquipPopup(uint summonID)
    {
        this.OpenAndGetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).SetSummon(summonID);
    }

    public void OpenEventPopup(uint eventID)
    {
        this.OpenAndGetPopup<Popup_Event>(ePOPUP_ID.Event).SetEvent(eventID);
    }

    public void OpenRewardSummonPopup(uint summonID, UnityAction onClose)
    {
        this.OpenAndGetPopup<Popup_Reward_Summon>(ePOPUP_ID.Reward_Summon).SetSummon(summonID, onClose);
    }

    public void OpenRewardItemPopup(stItem stReward, UnityAction onClose)
    {
        this.OpenAndGetPopup<Popup_Reward_Item>(ePOPUP_ID.Reward_Item).SetItem(stReward, onClose);
    }

    public void OpenBattleResultPopup(Popup_BattleResult.eRESULT eResult)
    {
        this.OpenAndGetPopup<Popup_BattleResult>(ePOPUP_ID.BattleResult).SetResult(eResult);
    }

    public void OpenBattleStartPopup(Popup_BattleStart.eTYPE eType, UnityAction onClosed)
    {
        this.OpenAndGetPopup<Popup_BattleStart>(ePOPUP_ID.BattleStart).SetType(eType, onClosed);
    }
}