using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHUD : MonoBehaviour
{
    [SerializeField] private UI_CanvasGroup m_uiCanvasGroup = null;
    public bool Visible
    {
        set
        {
            if(this.m_uiCanvasGroup == null) return;

            this.m_uiCanvasGroup.Active = value;
        }
    }

    [SerializeField] private UI_ReactiveGroup m_uiReactiveGroup = null;
    public UI_ReactiveGroup ReactiveGroup { get => this.m_uiReactiveGroup; }

    [SerializeField] private UI_CurrencySlotGroup m_uiCurrencyGroup = null;

    //Key : PopupID, Value : UI_New
    private Dictionary<int, UI_New> m_dicNew = new Dictionary<int, UI_New>();

	[Header("Transform_UI")][Space(5)]
    [SerializeField] private Transform m_transReactiveParent = null;
	public Transform TransReactiveParent { get { return this.m_transReactiveParent; } }

    public virtual void Init()
    {
        UIManager.Instance.SetUIScaler(this.GetComponent<CanvasScaler>());

		//모든 버튼 가져와서 세팅
		var arrBtn = this.GetComponentsInChildren<BaseButton>(true);
		for(int i = 0; i < arrBtn.Length; ++i)
        {
			arrBtn[i].InitButton();
        }

        //String 세팅
        var arrString = this.GetComponentsInChildren<Text_StringData>(true);
        for(int i = 0; i < arrString.Length; ++i)
        {
            arrString[i].Init();
        }

        //New들 가져오기
        var arrNew = this.GetComponentsInChildren<UI_New>(true);
        for(int i = 0, nMax = arrNew.Length; i < nMax; ++i)
        {
            this.m_dicNew.Add(arrNew[i].PopupID, arrNew[i]);
            arrNew[i].IsNew = false;
        }

        this.m_uiCurrencyGroup?.InitSlot();
    }

    abstract public void RefreshUI();

    public void SetNew(ePOPUP_ID ePopupID, bool isNew)
    {
        int nPopupID = (int)ePopupID;
        if(this.m_dicNew.ContainsKey(nPopupID) == false) return;
        if(this.m_dicNew[nPopupID].IsNew == isNew) return;

        this.m_dicNew[nPopupID].IsNew = isNew;
    }

    public bool IsNew(ePOPUP_ID ePopupID)
    {
        int nPopupID = (int)ePopupID;
        if(this.m_dicNew.ContainsKey(nPopupID) == false) return false;

        return this.m_dicNew[nPopupID].IsNew;
    }
}