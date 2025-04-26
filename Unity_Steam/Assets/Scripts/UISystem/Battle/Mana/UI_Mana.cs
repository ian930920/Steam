using UnityEngine;

public class UI_Mana : MonoBehaviour
{
    [SerializeField] private UI_ManaSlot[] m_arrMana = null;
    [SerializeField] private LayoutUpdater m_layoutUpdater = null;

    public void SetMaxMana(ulong nMaxMana)
    {
        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            this.m_arrMana[i].gameObject.SetActive(i < (int)nMaxMana);
        }

        this.m_layoutUpdater.Refresh();
    }

    public void Refresh(ulong nCurrMana, ulong nMaxMana = 0)
    {
        //지금 마나 갯수로 갱신
        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            if(nMaxMana == 0 && this.m_arrMana[i].gameObject.activeSelf == false) continue;

            if(nMax > i && this.m_arrMana[i].gameObject.activeSelf == false) this.m_arrMana[i].gameObject.SetActive(true);

            this.m_arrMana[i].RefreshSlot(i < (int)nCurrMana);
        }
    }
}