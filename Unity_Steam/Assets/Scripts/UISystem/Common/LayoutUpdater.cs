using UnityEngine;
using UnityEngine.UI;

public class LayoutUpdater : MonoBehaviour
{
    [SerializeField] private LayoutGroup m_layoutGroup = null;
    [SerializeField] private RectTransform m_rt = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.m_layoutGroup.enabled = false;
    }

    public void Refresh()
    {
        this.m_layoutGroup.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_rt);
    }
}