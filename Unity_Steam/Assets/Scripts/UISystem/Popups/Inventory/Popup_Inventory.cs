using UnityEngine;
using UnityEngine.Events;

public class Popup_Inventory : ScrollPopup
{
    [SerializeField] private UI_RuneInfo m_uiRuneInfo = null;

    [SerializeField] private UI_Scroll_Rune m_scrollUI = null;
    [SerializeField] private GameObject m_gobjEmpty = null;

    private Item_Rune m_currRune = null;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.m_uiRuneInfo.gameObject.SetActive(false);

        this.m_currRune = null;

        this.RefreshEmpty();

        return this;
    }

    public void RefreshEmpty()
    {
        this.m_gobjEmpty.SetActive(this.m_scrollUI.IsEmpty);
    }

    public void SelectRune(Item_Rune rune)
    {
        this.m_currRune = rune;
        this.m_uiRuneInfo.RefreshInfo(this.m_currRune);
        this.RefreshScroller();
    }

    public bool IsSelectedRune(uint uniqueRuneID)
    {
        if(this.m_currRune == null) return false;

        return this.m_currRune.UniqueRuneID == uniqueRuneID;
    }
}