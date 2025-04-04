using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
public class UI_CanvasGroup : MonoBehaviour
{
    private CanvasGroup m_anvasGroup = null;
    public bool Active
    {
        get
        {
            if(this.m_anvasGroup == null) this.m_anvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            
            return this.m_anvasGroup.blocksRaycasts;
        }
        set
        {
            if(this.m_anvasGroup == null) this.m_anvasGroup = this.gameObject.GetComponent<CanvasGroup>();

            this.m_anvasGroup.alpha = value ? 1 : 0;
            this.m_anvasGroup.blocksRaycasts = value;
        }
    }
}

public interface iCanvasGroup
{
    public bool Active { get; set; }
}