using UnityEngine;

public class UI_SummonSelectSlot : MonoBehaviour
{
    [SerializeField] private UI_DefaultSummonInfo m_summonInfo = null;

    [SerializeField] private GameObject m_gobjSelect = null;

    public void RefreshSlot(uint summonID)
    {
        this.m_summonInfo.Refresh(summonID);
    }

    public void RefreshSelect(bool isSelect)
    {
        this.m_gobjSelect.SetActive(isSelect);
    }
}