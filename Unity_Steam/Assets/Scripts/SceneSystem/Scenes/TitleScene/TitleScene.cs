using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override void OnSceneStart()
    {
        ProjectManager.Instance.InitInTitleScene();

        base.OnSceneStart();

        this.OnGameStartClicked();
    }

    public void OnGameStartClicked()
    {
        ProjectManager.Instance.Scene.ChangeScene(SceneManager.eSCENE_ID.Battle);
    }
}