using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fx_Animation_Count : Fx_Animation
{
    [SerializeField] private TextMeshProUGUI m_textCount = null;
    [SerializeField] private bool m_isUI = false;

    /*
    public void Init(System.Numerics.BigInteger nValue, Vector3 vecPos)
    {
        this.m_textCount.text = $"{Utility_UI.GetBigIntToABC(nValue)}";

        if(this.m_isUI == false) base.Play(Camera.main.WorldToScreenPoint(vecPos));
        else base.Play(vecPos);
    }
    */

    public void Init(stDamage damage, Vector3 vecPos)
    {
        this.m_textCount.text = damage.Damage == 0 ? "Miss" : $"{Utility_UI.GetCommaNumber(damage.Damage)}";
        this.m_textCount.color = damage.IsCritical ? Color.red : Color.white;
        if(damage.IsHeal == true) this.m_textCount.color = Color.green;

        if(this.m_isUI == false) base.Play(Camera.main.WorldToScreenPoint(vecPos));
        else base.Play(vecPos);
    }
}