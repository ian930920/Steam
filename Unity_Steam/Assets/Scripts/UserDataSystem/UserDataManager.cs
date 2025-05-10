using System.Collections.Generic;
using UnityEngine.Events;

public class UserDataManager : BaseSingleton<UserDataManager>
{
    public enum eID
    {
        Account = 0,
        Setting,
        Inventory,
        Time,
        Summon,
        Session,
    };

    private Dictionary<eID, BaseUserData> m_dicUser = new Dictionary<eID, BaseUserData>()
    {
        { eID.Account, new UserData_Account() },
        { eID.Setting, new UserData_Setting() },
        { eID.Inventory, new UserData_Inventory() },
        { eID.Time, new UserData_Time() },
        { eID.Summon, new UserData_Summon() },
        { eID.Session, new UserData_Session() },
    };

    public UserData_Account Account => this.m_dicUser[eID.Account] as UserData_Account;
    public UserData_Setting Setting => this.m_dicUser[eID.Setting] as UserData_Setting;
    public UserData_Inventory Inventory => this.m_dicUser[eID.Inventory] as UserData_Inventory;
    public UserData_Time Time => this.m_dicUser[eID.Time] as UserData_Time;
    public UserData_Summon Summon => this.m_dicUser[eID.Summon] as UserData_Summon;
    public UserData_Session Session => this.m_dicUser[eID.Session] as UserData_Session;

    public string BuildVersion { get; set; }
    public string PatchVersion { get; set; }

    public override void Initialize()
    {
        //필수
        if(base.IsInitialized == true) return;

        this.loadClientData();

        base.IsInitialized = true;
    }

    public override void ResetManager()
    {
        Dictionary<eID, BaseUserData>.Enumerator enumUser = this.m_dicUser.GetEnumerator();
        while(enumUser.MoveNext())
        {
            enumUser.Current.Value.Reset();
        }
    }

    private void loadClientData()
    {
        Dictionary<eID, BaseUserData>.Enumerator enumUser = this.m_dicUser.GetEnumerator();
        while(enumUser.MoveNext())
        {
            enumUser.Current.Value.LoadClientData();
        }
    }

    public void StartSession()
    {
        this.Session.StartSession();
        this.Inventory.Reset();
        this.Summon.Reset();
    }

    #region Item
    //Key : ItemID, Value : RefreshFunc
    private Dictionary<uint, UnityEvent<int>> m_dicItemCountRefreshEvent = new Dictionary<uint, UnityEvent<int>>();

    #region RefreshEvent
    public void AddItemCountRefreshEvent(uint itemID, UnityAction<int> funcRefresh)
    {
        if(this.m_dicItemCountRefreshEvent.ContainsKey(itemID) == false)
        {
            this.m_dicItemCountRefreshEvent.Add(itemID, new UnityEvent<int>());
        }

        this.m_dicItemCountRefreshEvent[itemID].AddListener(funcRefresh);
    }

    private void doItemCountRefreshEvent(stItem stItem)
    {
        if(this.m_dicItemCountRefreshEvent.ContainsKey(stItem.ItemID) == false) return;

        this.m_dicItemCountRefreshEvent[stItem.ItemID].Invoke(stItem.Count);
    }
    #endregion
    #endregion

    #region Rune
    public void EquipRune(uint summonID, Item_Rune rune)
    {
        this.Inventory.EquipRune(rune.UniqueRuneID, summonID);
        this.Summon.AddRune(summonID, rune);
    }

    public void UnequipRune(uint summonID, Item_Rune rune)
    {
        this.Summon.RemoveRune(summonID, rune);
        this.Inventory.UnequipRune(rune.UniqueRuneID);
    }

    public void ChangeRune(uint summonID, Item_Rune rune)
    {
        //다른 소환수가 장착중인거 장착 해제하고
        if(rune.SummonID != 0) this.UnequipRune(rune.SummonID, rune);

        //지금 장착중인 제일 앞에 룬 해제
        if(this.Summon.GetSummon(summonID).ListRune.Count > 0)
        {
            var prevRune = this.Summon.GetSummon(summonID).ListRune[0];
            this.UnequipRune(summonID, prevRune);
        }

        //새로 장착
        this.EquipRune(summonID, rune);
    }
    #endregion

    #region Debug
    public void Debug_Cheat()
    {
    }
    #endregion
}