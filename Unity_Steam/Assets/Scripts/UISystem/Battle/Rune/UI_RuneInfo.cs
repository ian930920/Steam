using TMPro;
using UnityEngine;

public class UI_RuneInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Open(uint runeID, Vector3 vecPos)
    {
        if(this.gameObject.activeSelf == true) return;

        this.m_textTitle.text = ProjectManager.Instance.Table.Rune.GetString_Title(runeID);
        this.m_textDesc.text = ProjectManager.Instance.Table.Rune.GetString_Desc(runeID);

        this.transform.position = vecPos;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        if(this.gameObject.activeSelf == false) return;

        this.gameObject.SetActive(false);
    }
}