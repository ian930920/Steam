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
        //진행중인 세션이있다면
        if(UserDataManager.Instance.Session.IsSessionStart == true)
        {
            //TODO 지우고 진행할건지 물어보기
            //UIManager.Instance.PopupSystem.OpenSystemConfirmPopup("진행중인 게임이 있습니다.\n처음부터 시작하시겠습니까?", this);

            //그냥 시작
            this.GameStart();
        }
        else if(UserDataManager.Instance.Session.IsScenarioWatch == false)
        {
            //세션 시작했다고 저장하고
            UserDataManager.Instance.StartSession();

            //시나리오로 넘기기
            SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Scenario);
        }
        else
        {
            //그냥 시작
            this.GameStart();
        }
    }
}