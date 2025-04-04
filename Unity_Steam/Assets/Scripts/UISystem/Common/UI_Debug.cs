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
}