using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterStatusBar : MonoBehaviour
{
    [SerializeField] private Slider m_sliderHP = null;
    [SerializeField] private TextMeshProUGUI m_textHP = null;

    public void Init(Vector3 vecPos, ulong nHP)
    {
        this.transform.position = new Vector3(vecPos.x, vecPos.y - 20, vecPos.z);

        this.m_sliderHP.minValue = 0;
        this.m_sliderHP.maxValue = nHP;
        this.RefreshGauge(nHP);

        this.gameObject.SetActive(true);
    }

    public void RefreshGauge(ulong nHP)
    {
        this.m_sliderHP.value = nHP;
        this.m_textHP.text = $"{this.m_sliderHP.value}/{this.m_sliderHP.maxValue}";
    }
}