using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scroll_Summon : ScrollUI
{
    [SerializeField] private List<UserData_User.SummonData> m_listSummonData = new List<UserData_User.SummonData>();
    
    protected override int DataCount { get => this.m_listSummonData.Count; }

    public override void UpdateData()
    {
        base.UpdateData();

        this.m_listSummonData = ProjectManager.Instance.UserData.User.GetSummonDataList();
    }

    public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var item = scroller.GetCellView(base.m_arrCellItem[0]) as ScrollItem_Summon;
        item.name = $"Item_{cellIndex}";
        item.SetData(this.m_listSummonData[dataIndex]);
        return item;
    }
}