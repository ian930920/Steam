using TMPro;
using UnityEngine;

public class UI_CostInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textCost = null;

    public void Init(ulong cost)
    {
        this.m_textCost.text = $"X {cost}";
    }
}
