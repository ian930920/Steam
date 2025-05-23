using UnityEngine;
using UnityEngine.UI;

public class UI_RuneSlot : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon = null;

    private uint m_runeID = 0;

    public void Init(uint runeID)
    {
        this.m_runeID = runeID;
        this.m_imgIcon.sprite = TableManager.Instance.Rune.GetIcon(this.m_runeID);
        this.m_imgIcon.enabled = true;
    }

    public void Inactive()
    {
        this.m_imgIcon.enabled = false;
    }

    public void OnDetailClicked()
    {
        UIManager.Instance.PopupSystem.OpenRuneInfoPopup(this.m_runeID, this.transform.position);
    }

    public void CloseDetail()
    {
        UIManager.Instance.PopupSystem.ClosePopup(ePOPUP_ID.RuneInfo);
    }
}