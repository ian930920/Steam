using TMPro;
using UnityEngine;

public class UI_StatusInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Open(uint statusID, Vector3 vecPos)
    {
        if(this.gameObject.activeSelf == true) return;

        var data = ProjectManager.Instance.Table.Status.GetData(statusID);
        this.m_textTitle.text = ProjectManager.Instance.Table.String.GetString(data.strID);
        this.m_textDesc.text = ProjectManager.Instance.Table.String.GetString(data.strID, TableData.TableString.eTYPE.Description);

        this.transform.position = vecPos;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        if(this.gameObject.activeSelf == false) return;

        this.gameObject.SetActive(false);
    }
}