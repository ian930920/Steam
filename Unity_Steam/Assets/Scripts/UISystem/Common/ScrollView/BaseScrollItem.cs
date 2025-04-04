using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

abstract public class BaseScrollItem : EnhancedScrollerCellView
{
    private void Awake()
    {
        //버튼 세팅
        BaseButton[] arrBtn = this.GetComponentsInChildren<BaseButton>(true);
        for(int i = 0; i < arrBtn.Length; ++i)
        {
            arrBtn[i].InitButton();
        }

        //String 세팅
        Text_StringData[] arrString = this.GetComponentsInChildren<Text_StringData>(true);
        for(int i = 0; i < arrString.Length; ++i)
        {
            arrString[i].Init();
        }
    }
}

abstract public class ScrollItem<D> : BaseScrollItem
{
    protected D Data { get; private set; }

    public virtual void InitData(D data)
    {
        //데이터 저장하고
        this.Data = data;

        //갱신
        this.RefreshCellView();
    }
}

abstract public class BaseScrollGroup<D> : BaseScrollItem
{
    [SerializeField] private BaseSlot<D>[] m_arrSlot = null;
    protected int RowCount { get => this.m_arrSlot.Length; }

    private D[] m_arrData = null;

    public virtual void InitData(D[] arrData)
    {
        this.m_arrData = arrData;

        this.RefreshCellView();
    }

    public override void RefreshCellView()
    {
        base.RefreshCellView();

        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(this.m_arrData.Length <= i)
            {
                this.m_arrSlot[i].gameObject.SetActive(false);
                continue;
            }

            if(this.m_arrSlot[i].gameObject.activeSelf == false) this.m_arrSlot[i].gameObject.SetActive(true);
            this.m_arrSlot[i].RefreshSlot(this.m_arrData[i]);
        }
    }
}

abstract public class BaseSlot<D> : MonoBehaviour
{
    protected D Data;

    private bool m_isInit = false;

    virtual protected void init()
    {
        this.m_isInit = true;
    }

    virtual public void RefreshSlot(D data)
    {
        if(this.m_isInit == false) this.init();

        this.Data = data;
    }
}