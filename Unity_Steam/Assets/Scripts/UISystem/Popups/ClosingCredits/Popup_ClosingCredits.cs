using Febucci.UI;
using UnityEngine;
using UnityEngine.Events;

public class Popup_ClosingCredits : BasePopup
{
    [SerializeField] private TypewriterByCharacter m_text = null;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.m_text.ShowText("플레이해주셔서 감사합니다.");
        
        return this;
    }

    public override void OnCloseClicked()
    {
        //세션 끝남 ㅠ
        UserDataManager.Instance.Session.FinishSession();

        SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);

        base.OnCloseClicked();
    }
}