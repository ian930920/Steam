using System.Linq;

public class UI_Scroll_Rune : ScrollGroupUI<Item_Rune, ScrollGroup_Rune>
{
    public override void UpdateData()
    {
        base.UpdateData();

        base.m_listData = ProjectManager.Instance.UserData.Inventory.GetRuneList().OrderBy(data => this.sort(data)).ToList();
    }

    public Item_Rune GetFirstRune()
    {
        if(base.m_listData.Count == 0) return null;

        return base.m_listData[0];
    }

    private int sort(Item_Rune data)
    {
        if(data.SummonID == ProjectManager.Instance.UI.PopupSystem.GetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).SummonID) return 0;
        if(data.SummonID != 0) return 1;

        return 2;
    }
}