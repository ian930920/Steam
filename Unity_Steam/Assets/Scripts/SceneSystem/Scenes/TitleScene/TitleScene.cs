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
        //TODO Delete
        //UserDataManager.Instance.Inventory.AddItem(new stItem(TableData.TableItem.eID.Ticket, 10));
        
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
        if(UserDataManager.Instance.Summon.SummonCount < 3) //진행중인 세션이 없다면
        {
            //세션 시작했다고 저장하고
            UserDataManager.Instance.StartSession();

            //소환수 선택
            UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.SummonSelect);
        }
        else if(UserDataManager.Instance.Session.IsSessionStart == true) //진행중인 세션이있다면
        {
            //TODO 지우고 진행할건지 물어보기
            //UIManager.Instance.PopupSystem.OpenSystemConfirmPopup("진행중인 게임이 있습니다.\n처음부터 시작하시겠습니까?", this);

            //그냥 시작
            this.GameStart();
        }
        else
        {
            //그냥 시작
            this.GameStart();
        }
    }
}