using UnityEngine;

public class ScenarioScene : BaseScene
{
    private void senarioFinish()
    {
        //저장하고
        UserDataManager.Instance.Session.WatchScenario();

        //소환수 선택
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.SummonSelect);
    }

    public void OnSkipClicked()
    {
        this.senarioFinish();
    }
}