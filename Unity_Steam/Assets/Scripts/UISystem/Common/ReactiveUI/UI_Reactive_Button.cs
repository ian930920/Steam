using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_Reactive_Button : UI_Reactive
{
    public enum eTYPE
    {
        Add,
        ReceiveOrder,
        Clean,
        Build,
        AD,
        VisitStamp,
        BuildingDetail,
        Event,
        Gambling,
        End
    }

    [SerializeField] private BaseButton m_btn = null;
    private UnityAction m_funcOnClick = null;

    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private Sprite[] m_arrSprite = null;

    private eTYPE m_eType = eTYPE.End;
    private eTYPE Type
    {
        set
        {
            if(this.m_eType == value) return;

            this.m_eType = value;
            this.m_imgIcon.sprite = this.m_arrSprite[(int)this.m_eType];
        }
    }

    public void ActiveUI(eTYPE eType, Transform transTarget, UnityAction funcOnClick)
    {
        if(this.m_btn.IsInit == false) this.m_btn.InitButton();

        this.Type = eType;

        this.m_funcOnClick = funcOnClick;

        UI_ReactiveGroup.eMODE eMode = UI_ReactiveGroup.eMODE.Default;
        if(this.m_eType == eTYPE.Build) eMode = UI_ReactiveGroup.eMODE.Build;

        if(this.m_eType == eTYPE.Clean) this.gameObject.tag = "Clean";
        else this.gameObject.tag = "Untagged";

        base.ActiveUI(transTarget, eMode);

        this.transform.SetAsLastSibling();
    }

    public void OnClicked()
    {
        //버튼 이벤트 처리
        this.m_funcOnClick?.Invoke();

        //UI 끄기
        this.InactiveUI();
    }
}