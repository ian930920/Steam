using UnityEngine;
using UnityEngine.Events;

public class Popup_StationShop : BasePopup
{
    [SerializeField] private UI_ShopSlot_Summon[] m_arrSlotSummon = null;
    [SerializeField] private UI_ShopSlot_Rune[] m_arrSlotRune = null;

    private static readonly int COUNT_MAX_GOODS = 3;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.init();

        return this;
    }

    private void init()
    {
        int summonCount = Random.Range(0, COUNT_MAX_GOODS);

        var listSummon = TableManager.Instance.Summon.GetRandomList(summonCount);
        for(int i = 0, nMax = this.m_arrSlotSummon.Length; i < nMax; ++i)
        {
            if(listSummon.Count <= i)
            {
                this.m_arrSlotSummon[i].gameObject.SetActive(false);
                continue;
            }

            this.m_arrSlotSummon[i].InitSlot(listSummon[i].tableID);
        }

        int runeCount = COUNT_MAX_GOODS - summonCount;
        if(runeCount < 0) runeCount = 0;

        var listRune = TableManager.Instance.Rune.GetRandomList(runeCount);
        for(int i = 0, nMax = this.m_arrSlotRune.Length; i < nMax; ++i)
        {
            if(listRune.Count <= i)
            {
                this.m_arrSlotRune[i].gameObject.SetActive(false);
                continue;
            }

            this.m_arrSlotRune[i].InitSlot(listRune[i].tableID);
        }
    }
}