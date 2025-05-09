using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatusSlot : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textTurn = null;

    private uint m_statusID = 0;

    public void Init(uint statusID)
    {
        this.gameObject.SetActive(true);

        this.m_statusID = statusID;
        this.m_imgIcon.sprite = TableManager.Instance.Status.GetIcon(statusID);
    }

    /// <summary>
    /// 상태이상 UI 업데이트
    /// </summary>
    /// <param name="turn"></param>
    /// <returns>지워지는지 여부</returns>
    public bool UpdateTurn(int turn)
    {
        this.gameObject.SetActive(turn != 0);

        this.m_textTurn.text = turn.ToString();

        return !this.gameObject.activeSelf;
    }

    public void OnClicked()
    {
        UIManager.Instance.PopupSystem.OpenStatusInfoPopup(this.m_statusID, this.transform.position);
    }

    public void OnClickEnd()
    {
        UIManager.Instance.PopupSystem.ClosePopup(ePOPUP_ID.StatusInfo);
    }
}