using TMPro;
using UnityEngine;

public class UI_SkillTurn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textTurn = null;
    [SerializeField] private TextColor_EffectType m_tcEffectType = null;

    public bool IsActive => this.gameObject.activeSelf;

    public void RefreshTurn(int turn, bool IsInfo = false)
    {
        if(IsInfo == false && turn < 1)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);
        this.m_textTurn.text = turn.ToString();
    }

    public void SetTextColor(TableData.TableStatus.eEFFECT_TYPE eType)
    {
        this.m_tcEffectType.SetTextColor(eType);
    }
}