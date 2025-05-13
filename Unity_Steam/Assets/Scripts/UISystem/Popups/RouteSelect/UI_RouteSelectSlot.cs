using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RouteSelectSlot : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textName = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    [SerializeField] private GameObject m_gobjSelect = null;
    [SerializeField] private UI_LevelInfo m_uiLevelInfo = null;

    private uint m_routeID = 0;

    public void Init(uint routeID)
    {
        this.m_routeID = routeID;

        this.m_imgIcon.sprite = TableManager.Instance.Route.GetIcon(this.m_routeID);
        this.m_textName.text = TableManager.Instance.Route.GetString(this.m_routeID);
        this.m_textDesc.text = TableManager.Instance.Route.GetString(this.m_routeID, TableData.TableString.eTYPE.Description);

        var nLevel = TableManager.Instance.Route.GetData(this.m_routeID).level;
        this.m_uiLevelInfo.Refresh(nLevel);
    }

    public void RefreshSelect(bool isSelect)
    {
        this.m_gobjSelect.SetActive(isSelect);
    }
}