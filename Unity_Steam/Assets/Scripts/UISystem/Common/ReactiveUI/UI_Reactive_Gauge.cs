using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Reactive_Gauge : UI_Reactive
{
    public enum eGAUGE_TYPE
    {
        Fill,
        Empty
    }

    [SerializeField] private Slider m_slider = null;

    public float Gauge { set => this.m_slider.value = value; }

    protected stReactiveGaugeInfo m_stInfo;

    public void ActiveUI(stReactiveGaugeInfo info)
    {
        this.m_stInfo = info;

        //게이지 초기화
        this.Gauge = 0;

        base.ActiveUI(this.m_stInfo.transTarget);
        this.transform.SetAsFirstSibling();

        //게이지 코루틴 시작
        switch(this.m_stInfo.eGaugeType)
        {
            case eGAUGE_TYPE.Fill:
            {
                StopCoroutine("coFillGauge");
                StartCoroutine("coFillGauge");
            }
            break;
            case eGAUGE_TYPE.Empty:
            {
                StopCoroutine("coEmptyGauge");
                StartCoroutine("coEmptyGauge");
            }
            break;
        }
    }

    private IEnumerator coFillGauge()
    {
        double dMul = 1 / this.m_stInfo.dTotalTime, dDuration = 0;
        while(dDuration < this.m_stInfo.dTotalTime)
        {
            this.Gauge = (float)(dDuration * dMul);

            this.onTick(dDuration);

            dDuration += ProjectManager.Instance.Time.DeltaTime;

            yield return null;
        }
        //끄고
        base.InactiveUI();

        //후처리
        if(this.m_stInfo.funcOnFinish != null) this.m_stInfo.funcOnFinish.Invoke();
    }

    private IEnumerator coEmptyGauge()
    {
        double dMul = 1 / this.m_stInfo.dTotalTime, dRemainTime = this.m_stInfo.dRemainTime;
        while(dRemainTime > 0)
        {
            this.Gauge = (float)(dRemainTime * dMul);

            this.onTick(dRemainTime);

            dRemainTime -= ProjectManager.Instance.Time.DeltaTime;

            yield return null;
        }
        //끄고
        base.InactiveUI();

        //후처리
        if(this.m_stInfo.funcOnFinish != null) this.m_stInfo.funcOnFinish.Invoke();
    }

    protected virtual void onTick(double dDuration) { }
}

public struct stReactiveGaugeInfo
{
    public Transform transTarget;
    public UI_Reactive_Gauge.eGAUGE_TYPE eGaugeType;
    public double dTotalTime;
    public double dRemainTime;
    public UnityAction funcOnFinish;

    public stReactiveGaugeInfo(Transform transTarget, UnityAction funcOnFinish, double dTotalTime, double dRemainTime = 0, UI_Reactive_Gauge.eGAUGE_TYPE eGaugeType = UI_Reactive_Gauge.eGAUGE_TYPE.Fill)
    {
        this.transTarget = transTarget;
        this.eGaugeType = eGaugeType;
        this.dTotalTime = dTotalTime;
        this.dRemainTime = dRemainTime;
        this.funcOnFinish = funcOnFinish;
    } 
}