using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurrencySlot : MonoBehaviour
{
    [SerializeField] private TableData.TableItem.eID m_eCurrencyID = TableData.TableItem.eID.Gold;
    private int ItemID { get => (int)this.m_eCurrencyID; }
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textValue = null;

    public void InitSlot()
    {
        this.m_imgIcon.sprite = ProjectManager.Instance.Table.Item.GetSprite(this.ItemID);

        //갱신 이벤트 등록
        ProjectManager.Instance.UserData.AddItemCountRefreshEvent(this.ItemID, this.refreshSlot);

        //갱신
        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        this.refreshSlot(ProjectManager.Instance.UserData.GetInventoryItem(this.ItemID).Count);
    }

    private void refreshSlot(uint nCount)
    {
        //this.m_textValue.text = Utility_UI.GetCommaNumber(nCount);
        this.m_textValue.text = Utility_UI.GetBigIntToABC(new BigInteger(200000));
    }

    public void OnShortCutClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO Shortcut");
    }
}
