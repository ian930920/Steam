using Febucci.UI;
using UnityEngine;

public class ScenarioScene : BaseScene
{
    [SerializeField] private TypewriterByCharacter m_textDesc = null;

    private TableData.TableEventCutscene.eID m_eCutsceneID = TableData.TableEventCutscene.eID.Start;
    private int m_nIdx = 0;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        if(UserDataManager.Instance.Session.IsEnding == true)
        {
            this.m_eCutsceneID = TableData.TableEventCutscene.eID.Ending;
            this.NextScenario();
        }
        else if(UserDataManager.Instance.Session.IsScenarioWatch == false)
        {
            this.m_eCutsceneID = TableData.TableEventCutscene.eID.Start;
            this.NextScenario();
        }
        else this.summonSelect();
    }

    public void NextScenario()
    {
        if(this.m_nIdx >= TableManager.Instance.EventCutscene.GetScenarioCount(this.m_eCutsceneID))
        {
            this.finishSenario();
            return;
        }

        this.m_textDesc.ShowText(TableManager.Instance.EventCutscene.GetStringByIdx(this.m_eCutsceneID, this.m_nIdx));

        this.m_nIdx++;
    }

    private void finishSenario()
    {
        if(UserDataManager.Instance.Session.IsEnding == true)
        {
            //엔딩 크레딧보여주고 처음으로 돌아가기
            UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.ClosingCredits);
        }
        else
        {
            //저장하고
            UserDataManager.Instance.Session.WatchScenario();

            //소환수 저장
            this.summonSelect();
        }
    }

    private void summonSelect()
    {
        //소환수 선택
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.SummonSelect);
    }

    public void OnSkipClicked()
    {
        this.finishSenario();
    }
}