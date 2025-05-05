using UnityEngine;

public class UI_RuneGroup : MonoBehaviour
{
    [SerializeField] private UI_RuneSlot[] m_arrSlot = null;
    [SerializeField] private LayoutUpdater m_layoutUpdater = null;

    public void Init(uint summonID)
    {
        var maxRune = ProjectManager.Instance.Table.Summon.GetData(summonID).maxRune;
        var listRune = ProjectManager.Instance.UserData.Summon.GetRuneList(summonID);
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(i >= maxRune)
            {
                this.m_arrSlot[i].gameObject.SetActive(false);
                continue;
            }

            if(i < listRune.Count) this.m_arrSlot[i].Init(listRune[i]);
            else this.m_arrSlot[i].Inactive();
        }

        this.m_layoutUpdater.Refresh();
    }
}