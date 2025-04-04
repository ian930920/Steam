using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text_StringData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text = null;
    [SerializeField] private bool m_isIngoreSizeControl = false;

    [SerializeField] private TableData.TableString.eID m_eStringID = TableData.TableString.eID.None;

    public void Init()
    {
        if(this.m_text == null) this.m_text = this.GetComponent<TextMeshProUGUI>();
        this.m_text.text = ProjectManager.Instance.Table.GetString(this.m_eStringID);
        if(this.m_isIngoreSizeControl == false) Utility_UI.SetTextRectTransformWidth(this.m_text);
    }
}