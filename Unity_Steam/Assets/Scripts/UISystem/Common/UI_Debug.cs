using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Debug : MonoBehaviour
{
    [SerializeField] private GameObject m_gobjPanel = null;

    private void Start()
    {
        this.m_gobjPanel.SetActive(false);
    }

    public void OnDebugActiveClicked(bool isActive)
    {
        this.m_gobjPanel.SetActive(isActive);
    }

    public void OnDebugCloseClicked()
    {
        this.gameObject.SetActive(false);
    }

    public void OnCheatClicked()
    {
        this.OnDebugActiveClicked(false);
    }

#region TitleScene
    public void AddSummon()
    {
        ProjectManager.Instance.UserData.Summon.Debug_AddSummon();
    }

    public void RemoveSummon()
    {
        ProjectManager.Instance.UserData.Summon.Debug_RemoveSummon();
    }

    public void AddRune()
    {
        ProjectManager.Instance.UserData.Inventory.Debug_AddRune();
    }

    public void RemoveRune()
    {
        ProjectManager.Instance.UserData.Inventory.Debug_RemoveRune();
    }

    public void RemoveSummonRune()
    {
        ProjectManager.Instance.UserData.Inventory.Debug_RemoveRuneSummonID();
        ProjectManager.Instance.UserData.Summon.Debug_RemoveSummonRune();
    }
    #endregion
}