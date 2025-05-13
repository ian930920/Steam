using TMPro;
using UnityEngine;

public class Button_Event_Option : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textDesc = null;
    private uint m_optionID = 0;

    public void Init(uint optionID)
    {
        this.gameObject.SetActive(true);

        this.m_optionID = optionID;

        this.m_textDesc.text = TableManager.Instance.EventOption.GetString_Option(optionID);
    }

    public void OnOptionClicked()
    {
        if(UIManager.Instance.PopupSystem.IsOpenPopup(ePOPUP_ID.Event) == false) return;

        UIManager.Instance.PopupSystem.GetPopup<Popup_Event>(ePOPUP_ID.Event).ReciveReward(this.m_optionID);
    }
}