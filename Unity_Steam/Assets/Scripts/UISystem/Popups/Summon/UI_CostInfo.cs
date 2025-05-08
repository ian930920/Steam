using TMPro;
using UnityEngine;

public class UI_CostInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textCost = null;
    [SerializeField] private TextColor_EffectType m_tcEffectType = null;

    public void Init(int cost)
    {
        this.m_textCost.text = $"X {cost}";
    }

    public void SetTextColor(TableData.TableStatus.eEFFECT_TYPE eType)
    {
        this.m_tcEffectType.SetTextColor(eType);
    }
}