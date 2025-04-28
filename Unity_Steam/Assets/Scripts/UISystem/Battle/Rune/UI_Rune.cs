using UnityEngine;

public class UI_Rune : MonoBehaviour
{
    [SerializeField] private UI_RuneSlot[] m_arrSlot = null;
    [SerializeField] private LayoutUpdater m_layoutUpdater = null;

    public void Init(uint summonID)
    {
        var listRune = ProjectManager.Instance.UserData.User.GetRuneList(summonID);
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(i >= listRune.Count)
            {
                this.m_arrSlot[i].gameObject.SetActive(false);
                continue;
            }

            this.m_arrSlot[i].Init(listRune[i]);
        }

        this.m_layoutUpdater.Refresh();
    }
}