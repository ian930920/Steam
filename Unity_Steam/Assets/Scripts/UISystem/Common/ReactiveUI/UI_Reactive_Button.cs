using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_Reactive_Button : UI_Reactive
{
    public enum eTYPE
    {
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

    private void Awake()
    {
        this.m_btn.InitButton();
    }

    public void ActiveUI(eTYPE eType, Transform transTarget, UnityAction funcOnClick)
    {
        this.Type = eType;

        this.m_funcOnClick = funcOnClick;
        base.ActiveUI(transTarget);

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