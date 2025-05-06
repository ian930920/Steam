using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Battle_SummonSlot : MonoBehaviour
{
    public Summon Summon { get; private set; } = null;

    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private Image m_imgSelect = null;
    [SerializeField] private Image m_imgCooldown = null;

    [SerializeField] private UI_SkillTurn m_uiSkillTurn = null;

    public void Init(Summon summon)
    {
        this.Summon = summon;

        this.SetSelect(false);
        this.gameObject.SetActive(true);

        this.m_imgIcon.sprite = ProjectManager.Instance.Table.Summon.GetIcon(this.Summon.SummonID);
        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        bool isCoolTime = this.Summon.RemainTurn > 0;
        this.m_uiSkillTurn.RefreshTurn(this.Summon.RemainTurn);
        this.m_imgCooldown.enabled = isCoolTime;
    }

    public void SetSelect(bool bSelect)
    {
        this.m_imgSelect.enabled = bSelect;
    }
}