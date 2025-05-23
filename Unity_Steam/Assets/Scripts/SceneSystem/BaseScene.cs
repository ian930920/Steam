using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class BaseScene : MonoBehaviour
{
	[SerializeField] public SceneManager.eSCENE_ID m_eSceneID = SceneManager.eSCENE_ID.Title;

	[Header("FadeInfo")][Space(5)]
	[SerializeField] private float m_fStartDuration = 0.3f;
	[SerializeField] private Color m_colorStart = Color.black;

	[SerializeField] private float m_fEndDuration = 0.3f;
	[SerializeField] private Color m_colorEnd = Color.black;
	public float FadeEndDuration => this.m_fEndDuration;

	[Header("HUD")][Space(5)]
	[SerializeField] private BaseHUD m_hud = null;
	public BaseHUD BaseHUD => this.m_hud;
	[SerializeField] private Transform m_transRootObjectPool = null;
	public Transform TransRootObjectPool => this.m_transRootObjectPool;

	private void Awake()
	{
		this.gameObject.tag = "Scene";
		SceneManager.Instance.SetCurrScene(this);
	}

    private void Start()
    {
        this.OnSceneStart();
    }

	/// <summary>
	/// fade 연출이 끝난 해당씬의 시각적 시작 지점
	/// </summary>
    virtual protected void onFadeEnd() { }

	virtual public void OnSceneStart()
	{
		SceneManager.Instance.FadeStart(new UI_SceneFade.stFadeInfo(UI_SceneFade.eFADE_TYPE.Out, this.m_fStartDuration, this.m_colorStart, this.onFadeEnd));

		//hud
		this.m_hud?.Init();

		ProjectManager.Instance.InitManagerByScene(this.m_eSceneID);
	}

	virtual public void OnSceneEnd()
	{
		SceneManager.Instance.FadeStart(new UI_SceneFade.stFadeInfo(UI_SceneFade.eFADE_TYPE.In, this.m_fEndDuration, this.m_colorEnd, null));
	}
}