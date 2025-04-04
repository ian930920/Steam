using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Button_Tab : BaseButton
{
    [SerializeField] private Image m_imgBtn = null;
    [SerializeField] private TextMeshProUGUI m_textTitle = null;

    [SerializeField] private Image m_imgNew = null;
    [SerializeField] private Image m_imgSelect = null;

    [Space(5)][Header("활성화")]
    [SerializeField] private Sprite m_sprActive = null;
    [SerializeField] private Color m_colorTextActive = Color.white;
    
    [Space(5)][Header("비활성화")]
    [SerializeField] private Sprite m_sprInactive = null;
    [SerializeField] private Color m_colorTextInactive = Color.grey;

    public bool IsNew
    {
        get
        {
            if(this.m_imgNew == null) return false;

            return this.m_imgNew.enabled;
        }
        set
        {
            if(this.m_imgNew == null) return;

            if(this.m_isActive == true && value == true) return;

            this.m_imgNew.enabled = value;
        }
    }

    [SerializeField] private GameObject m_gobjActive = null;

    [SerializeField] private bool m_isActive = true;

    public override void InitButton()
    {
        base.InitButton();

        this.IsNew = false;
    }

    public override void OnClicked()
    {
        //활성상태라면 ㄴㄴ
        if(this.m_isActive == true) return;

        base.OnClicked();
    }

    virtual public void SetBtnActive(bool bActive)
    {
        //New떠있다면 끄기
        if(bActive == true && this.IsNew == true) this.IsNew = false;

        //활성 상태 저장
        this.m_isActive = bActive;

        //텍스트 색상 변경
        this.m_textTitle.color = this.m_isActive ? this.m_colorTextActive : this.m_colorTextInactive;

        //버튼 이미지 변경
        if(this.m_isActive == true) this.m_imgBtn.sprite = this.m_sprActive;
        else this.m_imgBtn.sprite = this.m_sprInactive;

        //선택상태
        if(this.m_imgSelect != null) this.m_imgSelect.enabled = this.m_isActive;

        //오브젝트 상태 변경
        if(this.m_gobjActive != null) this.m_gobjActive.SetActive(this.m_isActive);
    }
}