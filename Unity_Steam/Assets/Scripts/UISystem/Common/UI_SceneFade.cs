using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_SceneFade : BasePreloadUI<UI_SceneFade>
{
    public static readonly string STR_PATH_FADE = "Prefabs/UI/Fade";

	public enum eFADE_TYPE
    {
		In,
		Out
    }

	public struct stFadeInfo
	{
		public eFADE_TYPE eFadeType;
		public float fDuration;
		public Color color;
		public UnityAction onFinish;

		public stFadeInfo(eFADE_TYPE eFadeType, float fDuration, Color color, UnityAction onFinish)
		{
			this.eFadeType = eFadeType;
			this.fDuration = fDuration;
			this.color = color;
			this.onFinish = onFinish;
		}
	}

    [SerializeField] private Canvas m_canvas = null;
    [SerializeField] private Image m_imgFade = null;

    public void FadeStart(stFadeInfo fadeInfo)
	{
		if(this.m_canvas.enabled == false) this.m_canvas.enabled = true;

		StopCoroutine("coFadeStart");
		StartCoroutine("coFadeStart", fadeInfo);
	}

	private IEnumerator coFadeStart(stFadeInfo fadeInfo)
	{
		float fDuration = 0.0f;
		float fFadeDuration = 1 / fadeInfo.fDuration;
		while(fDuration < fadeInfo.fDuration)
		{
			float fAlpha = fDuration * fFadeDuration;
			if(fadeInfo.eFadeType == eFADE_TYPE.Out) fAlpha = 1 - fAlpha;
			this.m_imgFade.color = new Color(fadeInfo.color.r, fadeInfo.color.g, fadeInfo.color.b, fAlpha);

			yield return null;

			fDuration += Time.fixedDeltaTime;
		}

		if(fadeInfo.onFinish != null) fadeInfo.onFinish();

		if(fadeInfo.eFadeType == eFADE_TYPE.Out) this.m_canvas.enabled = false;
	}
}
