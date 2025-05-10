using UnityEngine;

public class ScenarioScene : BaseScene
{
    private void senarioFinish()
    {
        //저장하고
        UserDataManager.Instance.Session.WatchScenario();

        //저장하고
        UserDataManager.Instance.Session.SetSessionType(eSESSION_TYPE.Station);

        //역으로 이동~
        SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Station);
    }

    public void OnSkipClicked()
    {
        this.senarioFinish();
    }
}