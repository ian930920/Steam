using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_DailyResetTimer : MonoBehaviour
{
    private bool m_isInit = false;
    [SerializeField] private TextMeshProUGUI m_textResetTimer = null;

    public void Init()
    {
        if(this.m_isInit == true) return;

        this.m_isInit = true;
        this.refreshResetTimer();
        TimeManager.Instance.AddRefreshUIEvent(this.refreshResetTimer);
    }

    private void refreshResetTimer()
    {
        if(this.gameObject.activeSelf == false) return;

        this.m_textResetTimer.text = $"+STR 초기화까지 {TimeManager.Instance.DailyResetTime}";
    }
}