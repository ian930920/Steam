using TMPro;
using UnityEngine;

public class TextColor_EffectType : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text = null;

    [SerializeField] private Color m_colorDefault = Color.black;
    [SerializeField] private Color m_colorPositive = Color.green;
    [SerializeField] private Color m_colorNegative = Color.red;

    public void SetTextColor(TableData.TableStatus.eEFFECT_TYPE eType)
    {
        switch(eType)
        {
            case TableData.TableStatus.eEFFECT_TYPE.None:
            {
                this.m_text.color = this.m_colorDefault;
            }
            break;

            case TableData.TableStatus.eEFFECT_TYPE.Negative:
            {
                this.m_text.color = this.m_colorNegative;
            }
            break;

            case TableData.TableStatus.eEFFECT_TYPE.Positive:
            {
                this.m_text.color = this.m_colorPositive;
            }
            break;
        }
    }
}