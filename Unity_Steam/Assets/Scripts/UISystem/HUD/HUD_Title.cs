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
        SceneManager.Instance.GetCurrScene<TitleScene>().CheckSession();
    }

    public void OnOptionClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("TODO 옵션 팝업");
    }

    public void OnDebugClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.Debug);
    }

    public void OnQuitClicked()
    {
        ProjectManager.Instance.QuitGame();
    }
}