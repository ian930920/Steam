using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollGroupSlot_Rune : BaseSlot<Item_Rune>
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private GameObject m_gobjEquiped = null;
    [SerializeField] private TextMeshProUGUI m_textName = null;
    [SerializeField] private GameObject m_gobjSelected = null;

    public override void RefreshSlot(Item_Rune rune)
    {
        base.RefreshSlot(rune);

        this.m_imgIcon.sprite = ProjectManager.Instance.Table.Rune.GetIcon(base.Data.RuneID);

        this.m_gobjEquiped.SetActive(base.Data.SummonID != 0);
        if(this.m_gobjEquiped.activeSelf == true)
        {
            var dataSummon = ProjectManager.Instance.Table.Summon.GetData(base.Data.SummonID);
            this.m_textName.text = ProjectManager.Instance.Table.String.GetString(dataSummon.strID);
        }

        bool isSelected = ProjectManager.Instance.UI.PopupSystem.GetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).IsSelectedRune(base.Data.UniqueRuneID);
        this.m_gobjSelected.SetActive(isSelected);
    }

    public void OnClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.GetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).EquipRunePreview(base.Data.UniqueRuneID, base.Data.RuneID);
    }
}