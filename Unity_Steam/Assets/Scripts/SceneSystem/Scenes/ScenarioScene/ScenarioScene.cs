using Febucci.UI;
using UnityEngine;

public class ScenarioScene : BaseScene
{
    [SerializeField] private TypewriterByCharacter m_textDesc = null;

    private int m_nIdx = 0;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        this.NextScenario();
    }

    public void NextScenario()
    {
        if(this.m_nIdx >= TableManager.Instance.EventCutscene.GetScenarioCount(TableData.TableEventCutscene.eID.Start))
        {
            this.finishSenario();
            return;
        }

        this.m_textDesc.ShowText(TableManager.Instance.EventCutscene.GetStringByIdx(TableData.TableEventCutscene.eID.Start, this.m_nIdx));

        this.m_nIdx++;
    }

    private void finishSenario()
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
        this.finishSenario();
    }
}