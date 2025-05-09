using UnityEngine;
using TMPro;

public class Popup_System : BasePopup
{
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();
    }

    /*
    public void SetDescription(eSTRING_ID eStringID)
    {
        this.SetDescription(TableManager.Instance.String.GetString(eStringID));
    }
    */

    public void SetDescription(string strDesc)
    {
        this.m_textDesc.text = strDesc;
    }
}