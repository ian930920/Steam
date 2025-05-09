using TMPro;
using UnityEngine;

public class UI_RuneInfo : MonoBehaviour
{
    private enum eBTN
    {
        Equip,
        Unequip,
        Change
    }

    [SerializeField] private TextMeshProUGUI m_textName = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    [SerializeField] private GameObject[] m_arrBtn = null;

    public void RefreshInfo(Item_Rune rune)
    {
        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);

        this.m_textName.text = TableManager.Instance.Rune.GetString_Title(rune.RuneID);
        this.m_textDesc.text = TableManager.Instance.Rune.GetString_Desc(rune.RuneID);
    }

    private void refreshBtn(Item_Rune rune)
    {
        //TODO
        //장착중인지 확인
        this.m_arrBtn[(int)eBTN.Equip].SetActive(rune.SummonID == 0);
        this.m_arrBtn[(int)eBTN.Unequip].SetActive(rune.SummonID != 0);
        this.m_arrBtn[(int)eBTN.Change].SetActive(rune.SummonID != 0);
    }

    public void OnEquipClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.Summon);
        UIManager.Instance.PopupSystem.OpenSystemPopup("정령의 룬 슬롯에서 룬을 장착 하세요.");
    }
}