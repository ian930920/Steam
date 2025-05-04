using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using EnhancedUI.EnhancedScroller;
using static EnhancedUI.EnhancedScroller.EnhancedScroller;

[RequireComponent(typeof(Canvas))]
abstract public class BasePopup : MonoBehaviour
{
	[SerializeField] protected ePOPUP_ID m_ePopupID = ePOPUP_ID.End;
	public ePOPUP_ID PopupID { get => this.m_ePopupID; }

	[SerializeField] protected UIManager.eUI_TYPE m_ePopupType = UIManager.eUI_TYPE.Popup_Main;
	public UIManager.eUI_TYPE PopupType { get => this.m_ePopupType; }
	[SerializeField] private bool m_isOpenSound = true;

	//닫을 때 콜백 이벤트
	protected UnityAction m_funcOnCloseClicked = null;

	public bool IsOpen { get => this.m_canvas.enabled; }

	protected Canvas m_canvas = null;

    //최초 생성 할 때 호출
    virtual public void InitPopup()
    {
		this.m_canvas = this.GetComponent<Canvas>();
		this.m_canvas.enabled = false;

		ProjectManager.Instance.UI.SetUIScaler(this.GetComponent<CanvasScaler>());

		//모든 버튼 가져와서 세팅
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

    virtual public void RefreshPopup() { }

	//virtual public BasePopup OpenPopup(int nOreder) { return this.OpenPopup(nOreder, null); }

	virtual public BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
	{
		//캔버스 켜주고
		this.m_canvas.enabled = true;
		this.m_canvas.sortingOrder = this.m_ePopupType == UIManager.eUI_TYPE.Popup_Small ? nOreder + 100 : nOreder;

		//콜백 초기화
		this.m_funcOnCloseClicked = funcClose;

		//소리
		if(this.m_isOpenSound == true) ProjectManager.Instance.ObjectPool.PlayEffectSound(BaseSound.eID.Popup_Open);

		return this;
	}

	/// <returns>닫기 성공 true, 이미 닫혀있다면 false</returns>
	protected virtual bool closePopup()
	{
		//이미 닫혀있다면 ㄴㄴ
		if(this.IsOpen == false) return false;

		//UI 매니저에 알리기
		ProjectManager.Instance.UI.PopupSystem.RemovePopup(this.m_ePopupID);

		//애니 실행
		this.m_canvas.enabled = false;

		//모든 코루틴 끄기
		StopAllCoroutines();

		return true;
	}

	//닫기 버튼 클릭 함수
	virtual public void OnCloseClicked()
    {
		//이미 닫혀있다면 ㄴㄴ
		if(this.IsOpen == false) return;

		if(this.closePopup() == true)
        {
			//콜백
			if(this.m_funcOnCloseClicked != null) this.m_funcOnCloseClicked.Invoke();
        }
    }
}

public abstract class ScrollPopup : BasePopup
{
	[SerializeField] private ScrollUI[] m_arrScroller = null;
	[SerializeField] protected TabGroup m_tabGroup = null;
	public int CurrTabIdx { get => this.m_tabGroup == null ? 0 : this.m_tabGroup.CurrTabIdx; }

	public override void InitPopup()
    {
		for(int i = 0, nMax = this.m_arrScroller.Length; i < nMax; ++i)
		{
			this.m_arrScroller[i].InitScroller();
		}

		this.m_tabGroup?.Init();

        base.InitPopup();
    }

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
		base.OpenPopup(nOreder, funcClose);

		if(this.IsScrolled() == true)
		{
			this.updateAllScrollerData();
			this.ChangeTab(0);
			this.ResetAllScroller();
		}

        return this;
    }

	protected override bool closePopup()
	{
		//모든 코루틴 끄기
		for(int i = 0, nMax = this.m_arrScroller.Length; i < nMax; ++i)
        {
			this.m_arrScroller[i].InactiveCellViews();
        }

		return base.closePopup();
	}

    public override void RefreshPopup()
    {
        base.RefreshPopup();

		this.GetScroller(this.CurrTabIdx)?.UpdateData();
		this.RefreshScroller(this.CurrTabIdx);
    }

	protected void UpdateReset(int nTabIdx)
	{
		this.GetScroller(nTabIdx)?.UpdateData();
		this.ResetScroller(nTabIdx);
	}

	public virtual void ChangeTab(int nTabIdx)
    {
		if(this.m_tabGroup == null) return;

		this.m_tabGroup.ChangeTab(nTabIdx);

		this.ResetScroller(this.m_tabGroup.CurrTabIdx);
    }

	private void updateAllScrollerData()
    {
		for(int i = 0, nMax = this.m_arrScroller.Length; i < nMax; ++i)
		{
			this.m_arrScroller[i].UpdateData();
		}
    }

	protected ScrollUI GetScroller(int nIdx = 0)
    {
		if(this.m_arrScroller.Length <= nIdx) return null;

		return this.m_arrScroller[nIdx];
    }

	protected bool IsScrolled(int nIdx = 0)
    {
		if(this.m_arrScroller.Length <= nIdx) return false;

		return this.m_arrScroller[nIdx].IsScrolled;
    }

	protected void SetScrolled(bool isScrolled, int nIdx = 0)
    {
		if(this.m_arrScroller.Length <= nIdx) return;

		this.m_arrScroller[nIdx].IsScrolled = isScrolled;
    }

    protected void ResetScroller(int nIdx = 0)
    {
		this.GetScroller(nIdx)?.ResetScroller();
    }

	protected void ResetAllScroller()
    {
		for(int i = 0, nMax = this.m_arrScroller.Length; i < nMax; ++i)
		{
			this.m_arrScroller[i].ResetScroller();
		}
    }

