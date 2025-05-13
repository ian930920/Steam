using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopSlot_Rune : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textName = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    [SerializeField] private Button_Cost m_btbBuy = null;
    [SerializeField] private GameObject m_gobjSoldOut = null;

    private stShop m_shop;
    private stItem m_stCost;
    
    public void InitSlot(stShop shop)
    {
        this.m_shop = shop;

        this.m_imgIcon.sprite = TableManager.Instance.Rune.GetIcon(this.m_shop.ItemID);

        this.m_textName.text = TableManager.Instance.Rune.GetString_Title(this.m_shop.ItemID);
        this.m_textDesc.text = TableManager.Instance.Rune.GetString_Desc(this.m_shop.ItemID);

        this.m_stCost = new stItem(TableData.TableItem.eID.Ticket, TableManager.Instance.Rune.GetData(this.m_shop.ItemID).price);

        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);

        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        bool isSoldOut = this.m_shop.IsSoldOut;
        this.m_gobjSoldOut.SetActive(isSoldOut);
        if(isSoldOut == true) this.m_btbBuy.gameObject.SetActive(false);
        else this.m_btbBuy.InitCost(this.m_stCost);
    }

    public void OnBuyClicked()
    {
        //재화 까고
        UserDataManager.Instance.Inventory.UseItem(this.m_stCost);

        //룬 저장
        stItem reward = new stItem(this.m_shop.ItemID, 1);
        UserDataManager.Instance.Inventory.AddItem(reward);

        //샀다고 저장
        UserDataManager.Instance.Session.SaveShopSoldOut(this.m_shop.ItemID);
        this.m_shop = new stShop(this.m_shop.ItemID, true);

        UIManager.Instance.PopupSystem.OpenRewardItemPopup(reward, null);

        //슬롯 
        this.RefreshSlot();
    }
}