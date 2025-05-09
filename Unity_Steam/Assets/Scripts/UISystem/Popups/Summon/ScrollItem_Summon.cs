using UnityEngine;

public class ScrollItem_Summon : ScrollItem<uint>
{
    [SerializeField] private UI_MySummonInfo m_summonInfo = null;

    public override void RefreshCellView()
    {
        base.RefreshCellView();

        this.m_summonInfo.Refresh(base.Data);
    }

    public void OnRuneEquipClicked()
    {
        UIManager.Instance.PopupSystem.OpenRuneEquipPopup(base.Data);
    }
}