	protected void RefreshScroller(int nIdx = 0)
    {
		this.GetScroller(nIdx)?.RefreshScroller();
    }

	protected void RefreshAllScroller()
    {
		for(int i = 0, nMax = this.m_arrScroller.Length; i < nMax; ++i)
		{
			this.m_arrScroller[i].RefreshScroller();
		}
    }

	protected void focusByDataIndex(int nDataIdx, int nIdx = 0)
    {
        this.GetScroller(nIdx)?.FocusByDataIndex(nDataIdx);
    }

	protected void repositionScroller(int nIdx = 0)
	{
		this.GetScroller(nIdx)?.RepositionScroller();
	}
}

public class ScrollUI : MonoBehaviour, IEnhancedScrollerDelegate
{
	private EnhancedScroller m_scroller = null;
	private ScrollRect m_scrollRect = null;
	public float ScrollPosition { get => this.m_scroller.ScrollPosition; set => this.m_scroller.ScrollPosition = value; }

	[SerializeField] protected EnhancedScrollerCellView[] m_arrCellItem = null;
	protected RectTransform[] m_arrTransItem = null;

	[SerializeField] private int m_nRowCount = 1;
	protected int RowCount { get => this.m_nRowCount; }
	public bool IsScrolled = true;

	protected virtual int DataCount { get; }

    protected virtual void initData() { }
	public virtual void UpdateData() { }

	public void InitScroller()
	{
        if(this.m_scroller == null) this.m_scroller = this.GetComponent<EnhancedScroller>();
        if(this.m_scrollRect == null) this.m_scrollRect = this.GetComponent<ScrollRect>();

		this.initData();

		//스크롤 세팅
		this.m_scroller.Delegate = this;
		this.m_scroller.enabled = true;

		this.m_arrTransItem = new RectTransform[this.m_arrCellItem.Length];
		for(int i = 0, nMax = this.m_arrTransItem.Length; i < nMax; ++i)
        {
			this.m_arrTransItem[i] = this.m_arrCellItem[i].GetComponent<RectTransform>();
        }

		this.m_scroller.scrollerScrollingChanged = this.onScrolled;
		this.IsScrolled = true;
	}

	private void onScrolled(EnhancedScroller scroller, bool scrolling)
	{
		this.IsScrolled = scrolling;
	}

	public void SetMovementType(ScrollRect.MovementType eType)
    {
		if(this.isValuableScroller() == false) return;

		this.m_scrollRect.movementType = eType;
    }

	private bool isValuableScroller()
    {
        if(this.m_scroller == null) return false;
        if(this.m_scroller.Container == null) return false;

		return true;
    }

	public void RefreshScroller()
	{
		if(this.isValuableScroller() == false) return;

		this.m_scroller.RefreshActiveCellViews();
	}

	public void ResetScroller()
    {
        if(this.isValuableScroller() == false) return;

		this.m_scroller.ReloadData();
        this.m_scroller.RefreshActiveCellViews();
    }

	public void InactiveCellViews()
    {
        if(this.isValuableScroller() == false) return;

		this.m_scroller.InactiveCellViews();
    }

	public void FocusByDataIndex(int nDataIdx)
    {
        if(this.isValuableScroller() == false) return;

		this.m_scroller.JumpToDataIndex(nDataIdx);
    }

	public void RepositionScroller()
	{
		if(this.isValuableScroller() == false) return;

		this.m_scroller.ScrollPosition = 0;
	}

	public ScrollState GetHorizontalScrollState()
	{
		if(this.isValuableScroller() == false) return ScrollState.NotScrollable;

		return this.m_scroller.GetHorizontalScrollState();
	}

	public float GetCellViewSize(int nScrollIdx = 0)
	{
		//Default
		bool bHorizontal = this.m_scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal;

		return bHorizontal == true ? this.m_arrTransItem[0].sizeDelta.x : this.m_arrTransItem[0].sizeDelta.y;
	}

	#region "IEnhancedScrollerDelegate"

	public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return this.DataCount;
    }

	public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		//Default
		bool bHorizontal = scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal;

		return bHorizontal == true ? this.m_arrTransItem[0].sizeDelta.x : this.m_arrTransItem[0].sizeDelta.y;
	}

	public virtual EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex) { return null; }
   
	#endregion
}

public class ScrollGroupUI<Data, Slot> : ScrollUI where Slot : BaseScrollGroup<Data>
{
	protected List<Data> m_listData = new List<Data>();
    private List<Data> m_listTemp = new List<Data>();

	protected override int DataCount
    {
        get
        {
            int nDataCount = this.m_listData.Count / base.RowCount;
            if(this.m_listData.Count % base.RowCount > 0) nDataCount += 1;
            return nDataCount;
        }
    }

	protected override void initData()
    {
        base.initData();

        //리스트 가져오고
        this.UpdateData();
    }

	private Data[] getArrData(int nScrollIdx, int nCount)
    {
        this.m_listTemp.Clear();
        int nStartIdx = nScrollIdx * nCount;
        int nMax = Mathf.Clamp(nStartIdx + nCount, 0, this.m_listData.Count);
        for(int i = nStartIdx; i < nMax; ++i)
        {
            this.m_listTemp.Add(this.m_listData[i]);
        }
        return this.m_listTemp.ToArray();
    }

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        Slot item = scroller.GetCellView(base.m_arrCellItem[0]) as Slot;
        item.name = $"Item_{cellIndex}";
        item.InitData(this.getArrData(dataIndex, base.RowCount));
        return item;
    }
}