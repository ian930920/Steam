using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD_Title : BaseHUD
{
    [SerializeField] private TextMeshProUGUI m_textVer = null;

    public override void RefreshUI()
    {
        this.m_textVer.text = Application.version;
    }

    public void OnGameStartClicked()
    {
        ProjectManager.Instance.Scene.GetCurrScene<TitleScene>().CheckSession();
    }

    public void OnOptionClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("TODO 옵션 팝업");
    }

    public void OnQuitClicked()
    {
        ProjectManager.Instance.QuitGame();
    }
}