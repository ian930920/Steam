using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Button_ChangeState : MonoBehaviour
{
    [SerializeField] private Image m_imgButton = null;
    [SerializeField] private Sprite m_sprActive = null;
    [SerializeField] private Sprite m_sprInactive = null;

    [Space(5)]
    [SerializeField] private TextMeshProUGUI m_textText = null;
    [SerializeField] private Color m_colorTextActive = Color.white;
    [SerializeField] private Color m_colorTextInactive = Color.black;

    [Space(5)]
    [SerializeField] private UnityEvent m_funcOnInactiveClicked = null;

    public UIManager.eUI_BUTTON_STATE State { get; private set; } = UIManager.eUI_BUTTON_STATE.Active;

    public void RefreshActive(bool isActive)
    {
        this.State = isActive == true ? UIManager.eUI_BUTTON_STATE.Active : UIManager.eUI_BUTTON_STATE.Inactive;
        switch(this.State)
        {
            case UIManager.eUI_BUTTON_STATE.Active:
            {
                this.m_imgButton.sprite = this.m_sprActive;
                this.m_textText.color = this.m_colorTextActive;
            }
            break;
            case UIManager.eUI_BUTTON_STATE.Inactive:
            {
                this.m_imgButton.sprite = this.m_sprInactive;
                this.m_textText.color = this.m_colorTextInactive;
            }
            break;
        }
    }

    public void OnClicked()
    {
        if(this.m_funcOnInactiveClicked == null) return;

        this.m_funcOnInactiveClicked.Invoke();
    }
}