using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Gauge_Count : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private TextMeshProUGUI m_textCount;

    public void Refresh(double dCurr, double dMax)
    {
        this.m_slider.value = (float)(dCurr / dMax);
        this.m_textCount.text = Utility_UI.GetCountText(dCurr, dMax);
    }

    public void RefreshByBigInteger(BigInteger nCurr, BigInteger nMax)
    {
        if(nMax == BigInteger.Zero)
        {
            this.m_textCount.text = "";
            this.m_slider.value = 0;
            return;
        }

        decimal dResult = 0;
        int nDiffLength = nMax.ToString().Length - decimal.MaxValue.ToString().Length;
        if(nDiffLength > 0) dResult = (decimal)(nCurr / (BigInteger)Mathf.Pow(10, nDiffLength)) / (decimal)(nMax / (BigInteger)Mathf.Pow(10, nDiffLength));
        else dResult = (decimal)nCurr / (decimal)nMax;
        this.m_slider.value = (float)dResult;
        this.m_textCount.text = $"{Utility_UI.GetBigIntToABC(nCurr)}";
        this.m_textCount.text += $"/{Utility_UI.GetBigIntToABC(nMax)}";
    }
}