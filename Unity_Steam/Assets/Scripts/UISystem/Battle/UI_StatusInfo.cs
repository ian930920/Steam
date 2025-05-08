using TMPro;
using UnityEngine;

public class UI_StatusInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    public void SetStatusInfo(uint statusID)
    {
        var data = ProjectManager.Instance.Table.Status.GetData(statusID);
        this.m_textTitle.text = ProjectManager.Instance.Table.String.GetString(data.strID);
        this.m_textDesc.text = ProjectManager.Instance.Table.String.GetString(data.strID, TableData.TableString.eTYPE.Description);

        this.gameObject.SetActive(true);
    }

    public void Inactive()
    {
        this.gameObject.SetActive(false);
    }
}