using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button_Text : BaseButton
{
    [SerializeField] private TextMeshProUGUI m_textDesc = null;
    [SerializeField] private bool m_isIgnoreResize = true;

    public string Text
    {
        set
        {
            this.m_textDesc.text = value;
            if(this.m_isIgnoreResize == false) Utility_UI.SetTextRectTransformWidth(this.m_textDesc);
        }
    }

    // TODO : Table참조해서 적용
}