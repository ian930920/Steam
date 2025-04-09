using UnityEngine;
using UnityEngine.UI;

public class UI_SummonSlot : BaseSlot<TableData.TableData_Summon>
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private Image m_imgEdge = null;

    public uint SummonID => base.Data.tableID;

    protected override void init()
    {
        base.init();

        this.m_imgEdge.enabled = false;
        this.gameObject.SetActive(true);
    }

    public override void RefreshSlot(TableData.TableData_Summon data)
    {
        base.RefreshSlot(data);

        this.m_imgIcon.sprite = ProjectManager.Instance.Table.Summon.GetSprite(base.Data.tableID);

        this.SetSelect(false);
        this.gameObject.SetActive(true);
    }

    public void SetSelect(bool bSelect)
    {
        this.m_imgEdge.enabled = bSelect;
    }
}
