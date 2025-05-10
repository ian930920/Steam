using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Button_Cost : MonoBehaviour
{
    [SerializeField] private BaseButton m_btn = null;
    [SerializeField] private Button_ChangeState m_csbtn = null;

    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textCount = null;

    [SerializeField] private UnityEvent m_onBuyClicked = null;

    private stItem m_stCost;

    public void InitCost(stItem item)
    {
        this.m_stCost = item;

        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);

        this.Refresh();
    }

    public void Refresh()
    {
        this.m_imgIcon.sprite = TableManager.Instance.Item.GetIcon(this.m_stCost.ItemID);
        this.m_textCount.text = Utility_UI.GetCommaNumber(this.m_stCost.Count);

        this.m_csbtn.RefreshActive(UserDataManager.Instance.Inventory.IsUseable(this.m_stCost));
    }

    public void OnBuyClicked()
    {
        if(this.m_csbtn.State == UIManager.eUI_BUTTON_STATE.Inactive)
        {
            UIManager.Instance.PopupSystem.OpenSystemTimerPopup($"{TableManager.Instance.Item.GetString(this.m_stCost.ItemID)}이(가) 부족합니다.");
            return;
        }

        this.m_onBuyClicked.Invoke();
    }
}