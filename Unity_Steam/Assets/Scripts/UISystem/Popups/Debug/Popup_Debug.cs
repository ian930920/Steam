public class Popup_Debug : BasePopup
{
    public void OnRestartDataClicked()
    {
        UserDataManager.Instance.ResetManager();

        //타이틀씬으로
        if(SceneManager.Instance.CurrSceneID != SceneManager.eSCENE_ID.Title) SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);

        //팝업 닫기
        this.OnCloseClicked();
    }

    public void OnRestartSessionClicked()
    {
        UserDataManager.Instance.Session.FinishSession();

        //타이틀씬으로
        if(SceneManager.Instance.CurrSceneID != SceneManager.eSCENE_ID.Title) SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);

        //팝업 닫기
        this.OnCloseClicked();
    }

    public void OnAddAllSummonClicked()
    {
        //TODO 소환수 ID 추가해서 적용
        
        UserDataManager.Instance.Summon.Debug_AddSummon();

        //배틀씬이면 UI 갱신 
        if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle) SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();

        //팝업 닫기
        this.OnCloseClicked();
    }


    public void OnRemoveAllSummonClicked()
    {
        UserDataManager.Instance.Summon.Debug_RemoveSummon();

        //배틀씬이면 UI 갱신 
        if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle) SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();

        //팝업 닫기
        this.OnCloseClicked();
    }

    public void OnAddAllRuneClicked()
    {
        //TODO 룬 ID 추가해서 적용

        UserDataManager.Instance.Inventory.Debug_AddRune();

        //팝업 닫기
        this.OnCloseClicked();
    }

    public void OnRemoveAllRuneClicked()
    {
        UserDataManager.Instance.Inventory.Debug_RemoveRune();

        //팝업 닫기
        this.OnCloseClicked();
    }

    public void OnRemoveAllSummonRuneClicked()
    {
        UserDataManager.Instance.Inventory.Debug_RemoveRuneSummonID();
        UserDataManager.Instance.Summon.Debug_RemoveSummonRune();

        //배틀씬이면 UI 갱신 
        if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle) SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();

        //팝업 닫기
        this.OnCloseClicked();
    }

    public void OnGoTitleClicked()
    {
        SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);

        //팝업 닫기
        this.OnCloseClicked();
    }
}