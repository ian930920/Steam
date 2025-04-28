using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fx_Animation_Count : Fx_Animation
{
    [SerializeField] private TextMeshProUGUI m_textCount = null;
    [SerializeField] private bool m_isUI = false;

    public void Init(stDamage damage, Vector3 vecPos)
    {
        this.m_textCount.color = damage.IsCritical ? Color.red : Color.white;
        switch(damage.eSkillType)
        {
            case stDamage.eSKILL_TYPE.Miss:
            {
                this.m_textCount.text = "Miss";
            }
            break;

            case stDamage.eSKILL_TYPE.Heal:
            {
                this.m_textCount.color = Color.green;
            }
            break;

            default:
            {
                this.m_textCount.text = $"{Utility_UI.GetCommaNumber(damage.Value)}";
            }
            break;
        }

        if(this.m_isUI == false) base.Play(Camera.main.WorldToScreenPoint(vecPos));
        else base.Play(vecPos);
    }
}