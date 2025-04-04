using UnityEngine;

public class DebugModeSystem : MonoBehaviour
{
    private readonly int COUNT_DEBUG_MODE = 5;
    private int m_nDebugMode = 0;
    public bool IsDebugMode => this.m_nDebugMode >= COUNT_DEBUG_MODE;

    [SerializeField] private GameObject m_gobjLunarConsole = null;

    public void SetDebugMode()
    {
        if(this.IsDebugMode == true) return;

        this.m_nDebugMode++;

        if(this.IsDebugMode == true) return;

        this.m_gobjLunarConsole.SetActive(true);

        //TODO UIManager
        //UIManager.Instance.PopupSystem.OpenSystemTimerPopup("DebugMode On");
    }
}