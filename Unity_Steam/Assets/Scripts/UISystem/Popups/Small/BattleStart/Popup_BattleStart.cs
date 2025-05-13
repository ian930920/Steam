using UnityEngine;
using UnityEngine.Events;

public class Popup_BattleStart : BasePopup
{
    public enum eTYPE
    {
        Normal,
        Boss
    }

    private static readonly string[] ARR_STR_ANIM =
    {
        "Anim_Normal",
        "Anim_Boss",
    };

    [SerializeField] private Animation m_anim = null;

    private UnityAction m_onClosed = null;

    public void SetType(eTYPE eType, UnityAction onClosed)
    {
        this.m_anim.Play(ARR_STR_ANIM[(int)eType]);

        this.m_onClosed = onClosed;
    }

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();

        if(this.m_onClosed != null) this.m_onClosed.Invoke();
    }
}
