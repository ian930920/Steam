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

    private uint m_runeID = 0;
    private stItem m_stCost = new stItem(TableData.TableItem.eID.Ticket, 1);
    
    public void InitSlot(uint runeID)
    {
        this.m_runeID = runeID;

        this.m_imgIcon.sprite = TableManager.Instance.Rune.GetIcon(this.m_runeID);

        this.m_textName.text = TableManager.Instance.Rune.GetString_Title(this.m_runeID);
        this.m_textDesc.text = TableManager.Instance.Rune.GetString_Desc(this.m_runeID);

        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);

        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        bool isSoldOut = UserDataManager.Instance.Inventory.GetRune(this.m_runeID) != null;
        this.m_gobjSoldOut.SetActive(isSoldOut);
        if(isSoldOut == true) this.m_btbBuy.gameObject.SetActive(false);
        else this.m_btbBuy.InitCost(this.m_stCost);
    }

    public void OnBuyClicked()
    {
        //재화 까고
        UserDataManager.Instance.Inventory.UseItem(this.m_stCost);

        //룬 저장
        UserDataManager.Instance.Inventory.AddRune(this.m_runeID);

        //슬롯 
        this.RefreshSlot();
    }
}