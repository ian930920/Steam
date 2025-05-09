using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Reactive_Gauge_Time : UI_Reactive
{
    [SerializeField] private TextMeshProUGUI m_textTime = null;
    protected float Time { set => this.m_textTime.text = Utility_Time.GetTimeString(value); }

    [SerializeField] private Slider m_slider = null;
    [SerializeField] private bool m_isPreset = false;

    protected float Gauge { set => this.m_slider.value = value; }

    protected stGaugeTimeInfo m_stTime;

    public void ActiveUI(stGaugeTimeInfo stTime, Transform transTarget = null)
    {
        if(this.m_isPreset == false) base.ActiveUI(transTarget);
        //else if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);
        this.gameObject.SetActive(true);

        this.m_stTime = stTime;
        Invoke("StartGauge", 0.05f);
    }

    private void StartGauge()
    {
        StartCoroutine("coGauge");
    }

    virtual protected IEnumerator coGauge()
    {
        //시간 게이지
        double dTotalTime = this.m_stTime.funcGetTotalTime.Invoke();
        double dRemainTime = this.m_stTime.funcGetRemainTime.Invoke();
        double dMul = 1 / dTotalTime, dOneSec = dRemainTime - (int)dRemainTime;
        this.Time = (float)dRemainTime;
        while(dRemainTime > 0)
        {
            this.Gauge = (float)(dRemainTime * dMul);
            
            dRemainTime = this.m_stTime.funcGetRemainTime.Invoke();
            dOneSec += TimeManager.Instance.DeltaTime;

            yield return null;

            if(dOneSec < 1) continue;

            this.Time = (float)dRemainTime;
            dOneSec = 0;
        }

        if(this.m_stTime.funcOnTimeEnd != null) this.m_stTime.funcOnTimeEnd.Invoke();

        this.InactiveUI();
    }
}

public struct stGaugeTimeInfo
{
    public TimeManager.GetTime funcGetTotalTime;
    public TimeManager.GetTime funcGetRemainTime;
    public UnityAction funcOnTimeEnd;

    public stGaugeTimeInfo(TimeManager.GetTime funcGetTotalTime, TimeManager.GetTime funcGetRemainTime, UnityAction funcOnTimeEnd)
    {
        this.funcGetTotalTime = funcGetTotalTime;
        this.funcGetRemainTime = funcGetRemainTime;
        this.funcOnTimeEnd = funcOnTimeEnd;
    }
}