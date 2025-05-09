using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public int CurrTabIdx { get; private set; } = 0;

    private Button_Tab[] m_arrTab = null;

    [SerializeField] private Color m_colorTextActive = Color.white;
    [SerializeField] private Color m_colorTextInactive = Color.grey;

    public void Init()
    {
        this.m_arrTab = this.transform.GetComponentsInChildren<Button_Tab>(false);
        for(int i = 0; i < this.m_arrTab.Length; ++i)
        {
            this.m_arrTab[i].InitButton();
            this.m_arrTab[i].InitTab(this.m_colorTextActive, this.m_colorTextInactive);

            //일단 무조건 첫번째 탭 활성화
            this.m_arrTab[i].SetBtnActive(i == 0);
        }
    }

    public void ChangeTab(int nTabIdx)
    {
        //지금 탭 저장
        this.CurrTabIdx = nTabIdx;

        for(int i = 0; i < this.m_arrTab.Length; ++i)
        {
            this.m_arrTab[i].SetBtnActive(i == nTabIdx);
        }
    }
}