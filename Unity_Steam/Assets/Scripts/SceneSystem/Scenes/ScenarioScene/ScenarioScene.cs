using UnityEngine;

public class ScenarioScene : BaseScene
{
    private void senarioFinish()
    {
        //저장하고
        ProjectManager.Instance.UserData.Session.WatchScenario();

        //소환수 선택
        ProjectManager.Instance.UI.PopupSystem.OpenPopup(ePOPUP_ID.SummonSelect);
    }

    public void OnSkipClicked()
    {
        this.senarioFinish();
    }
}