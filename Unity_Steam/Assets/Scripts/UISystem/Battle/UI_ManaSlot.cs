using UnityEngine;
using UnityEngine.UI;

public class UI_ManaSlot : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private Sprite m_sprUsed = null;
    [SerializeField] private Sprite m_sprUsable = null;

    public void RefreshSlot(bool isUsable)
    {
        this.m_imgIcon.sprite = isUsable ? this.m_sprUsable : this.m_sprUsed;
    }
}