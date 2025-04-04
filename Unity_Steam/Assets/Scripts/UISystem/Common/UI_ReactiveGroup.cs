using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ReactiveGroup : MonoBehaviour
{
    public enum eMODE
    {
        Default = 0,
        Build,
        End
    }

    [SerializeField] private UI_CanvasGroup[] m_arrCanvasGroup = null;

    public Transform GetTransformByMode(eMODE eMode)
    {
        return this.m_arrCanvasGroup[(int)eMode].transform;
    }

    public void SetMode(eMODE eMode)
    {
        /*
        if(CustomSceneManager.Instance.CurrSceneID != CustomSceneManager.eSCENE_ID.Main) return;

        switch(eMode)
        {
            case eMODE.Default:
            {
                //HUD 켜고
                UIManager.Instance.HUD.Visible = true;
            }
            break;
            case eMODE.Build:
            {
                //HUD 끄고
                UIManager.Instance.HUD.Visible = false;
            }
            break;
        }
        */

        for(int i = 0, nMax = (int)eMODE.End; i < nMax; ++i)
        {
            this.m_arrCanvasGroup[i].Active = (int)eMode == i;
        }
    }
}
