using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScene : BaseScene
{
    [SerializeField] private TextMeshProUGUI m_textVer = null;

    public override void OnSceneStart()
    {
        ProjectManager.Instance.InitInTitleScene();

        base.OnSceneStart();

        ProjectManager.Instance.Scene.CurrScene.BaseHUD.RefreshUI();
    }
}