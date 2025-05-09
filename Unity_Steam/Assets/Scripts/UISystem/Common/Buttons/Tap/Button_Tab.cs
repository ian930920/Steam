using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Button_Tab : BaseButton
{
    [SerializeField] private TextMeshProUGUI m_textTitle = null;

    [SerializeField] private GameObject m_gobjActive = null;

    [Header("활성화 시 활성화할 오브젝트들")]
    [SerializeField] private List<GameObject> m_listOnActiveObject = null;

    private bool m_isActive = true;

    private Color m_colorActive;
    private Color m_colorInactive;

    public void InitTab(Color colorActive, Color colorInactive)
    {
        this.m_colorActive = colorActive;
        this.m_colorInactive = colorInactive;
    }

    public override void OnClicked()
    {
        //활성상태라면 ㄴㄴ
        if(this.m_isActive == true) return;

        base.OnClicked();
    }

    public void SetBtnActive(bool bActive)
    {
        //활성 상태 저장
        this.m_isActive = bActive;

        //텍스트 색상 변경
        this.m_textTitle.color = this.m_isActive ? this.m_colorActive : this.m_colorInactive;
        this.m_gobjActive.SetActive(this.m_isActive);

        //오브젝트 상태 변경
        for(int i = 0, nMax = this.m_listOnActiveObject.Count; i < nMax; ++i)
        {
            this.m_listOnActiveObject[i].SetActive(this.m_isActive);
        }
    }
}