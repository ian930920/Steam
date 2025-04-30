using UnityEngine.Events;

public class Popup_System_Confirm : Popup_System
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
