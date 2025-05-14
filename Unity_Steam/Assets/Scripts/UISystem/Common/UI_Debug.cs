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
    public void RestartData()
    {
        UserDataManager.Instance.ResetManager();
    }

    public void RestartSession()
    {
        UserDataManager.Instance.Session.FinishSession();
    }

    public void AddSummon()
    {
        UserDataManager.Instance.Summon.Debug_AddSummon();
    }

    public void RemoveSummon()
    {
        UserDataManager.Instance.Summon.Debug_RemoveSummon();
    }

    public void AddRune()
    {
        UserDataManager.Instance.Inventory.Debug_AddRune();
    }

    public void RemoveRune()
    {
        UserDataManager.Instance.Inventory.Debug_RemoveRune();
    }

    public void RemoveSummonRune()
    {
        UserDataManager.Instance.Inventory.Debug_RemoveRuneSummonID();
        UserDataManager.Instance.Summon.Debug_RemoveSummonRune();
    }
    #endregion
}