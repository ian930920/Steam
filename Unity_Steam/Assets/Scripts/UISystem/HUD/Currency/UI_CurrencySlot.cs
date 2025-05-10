using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurrencySlot : MonoBehaviour
{
    [SerializeField] private TableData.TableItem.eID m_eCurrencyID = TableData.TableItem.eID.Ticket;
    private uint ItemID { get => (uint)this.m_eCurrencyID; }
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textValue = null;

    public void InitSlot()
    {
        this.m_imgIcon.sprite = TableManager.Instance.Item.GetIcon(this.ItemID);

        //갱신 이벤트 등록
        UserDataManager.Instance.AddItemCountRefreshEvent(this.ItemID, this.refreshSlot);

        //갱신
        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        this.refreshSlot(UserDataManager.Instance.Inventory.GetItem(this.ItemID).Count);
    }

    private void refreshSlot(int nCount)
    {
        this.m_textValue.text = Utility_UI.GetCommaNumber(nCount);
    }

    public void OnShortCutClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO Shortcut");
    }
}
