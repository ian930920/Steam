using TMPro;
using UnityEngine;

public class Popup_StatusInfo : BasePopup
{
    [SerializeField] private GameObject m_gobjPopup = null;

    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    public void SetStatusInfo(uint statusID, Vector3 vecPos)
    {
        var data = ProjectManager.Instance.Table.Status.GetData(statusID);
        this.m_textTitle.text = ProjectManager.Instance.Table.String.GetString(data.strID);
        this.m_textDesc.text = ProjectManager.Instance.Table.String.GetString(data.strID, TableData.TableString.eTYPE.Description);

        this.m_gobjPopup.transform.position = vecPos;
    }
}