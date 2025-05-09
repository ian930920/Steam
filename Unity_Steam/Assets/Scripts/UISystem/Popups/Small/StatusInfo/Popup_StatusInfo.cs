using TMPro;
using UnityEngine;

public class Popup_StatusInfo : BasePopup
{
    [SerializeField] private GameObject m_gobjPopup = null;

    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    public void SetStatusInfo(uint statusID, Vector3 vecPos)
    {
        var data = TableManager.Instance.Status.GetData(statusID);
        this.m_textTitle.text = TableManager.Instance.String.GetString(data.strID);
        this.m_textDesc.text = TableManager.Instance.String.GetString(data.strID, TableData.TableString.eTYPE.Description);

        this.m_gobjPopup.transform.position = vecPos;
    }
}