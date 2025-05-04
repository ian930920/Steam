using UnityEngine;
using static EnhancedUI.EnhancedScroller.EnhancedScroller;

public class Popup_Summon : ScrollPopup
{
    [SerializeField] private GameObject m_gobjArrowLeft = null;
    [SerializeField] private GameObject m_gobjArrowRight = null;

    public void OnScrolled()
    {
        ScrollState eScrollState = this.GetScroller().GetHorizontalScrollState();
        this.m_gobjArrowLeft.SetActive(eScrollState == ScrollState.ScrollableBoth || eScrollState == ScrollState.ScrollableLeft);
        this.m_gobjArrowRight.SetActive(eScrollState == ScrollState.ScrollableBoth || eScrollState == ScrollState.ScrollableRight);
    }
}