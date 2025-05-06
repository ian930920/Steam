using TMPro;
using UnityEngine;

public class Popup_RuneInfo : BasePopup
{
    [SerializeField] private GameObject m_gobjPopup = null;

    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    public void SetRuneInfo(uint runeID, Vector3 vecPos)
    {
        this.m_textTitle.text = ProjectManager.Instance.Table.Rune.GetString_Title(runeID);
        this.m_textDesc.text = ProjectManager.Instance.Table.Rune.GetString_Desc(runeID);

        this.m_gobjPopup.transform.position = vecPos;
    }
}