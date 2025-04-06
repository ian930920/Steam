using System;
using UnityEngine;

/// <summary>
/// ???????????? ???????? ???? Manager or System ????
/// </summary>
public class ProjectManager : MonoBehaviour
{
    //TODO ?????????? Tag ???? : "ProjectManager" "Scene"
    //TODO ???????? Define?? ???? : DEBUG_LOG

#region Instance
    //Singleton Ojbect is only Managers
    static private string s_strGameObjectName = "ProjectManager";

    static readonly private Lazy<ProjectManager> s_instance = new Lazy<ProjectManager>(()=>
    {
        ProjectManager instance = FindAnyObjectByType(typeof(ProjectManager)) as ProjectManager;
        if(instance == null)
        {
            GameObject gobj = GameObject.FindGameObjectWithTag(s_strGameObjectName);
            if(gobj == null)
            {
                gobj = new GameObject(s_strGameObjectName);
                gobj.tag = s_strGameObjectName;
            }
            instance = gobj.AddComponent<ProjectManager>();
        }

        //???? ???? ?????? ???? ???????? ????
        if(instance.gameObject.scene.name != "DontDestroyOnLoad") DontDestroyOnLoad(instance.gameObject);

        return instance;
    });

    static public ProjectManager Instance { get { return s_instance.Value; } }
    #endregion

    [SerializeField] private DebugModeSystem m_systemDebugMode  = null;
    public DebugModeSystem DebugModeSystem => this.m_systemDebugMode;
    [SerializeField] private QueueActionSystem m_systemQueueAction  = null;
    public QueueActionSystem QueueActionSystem => this.m_systemQueueAction;

#region ?????? ???????? System
    public UIManager UI { private set; get; } = null;
    public SceneManager Scene { private set; get; } = null;
    public TableManager Table { private set; get; } = null;
    public TimeManager Time { private set; get; } = null;
    public ResourceManager Resource { private set; get; } = null;
    public ObjectPoolManager ObjectPool { private set; get; } = null;
    public UserDataManager UserData { private set; get; } = null;
#endregion

    private void Awake()
    {
        //?????? ????
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //????????
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //???? ?????? ???????? Manager or System
        this.Table = TableManager.Init(this.gameObject);
        this.UI = UIManager.Init(this.gameObject);
        this.Scene = SceneManager.Init(this.gameObject);
        this.Time = TimeManager.Init(this.gameObject);
        this.Resource = ResourceManager.Init(this.gameObject);
        this.ObjectPool = ObjectPoolManager.Init(this.gameObject);
        this.UserData = UserDataManager.Init(this.gameObject);
    }

    public void InitInTitleScene()
    {
        //?????? ???????? ???? ?? ?????????? Manager or System
        this.Table.LoadClientTables();
        this.Resource.LoadResByTable();
    }

    public void InitInMainScene()
    {
        //???? ???????? ???? ?? ?????????? Manager or System
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

		//???? ????????????
        //if(this.Scene.CurrScene == null || this.Scene.CurrSceneID != SceneManager.eSCENE_ID.Main) return;

        /*
        //???????????????? ????
		if(UserDataManager.Instance.Tutorial.IsActiveTutorial == true) return;

        //???? ????
        if(UIManager.Instance.PopupSystem.CurrPopup != null)
        {
        }
        */

        //TODO ????????
        //if(UIManager.Instance.PopupSystem.AutoClosePopup() == true) return;

        //TODO ????????????????? ????
        //UIManager.Instance.PopupSystem.OpenSystemConfirmPopup("?????????????????", this.QuitGame);
	}

    /// <summary>
    /// ???? ????
    /// </summary>
    public void QuitGame()
	{
        //?????? ????
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