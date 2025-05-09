using UnityEngine;

public class ProjectManager : BaseSingleton<ProjectManager>
{
    //TODO 프로젝트 Define에 추가 : DEBUG_LOG

    //TODO DebugSystem에 옮기기
    [SerializeField] private DebugModeSystem m_systemDebugMode  = null;
    public DebugModeSystem DebugModeSystem => this.m_systemDebugMode;
    public QueueActionSystem QueueActionSystem { get; private set; } = new QueueActionSystem();

    public override void Initialize()
    {
        if(base.IsInitialized == true) return;

        //프레임 고정
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //슬립모드
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //테이블 로드
        TableManager.Instance.LoadClientTables();

        base.IsInitialized = true;
    }

    public void InitManagerByScene(SceneManager.eSCENE_ID eSceneID)
    {
        switch(eSceneID)
        {
            case SceneManager.eSCENE_ID.Title:
            {

            }
            break;

            case SceneManager.eSCENE_ID.Station:
            {
                //전투씬에서만 쓰는 objectPool 세팅
                ObjectPoolManager.Instance.InitObjectPool();
            }
            break;

            case SceneManager.eSCENE_ID.Battle:
            {
                //전투씬에서만 쓰는 objectPool 세팅
                ObjectPoolManager.Instance.InitObjectPool();
            }
            break;

            case SceneManager.eSCENE_ID.Scenario:
            break;
        }
    }

    private void Update()
    {
#if !UNITY_IOS
        this.checkEscapeKey();
#endif
    }

    private void checkEscapeKey()
	{
		if(Input.GetKeyUp(KeyCode.Escape) == false) return;

        //팝업닫기
        if(UIManager.Instance.PopupSystem.AutoClosePopup() == true) return;

        //종료하시겠습니까? 팝업
        UIManager.Instance.PopupSystem.OpenSystemConfirmPopup("종료하시겠습니까?", this.QuitGame);
	}

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void QuitGame()
	{
        //데이터 저장
        //UserDataManager.Instance.SaveData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}

#region OnApplicationFocus
    private void OnApplicationFocus(bool bFocus)
    {
        //if(this.Scene.CurrSceneID != SceneManager.eSCENE_ID.Main) return;

        if(bFocus == false) this.onAppFocusOut();
        else this.onAppFocusIn();
    }

    private void onAppFocusOut()
    {
        this.Log($"onAppFocusOut");
    }

    private void onAppFocusIn()
    {
        this.Log($"onAppFocusIn");
    }
    #endregion

#region Log
    public void Log(string strLog)
    {
#if UNITY_EDITOR && DEBUG_LOG
        Debug.Log(strLog);
#else
#if !DEBUG_LOG
        if(this.DebugModeSystem.IsDebugMode == false) return;
#endif
        Debug.Log(strLog);
#endif
    }

    public void Log(string strLog, UnityEngine.Object obj)
    {
#if UNITY_EDITOR && DEBUG_LOG
        Debug.Log(strLog, obj);
#else
#if !DEBUG_LOG
        if(this.DebugModeSystem.IsDebugMode == false) return;
#endif
        Debug.Log(strLog, obj);
#endif
    }

    public void LogWarning(string strLog)
    {
#if UNITY_EDITOR && DEBUG_LOG
        Debug.LogWarning(strLog);
#else
#if !DEBUG_LOG
        if(this.DebugModeSystem.IsDebugMode == false) return;
#endif
        Debug.LogWarning(strLog);
#endif
    }

    public void LogError(string strLog)
    {
#if UNITY_EDITOR && DEBUG_LOG
        Debug.LogError(strLog);
#else
        if(this.DebugModeSystem.IsDebugMode == false) return;

        Debug.LogError(strLog);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#endif
    }
    #endregion
}