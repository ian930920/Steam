using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_New : MonoBehaviour
{
    [SerializeField] private ePOPUP_ID m_ePopupID = ePOPUP_ID.System;
    [SerializeField] private Image m_imgIcon = null;

    public bool IsNew
    {
        get => this.m_imgIcon.enabled;
        set => this.m_imgIcon.enabled = value;
    }

    public int PopupID { get => (int)this.m_ePopupID; }
}