using TMPro;
using UnityEngine;

public class TitleScene : BaseScene
{
    [SerializeField] private TextMeshProUGUI m_textVer = null;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        SceneManager.Instance.CurrScene.BaseHUD.RefreshUI();
    }

    public void GameStart()
    {
        //세션타입에 따라서 진행
        switch(UserDataManager.Instance.Session.CurrSessionType)
        {
            case eSESSION_TYPE.Battle:
            {
                SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Battle);
            }
            break;

            default:
            {
                SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Station);
            }
            break;
        }
    }

    public void CheckSession()
    {
        if(UserDataManager.Instance.Session.IsSessionStart == false) //진행중인 세션이 없다면
        {
            //세션 시작했다고 저장하고
            UserDataManager.Instance.StartSession();

            //시나리오 시작~!
            SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Scenario);
        }
        else if(UserDataManager.Instance.Session.IsScenarioWatch == false || UserDataManager.Instance.Summon.SummonCount < 3)
        {
            //시나리오 안봤거나 소환수 아직 덜 선택했다면

            //시나리오로 이동
            SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Scenario);
        }
        else
        {
            //그냥 시작
            this.GameStart();
        }
    }
}