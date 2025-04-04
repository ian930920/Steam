using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Button_ChangeState : BaseButton
{
    [SerializeField] private Image m_imgButton = null;
    [SerializeField] private Color m_colorImgActive = Color.white;
    [SerializeField] private Color m_colorImgInactive = Color.black;

    [SerializeField] private TextMeshProUGUI m_textText = null;
    [SerializeField] private Color m_colorTextActive = Color.white;
    [SerializeField] private Color m_colorTextInactive = Color.black;

    private UIManager.eUI_BUTTON_STATE m_eState = UIManager.eUI_BUTTON_STATE.Active;

    public delegate void ChangeStateBtnClickedEvent();

    private UIManager.eUI_BUTTON_STATE ButtonState
    {
        set
        {
            this.m_eState = value;

            switch(this.m_eState)
            {
                case UIManager.eUI_BUTTON_STATE.Active:
                {
                    this.m_imgButton.color = this.m_colorImgActive;
                    this.m_textText.color = this.m_colorTextActive;
                }
                break;
                case UIManager.eUI_BUTTON_STATE.Inactive:
                {
                    this.m_imgButton.color = this.m_colorImgInactive;
                    this.m_textText.color = this.m_colorTextInactive;
                }
                break;
            }
        }
    }

    private ChangeStateBtnClickedEvent OnActive { get; set; } = null;
    private ChangeStateBtnClickedEvent OnInactive { get; set; } = null;

    public virtual void InitStateButton(ChangeStateBtnClickedEvent onActive, ChangeStateBtnClickedEvent onInactive)
    {
        this.OnActive = onActive;
        this.OnInactive = onInactive;
    }

    public void RefreshActive(bool isActive)
    {
        this.ButtonState = isActive == true ? UIManager.eUI_BUTTON_STATE.Active : UIManager.eUI_BUTTON_STATE.Inactive;
    }

    public void OnStateBtnClicked()
    {
        switch(this.m_eState)
        {
            case UIManager.eUI_BUTTON_STATE.Active:
            {
                if(this.OnActive != null) this.OnActive.Invoke();
            }
            break;
            case UIManager.eUI_BUTTON_STATE.Inactive:
            {
                if(this.OnInactive != null) this.OnInactive.Invoke();
            }
            break;
        }
    }
}