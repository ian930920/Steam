using UnityEngine;
using UnityEngine.UI;

public class UI_LevelInfo : MonoBehaviour
{
    [SerializeField] private Image[] m_arrImg = null;

    public void Refresh(int nLevel)
    {
        for(int i = 0, nMax = this.m_arrImg.Length; i < nMax; ++i)
        {
            this.m_arrImg[i].enabled = i < nLevel;
        }
    }
}