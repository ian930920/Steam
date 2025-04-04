using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UI_Reactive_Balloon : UI_Reactive
{
    public enum eTYPE
    {
        Guest_Move,
        Guest_Angry,
        Guest_Happy,
        Guest_HappyRest,
        Cat_Guest,
        Cat,
        Cat_Rest,
        Tip
    }

    [SerializeField] private TextMeshProUGUI m_textDesc = null;
    [SerializeField] private eTYPE m_eType = eTYPE.Tip;
    private UnityAction m_onFinished = null;

    private static readonly float TIME_DURATION = 5.0f;

    public void ActiveUI(eTYPE eType, Transform transTarget, UnityAction onFinished)
    {
        this.m_onFinished = onFinished;

        this.m_eType = eType;
        base.ActiveUI(transTarget);

        this.transform.SetAsFirstSibling();

        StartCoroutine("coBalloon");
    }

    private IEnumerator coBalloon()
    {
        this.m_textDesc.text = "ㅇ대사";
        Utility_UI.SetTextRectTransformWidth(this.m_textDesc);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(TIME_DURATION);

        if(this.m_onFinished != null) this.m_onFinished.Invoke();

        //말풍선 끄기
        this.InactiveUI();
    }
}
