using UnityEngine;
using UnityEngine.UI;

public class UI_RuneSlot : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon = null;

    private uint m_runeID = 0;

    public void Init(uint runeID)
    {
        this.m_runeID = runeID;

        //TODO RuneTable
        //this.m_imgIcon.sprite = 

        this.gameObject.SetActive(true);
    }

    public void OnDetailClicked()
    {
        ProjectManager.Instance.BattleScene?.HUD.OpenRuneInfo(this.m_runeID, this.transform.position);
    }

    public void CloseDetail()
    {
        ProjectManager.Instance.BattleScene?.HUD.CloseRuneInfo();
    }
}