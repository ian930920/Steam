using UnityEngine;
using UnityEngine.Events;

public class Popup_Summon : BasePopup
{
    [SerializeField] private UI_SummonSelectSlot[] m_arrSlot = null;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        var listSummon = ProjectManager.Instance.UserData.User.GetSummonDataByList();
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
           // this.m_arrSlot[i].RefreshSlot(listSummon[i].SummonID);
            this.m_arrSlot[i].RefreshSelect(false);
        }

        return this;
    }
}
