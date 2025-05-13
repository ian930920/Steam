using UnityEngine;
using UnityEngine.Events;

public class Popup_StationShop : BasePopup
{
    [SerializeField] private GameObject m_gobjSummon = null;
    [SerializeField] private UI_ShopSlot_Summon[] m_arrSlotSummon = null;

    [SerializeField] private GameObject m_gobjRune = null;
    [SerializeField] private UI_ShopSlot_Rune[] m_arrSlotRune = null;

    public static readonly int COUNT_MAX_GOODS = 3;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.m_gobjSummon.SetActive(false);
        this.m_gobjRune.SetActive(false);
        this.init();

        return this;
    }

    private void init()
    {
        switch(UserDataManager.Instance.Session.ShopType)
        {
            case eSHOP_TYPE.Rune:
            {
                this.m_gobjRune.SetActive(true);
                var listCount = UserDataManager.Instance.Session.ShopCount;
                for(int i = 0, nMax = this.m_arrSlotRune.Length; i < nMax; ++i)
                {
                    if(listCount <= i)
                    {
                        this.m_arrSlotRune[i].gameObject.SetActive(false);
                        continue;
                    }

                    this.m_arrSlotRune[i].InitSlot(UserDataManager.Instance.Session.GetShop(i));
                }
            }
            break;

            case eSHOP_TYPE.Summon:
            {
                this.m_gobjSummon.SetActive(true);
                var listCount = UserDataManager.Instance.Session.ShopCount;
                for(int i = 0, nMax = this.m_arrSlotRune.Length; i < nMax; ++i)
                {
                    if(listCount <= i)
                    {
                        this.m_arrSlotSummon[i].gameObject.SetActive(false);
                        continue;
                    }

                    this.m_arrSlotSummon[i].InitSlot(UserDataManager.Instance.Session.GetShop(i).ItemID);
                }
            }
            break;
        }
    }
}