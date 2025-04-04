using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Lock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    public bool IsLocked { get; private set; } = false;

    private int m_nLockID = 0;

    public void Refresh(int nLockID, int nTargetID = 0)
    {
        this.m_nLockID = nLockID;

        this.IsLocked = false;//ProjectManager.Instance.Table.Lock.IsLocked(this.m_nLockID, nTargetID);

        this.setActive(this.IsLocked, nTargetID);
    }

    public void SetInactive()
    {
        if(this.gameObject.activeSelf == false) return;

        this.gameObject.SetActive(false);
    }

    /*
    public void Refresh(TableData.TableLock.eID eLockID, bool isLock)
    {
        this.m_nLockID = (int)eLockID;

        this.IsLocked = isLock;

        this.setActive(isLock);
    }
    */

    private void setActive(bool isLock, int nTargetID = 0)
    {
        if(this.gameObject.activeSelf != isLock) this.gameObject.SetActive(isLock);

        if(isLock == false) return;

        this.m_textDesc.text = ProjectManager.Instance.Table.String.GetString(TableData.TableString.eID.None); //ProjectManager.Instance.Table.Lock.GetString_Title(this.m_nLockID, nTargetID);
    }

    public void OnClicked()
    {

    }
}