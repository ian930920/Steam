using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public int CurrTabIdx { get; private set; } = 0;
    public int TabCount { get => this.m_arrTab.Length; }

    private Button_Tab[] m_arrTab = null;

    public void Init()
    {
        this.m_arrTab = this.transform.GetComponentsInChildren<Button_Tab>();
        for(int i = 0; i < this.m_arrTab.Length; ++i)
        {
            this.m_arrTab[i].InitButton();

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

    public void SetNewImage(int nTabIdx)
    {
        this.m_arrTab[nTabIdx].IsNew = true;
    }

    public Transform GetTabTransform(int nTabIdx)
    {
        return this.m_arrTab[nTabIdx].transform;
    }
}