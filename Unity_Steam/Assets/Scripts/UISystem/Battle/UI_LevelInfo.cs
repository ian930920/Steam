using UnityEngine;
using UnityEngine.UI;

public class UI_LevelInfo : MonoBehaviour
{
    [SerializeField] private GameObject[] m_arrGobj = null;

    public void Refresh(int nLevel)
    {
        for(int i = 0, nMax = this.m_arrGobj.Length; i < nMax; ++i)
        {
            this.m_arrGobj[i].SetActive(i < nLevel);
        }
    }
}