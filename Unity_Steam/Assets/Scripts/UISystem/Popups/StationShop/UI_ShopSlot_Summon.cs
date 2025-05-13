using UnityEngine;

public class UI_ShopSlot_Summon : MonoBehaviour
{
    [SerializeField] private UI_DefaultSummonInfo m_uiSummonInfo = null;
    [SerializeField] private Button_Cost m_btbBuy = null;
    [SerializeField] private GameObject m_gobjSoldOut = null;

    private uint m_summonID = 0;
    private stItem m_stCost;
    
    public void InitSlot(uint suumonID)
    {
        this.m_summonID = suumonID;
        this.m_uiSummonInfo.Refresh(this.m_summonID);
        this.m_stCost = new stItem(TableData.TableItem.eID.Ticket, TableManager.Instance.Summon.GetData(this.m_summonID).buyCost);

        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);

        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        bool isSoldOut = UserDataManager.Instance.Summon.IsContainsSummon(this.m_summonID);
        this.m_gobjSoldOut.SetActive(isSoldOut);
        if(isSoldOut == true) this.m_btbBuy.gameObject.SetActive(false);
        else this.m_btbBuy.InitCost(this.m_stCost);
    }

    public void OnBuyClicked()
    {
        //재화 까고
        UserDataManager.Instance.Inventory.UseItem(this.m_stCost);

        //소환수 저장
        UserDataManager.Instance.Summon.AddSummon(this.m_summonID);

        //소환수 얻었다고 저장
        UIManager.Instance.PopupSystem.OpenRewardSummonPopup(this.m_summonID, null);

        //슬롯 
        this.RefreshSlot();
    }
}