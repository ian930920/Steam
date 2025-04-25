using UnityEngine;

public class UI_Mana : MonoBehaviour
{
    [SerializeField] private UI_ManaSlot[] m_arrMana = null;
    [SerializeField] private LayoutUpdater m_layoutUpdater = null;

    private ulong m_nMaxMana = 0;

    public void Init(ulong nMaxMana)
    {
        this.m_nMaxMana = nMaxMana;
        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            this.m_arrMana[i].gameObject.SetActive(i < (int)this.m_nMaxMana);
        }

        this.m_layoutUpdater.Refresh();
    }

    public void Refresh(ulong nCurrMana)
    {
        //지금 마나 갯수로 갱신
        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            if(this.m_arrMana[i].gameObject.activeSelf == false) continue;

            this.m_arrMana[i].RefreshSlot(i < (int)nCurrMana);
        }
    }
}