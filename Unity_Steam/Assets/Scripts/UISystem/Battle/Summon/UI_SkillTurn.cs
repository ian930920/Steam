using TMPro;
using UnityEngine;

public class UI_SkillTurn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textTurn = null;

    public void RefreshTurn(ulong turn, bool IsInfo = false)
    {
        if(IsInfo == false && turn < 1)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if(this.gameObject.activeSelf == false) this.gameObject.SetActive(true);
        this.m_textTurn.text = turn.ToString();
    }
}