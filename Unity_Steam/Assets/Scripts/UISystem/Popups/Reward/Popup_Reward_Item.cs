using Febucci.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Popup_Reward_Item : BasePopup
{
    [SerializeField] private Image m_imgItem = null;
    [SerializeField] private Animation m_anim = null;
    [SerializeField] private TypewriterByCharacter m_text = null;
    
    private UnityAction m_funcOnClose = null;

    public void SetItem(stItem stReward, UnityAction onClose)
    {
        this.m_funcOnClose = onClose;

        this.m_imgItem.sprite = TableManager.Instance.Item.GetIcon(stReward.ItemID);
        this.m_text.ShowText(TableManager.Instance.Item.GetString_ItemCount(stReward));

        this.m_anim.Play();
    }

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();

        if(this.m_funcOnClose != null) this.m_funcOnClose.Invoke();
    }
}