using UnityEngine;

public class UI_Cost : MonoBehaviour
{
    [SerializeField] private GameObject[] m_arrGobjCost = null;

    private int m_nMax = 0;

    public void Init(int nMaxCost)
    {
        this.m_nMax = nMaxCost;
        for(int i = 0, nMax = this.m_arrGobjCost.Length; i < nMax; ++i)
        {
            this.m_arrGobjCost[i].SetActive(i < this.m_nMax);
        }
    }

    public void Refresh()
    {
        //
    }
}
