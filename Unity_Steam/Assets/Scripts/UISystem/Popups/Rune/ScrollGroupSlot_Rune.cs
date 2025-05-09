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

        this.m_imgIcon.sprite = TableManager.Instance.Rune.GetIcon(base.Data.RuneID);

        this.m_gobjEquiped.SetActive(base.Data.SummonID != 0);
        if(this.m_gobjEquiped.activeSelf == true)
        {
            var dataSummon = TableManager.Instance.Summon.GetData(base.Data.SummonID);
            this.m_textName.text = TableManager.Instance.String.GetString(dataSummon.strID);
        }

        bool isSelected = false;
        switch(UIManager.Instance.PopupSystem.CurrPopup.PopupID)
        {
            case ePOPUP_ID.RuneEquip:
            {
                isSelected = UIManager.Instance.PopupSystem.GetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).IsSelectedRune(base.Data.UniqueRuneID);
            }
            break;

            case ePOPUP_ID.Inventory:
            {
                isSelected = UIManager.Instance.PopupSystem.GetPopup<Popup_Inventory>(ePOPUP_ID.Inventory).IsSelectedRune(base.Data.UniqueRuneID);
            }
            break;
        }
        this.m_gobjSelected.SetActive(isSelected);
    }

    public void OnClicked()
    {
        switch(UIManager.Instance.PopupSystem.CurrPopup.PopupID)
        {
            case ePOPUP_ID.RuneEquip:
            {
                UIManager.Instance.PopupSystem.GetPopup<Popup_RuneEquip>(ePOPUP_ID.RuneEquip).EquipRunePreview(base.Data);
            }
            break;

            case ePOPUP_ID.Inventory:
            {
                UIManager.Instance.PopupSystem.GetPopup<Popup_Inventory>(ePOPUP_ID.Inventory).SelectRune(base.Data);
            }
            break;
        }
    }
}