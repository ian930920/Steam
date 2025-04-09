using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine.Events;

public class UserDataManager : BaseManager<UserDataManager>
{
    public enum eID
    {
        Account = 0,
        Setting,
        Inventory,
        Time,
        Summon,
    };

    private Dictionary<eID, BaseUserData> m_dicUser = new Dictionary<eID, BaseUserData>()
    {
        { eID.Account, new UserData_Account() },
        { eID.Setting, new UserData_Setting() },
        { eID.Inventory, new UserData_Inventory() },
        { eID.Time, new UserData_Time() },
        { eID.Summon, new UserData_Summon() },
    };

    public UserData_Account Account => this.m_dicUser[eID.Account] as UserData_Account;
    public UserData_Setting Setting => this.m_dicUser[eID.Setting] as UserData_Setting;
    public UserData_Inventory Inventory => this.m_dicUser[eID.Inventory] as UserData_Inventory;
    public UserData_Time Time => this.m_dicUser[eID.Time] as UserData_Time;
    public UserData_Summon Summon => this.m_dicUser[eID.Summon] as UserData_Summon;

    public string BuildVersion { get; set; }
    public string PatchVersion { get; set; }

    protected override void init()
    {
        //this.Account.LoadClientData();
        this.LoadClientData();
    }

    public override void ResetManager()
    {
        Dictionary<eID, BaseUserData>.Enumerator enumUser = this.m_dicUser.GetEnumerator();
        while(enumUser.MoveNext())
        {
            enumUser.Current.Value.Reset();
        }
    }

    public void LoadClientData()
    {
        Dictionary<eID, BaseUserData>.Enumerator enumUser = this.m_dicUser.GetEnumerator();
        while(enumUser.MoveNext())
        {
            enumUser.Current.Value.LoadClientData();
        }
    }

    #region Item
    //Key : ItemID, Value : RefreshFunc
    private Dictionary<uint, UnityEvent<uint>> m_dicItemCountRefreshEvent = new Dictionary<uint, UnityEvent<uint>>();

    #region RefreshEvent
    public void AddItemCountRefreshEvent(uint itemID, UnityAction<uint> funcRefresh)
    {
        if(this.m_dicItemCountRefreshEvent.ContainsKey(itemID) == false)
        {
            this.m_dicItemCountRefreshEvent.Add(itemID, new UnityEvent<uint>());
        }

        this.m_dicItemCountRefreshEvent[itemID].AddListener(funcRefresh);
    }

    private void doItemCountRefreshEvent(stItem stItem)
    {
        if(this.m_dicItemCountRefreshEvent.ContainsKey(stItem.ItemID) == false) return;

        this.m_dicItemCountRefreshEvent[stItem.ItemID].Invoke(stItem.Count);
    }
    #endregion

    public stItem GetInventoryItem(TableData.TableItem.eID eItemID)
    {
        return this.GetInventoryItem((uint)eItemID);
    }

    public stItem GetInventoryItem(uint itemID)
    {
        return this.Inventory.GetItem(itemID);
    }

    private stItem addItem(stItem stItemInfo)
    {
        this.Inventory.AddItem(stItemInfo.ItemID, stItemInfo.Count);

        //UI 업데이트
        this.doItemCountRefreshEvent(this.Inventory.GetItem(stItemInfo.ItemID));
        this.checkNewMark(stItemInfo.ItemID);

        return stItemInfo;
    }

    public stItem AddItem(stItem stItemInfo)
    {
        return this.addItem(stItemInfo);
    }

    public void AddItem(stItem[] arrItemInfo)
    {
        for(int i = 0, nMax = arrItemInfo.Length; i < nMax; ++i)
        {
            this.AddItem(arrItemInfo[i]);
        }
    }

    public bool UseItem(stItem stItemInfo)
    {
        return this.UseItem(stItemInfo.ItemID, stItemInfo.Count);
    }

    public bool UseItem(uint itemID, uint nUseCount)
    {
        //없는 아이템이면 못씀
        if(this.Inventory.IsContainsItem(itemID) == false)
        {
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("아이템 없다!");
            return false;
        }

        //갖고 있는 것보다 적으면 못씀
        if(this.Inventory.IsUseable(itemID, nUseCount) == false)
        {
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("아이템 모자라다!");
            return false;
        }

        //아이템 사용
        this.Inventory.UseItem(itemID, nUseCount);

        //UI 업데이트
        this.doItemCountRefreshEvent(this.Inventory.GetItem(itemID));
        this.checkNewMark(itemID);

        return true;
    }

    public bool IsUseableItem(stItem stItemInfo)
    {
        return this.Inventory.IsUseable(stItemInfo.ItemID, stItemInfo.Count);
    }

    public bool CheckUseableItem(stItem stItemInfo)
    {
        if(this.IsUseableItem(stItemInfo) == true) return true;

        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup($"{stItemInfo.ItemID} 부족 +STR");
        return false;
    }

    public bool IsContainsItem(uint itemID)
    {
        return this.Inventory.IsContainsItem(itemID);
    }

    public void RefreshNewMark()
    {
        for(uint i = (int)TableData.TableItem.eID.Gold, nMax = (int)TableData.TableItem.eID.Dia; i <= nMax; ++i)
        {
            this.checkNewMark(i);
        }
    }

    private void checkNewMark(uint itemID)
    {
        switch((TableData.TableItem.eID)itemID)
        {
            case TableData.TableItem.eID.Gold:
            case TableData.TableItem.eID.Dia:
            {
                //강화 가능한지 확인
                //EX) UIManager.Instance.HUD?.SetNew(ePOPUP_ID.Upgrade, ProjectManager.Instance.Table.UpgradeAbility.IsUpgradeable());
            }
            break;
        }
    }
    #endregion

    #region Debug
    public void Debug_Cheat()
    {
    }
    #endregion
}