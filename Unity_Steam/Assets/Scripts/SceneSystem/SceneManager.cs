using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : BaseManager<SceneManager>
{
    public enum eSCENE_ID
    {
        Title,
        //Main,
        Battle,
    }

    private string[] m_arrSceneName =
	{
        "TitleScene",
        //"MainScene",
        "BattleScene",
    };

	private eSCENE_ID m_reserveSceneID { get; set; }
	public eSCENE_ID CurrSceneID => this.CurrScene.m_eSceneID;
    public BaseScene CurrScene { get; private set; } = null;
	public T GetCurrScene<T>() where T : BaseScene
	{
		return this.CurrScene as T;
	}

    private UI_SceneFade m_fadeSystem = null;

    protected override void init()
    {
        this.m_fadeSystem = UI_SceneFade.Init(this.transform, UI_SceneFade.STR_PATH_FADE);
        this.m_fadeSystem.UpdateCanvasScaler();
    }

    public void SetCurrScene(BaseScene crrScene)
    {
        this.CurrScene = crrScene;
    }

    public void ChangeScene(eSCENE_ID eSceneID)
    {
		this.m_reserveSceneID = eSceneID;

        StartCoroutine("coChangeScene");
    }

    private IEnumerator coChangeScene()
    {
        this.CurrScene.OnSceneEnd();

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(this.CurrScene.FadeEndDuration);

        UnityEngine.SceneManagement.SceneManager.LoadScene(this.m_arrSceneName[(int)this.m_reserveSceneID]);

        // ? ?? ? ? ?? Canvas ??? ???
        this.m_fadeSystem.UpdateCanvasScaler();
    }

    public void FadeStart(UI_SceneFade.stFadeInfo fadeInfo)
    {
        this.m_fadeSystem.FadeStart(fadeInfo);
    }
}