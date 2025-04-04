using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Popup_System_Confirm_Text : Popup_System
{
    private UnityAction m_funcConfirm = null;

    public void SetConfirm(string strDesc, UnityAction funcConfirm)
    {
        this.m_funcConfirm = funcConfirm;

        base.SetDescription(strDesc);
    }

    /*
    public void SetConfirm(eSTRING_ID eStringID, UnityAction funcConfirm)
    {
        this.m_funcConfirm = funcConfirm;

        base.SetDescription(eStringID);
    }
    */

    public void OnConfirmClicked()
    {
        this.m_funcConfirm.Invoke();

        base.closePopup();
    }
}
