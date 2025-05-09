using UnityEngine;

public class UI_RuneGroup : MonoBehaviour
{
    [SerializeField] private UI_RuneSlot[] m_arrSlot = null;
    [SerializeField] private LayoutUpdater m_layoutUpdater = null;

    public void Init(uint summonID, bool isDefault)
    {
        var maxRune = TableManager.Instance.Summon.GetData(summonID).maxRune;
        var listRune = UserDataManager.Instance.Summon.GetRuneList(summonID);
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(i >= maxRune)
            {
                this.m_arrSlot[i].gameObject.SetActive(false);
                continue;
            }

            if(this.m_arrSlot[i].gameObject.activeSelf == false) this.m_arrSlot[i].gameObject.SetActive(true);

            if(isDefault == false && i < listRune.Count) this.m_arrSlot[i].Init(listRune[i].RuneID);
            else this.m_arrSlot[i].Inactive();
        }

        this.m_layoutUpdater.Refresh();
    }
}