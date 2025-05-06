using System.Linq;

public class UI_Scroll_Rune : ScrollGroupUI<Item_Rune, ScrollGroup_Rune>
{
    /*
    protected override void initData()
    {
        base.initData();

        base.m_listData = ProjectManager.Instance.UserData.Inventory.GetRuneList();
    }
    */

    public override void UpdateData()
    {
        base.UpdateData();

        if(base.m_listData.Count == 0) base.m_listData = ProjectManager.Instance.UserData.Inventory.GetRuneList();

        this.m_listData = this.m_listData.OrderBy(data => this.sort(data)).ToList();
    }

    private int sort(Item_Rune data)
    {
        //if(data.SummonID == ProjectManager.Instance.UI.PopupSystem.GetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).SummonID) return 0;
        if(data.SummonID != 0) return 1;

        return 2;
    }
}