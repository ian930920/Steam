using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPLinkHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private TextMeshProUGUI m_tmpText = null;

    private int lastLinkIndex = -1;

    public System.Action<string, Vector3> onHoverEnter;
    public System.Action onHoverExit;

    void Awake()
    {
        this.m_tmpText = GetComponent<TextMeshProUGUI>();

        this.onHoverEnter = (statusID, vecPos) =>
        {
            ProjectManager.Instance.UI.PopupSystem.OpenStatusPopup(uint.Parse(statusID), vecPos);
        };

        this.onHoverExit = () =>
        {
            ProjectManager.Instance.UI.PopupSystem.ClosePopup(ePOPUP_ID.StatusInfo);
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.CheckLinkHover(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lastLinkIndex = -1;
        onHoverExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        this.CheckLinkHover(eventData);
    }

    private void CheckLinkHover(PointerEventData eventData)
    {
        Vector2 localMousePos = eventData.position;

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_tmpText, localMousePos, null);

        if(linkIndex != -1 && linkIndex != lastLinkIndex)
        {
            lastLinkIndex = linkIndex;
            string id = this.m_tmpText.textInfo.linkInfo[linkIndex].GetLinkID();
            onHoverEnter?.Invoke(id, localMousePos);
        }
        else if (linkIndex == -1 && lastLinkIndex != -1)
        {
            lastLinkIndex = -1;
            onHoverExit?.Invoke();
        }
    }
}
