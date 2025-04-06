using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterStatusBar : MonoBehaviour
{
    [SerializeField] private Slider m_sliderHP = null;

    public void Init(ulong nHP)
    {
        this.m_sliderHP.minValue = 0;
        this.m_sliderHP.maxValue = nHP;
    }
}
