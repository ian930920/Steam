using UnityEngine;
using UnityEngine.UI;

public class LayoutUpdater : MonoBehaviour
{
    private LayoutGroup m_layoutGroup = null;
    private RectTransform m_rt = null;
    private ContentSizeFitter m_contentSizeFitter = null;

    private void Awake()
    {
        this.m_layoutGroup = this.GetComponent<LayoutGroup>();
        this.m_rt = this.GetComponent<RectTransform>();
        this.m_contentSizeFitter = this.GetComponent<ContentSizeFitter>();
    }

    private void Start()
    {
        if(this.m_contentSizeFitter != null) this.m_contentSizeFitter.enabled = false;
        this.m_layoutGroup.enabled = false;
    }

    public void Refresh()
    {
        this.m_layoutGroup.enabled = true;
        if(this.m_contentSizeFitter != null) this.m_contentSizeFitter.enabled = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_rt);

        if(this.m_contentSizeFitter != null) this.m_contentSizeFitter.enabled = false;
        this.m_layoutGroup.enabled = false;
    }
}