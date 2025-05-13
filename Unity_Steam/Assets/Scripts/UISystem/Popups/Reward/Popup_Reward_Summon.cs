using Febucci.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Popup_Reward_Summon : BasePopup
{
    [SerializeField] private Image m_imgSummon = null;
    [SerializeField] private Animation m_anim = null;
    [SerializeField] private TypewriterByCharacter m_text = null;

    private UnityAction m_funcOnClose = null;

    private uint m_summonID = 0;

    public void SetSummon(uint summonID, UnityAction onClose)
    {
        this.m_summonID = summonID;
        this.m_funcOnClose = onClose;

        this.m_text.gameObject.SetActive(false);
        this.m_imgSummon.sprite = TableManager.Instance.Summon.GetSprite(this.m_summonID);

        this.m_anim.Play();
    }

    public void ActiveName()
    {
        this.m_text.gameObject.SetActive(true);
        this.m_text.ShowText(TableManager.Instance.String.GetString(TableManager.Instance.Summon.GetData(this.m_summonID).strID));
    }

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();

        if(this.m_funcOnClose != null) this.m_funcOnClose.Invoke();
    }
}