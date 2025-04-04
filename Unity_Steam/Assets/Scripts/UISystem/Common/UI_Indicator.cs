using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Indicator : BasePreloadUI<UI_Indicator>, iCanvasGroup
{
    public static readonly string STR_PATH_INDICATOR = "Prefabs/UI/Indicator";

    [SerializeField] private Canvas m_canvas = null;
    [SerializeField] private CanvasGroup m_uiCanvasGroup = null;

    static private float TIME_WAIT_INDICATOR = 1.0f;

    private int m_nCounter = 0;

    public bool Active
    {
        get => this.m_uiCanvasGroup.blocksRaycasts;
        set
        {
            if(value == true)
            {
                this.m_nCounter++;

                if(this.m_canvas.enabled == false) this.m_canvas.enabled = true;

                StartCoroutine("coActiveUI");
            }
            else
            {
                this.m_nCounter--;
                if(this.m_nCounter < 0) this.m_nCounter = 0;

                if(this.m_nCounter > 0) return;

                StopCoroutine("coActiveUI");
                this.m_uiCanvasGroup.blocksRaycasts = false;
                this.m_uiCanvasGroup.alpha = 0;

                this.m_canvas.enabled = false;
            }
        }
    }

    protected override void init()
    {
        //일단 비활성화 시켜놓기
        this.Active = false;
    }

    private IEnumerator coActiveUI()
    {
        this.m_uiCanvasGroup.blocksRaycasts = true;

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(TIME_WAIT_INDICATOR);

        this.m_uiCanvasGroup.alpha = 1;
    }
}