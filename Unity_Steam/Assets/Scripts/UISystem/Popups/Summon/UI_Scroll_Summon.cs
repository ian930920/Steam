using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scroll_Summon : ScrollUI
{
    [SerializeField] private List<UserData_Summon.MySummon> m_listSummonData = new List<UserData_Summon.MySummon>();
    
    protected override int DataCount { get => this.m_listSummonData.Count; }

    public override void UpdateData()
    {
        base.UpdateData();

        this.m_listSummonData = UserDataManager.Instance.Summon.GetSummonDataList();
    }

    public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var item = scroller.GetCellView(base.m_arrCellItem[0]) as ScrollItem_Summon;
        item.name = $"Item_{cellIndex}";
        item.SetData(this.m_listSummonData[dataIndex].SummonID);
        return item;
    }
}