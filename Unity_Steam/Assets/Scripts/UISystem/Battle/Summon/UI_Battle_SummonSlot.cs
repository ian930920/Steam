using UnityEngine;
using UnityEngine.UI;

public class UI_Battle_SummonSlot : MonoBehaviour
{
    public Summon Summon { get; private set; } = null;

    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private Image m_imgSelect = null;
    [SerializeField] private Image m_imgInactive = null;

    [SerializeField] private UI_SkillTurn m_uiCooldown = null;
    [SerializeField] private UI_SkillTurn m_uiSkillTurn = null;
    [SerializeField] private UI_CostInfo m_uiCost = null;

    public bool IsCooldown => this.m_uiCooldown.IsActive;

    public void Init(Summon summon)
    {
        this.Summon = summon;

        this.SetSelect(false);
        this.gameObject.SetActive(true);

        this.m_imgIcon.sprite = TableManager.Instance.Summon.GetIcon(this.Summon.SummonID);

        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        bool isCoolTime = this.Summon.RemainTurn > 0;
        this.m_uiCooldown.RefreshTurn(this.Summon.RemainTurn);

        this.m_uiCost.Init(this.Summon.Cost);
        this.m_uiCost.SetTextColor(this.Summon.GetStatEffectType(Stat_Character.eTYPE.Mana));

        this.m_uiSkillTurn.RefreshTurn(this.Summon.Cooldown, true);
        this.m_uiSkillTurn.SetTextColor(this.Summon.GetAdditionalStatEffectType(Stat_Additional.eTYPE.Cooldown));
        
        bool isUsable = this.Summon.Cost <= SceneManager.Instance.GetCurrScene<BattleScene>().UnitUser.CurrStat.GetStat(Stat_Character.eTYPE.Mana);
        this.m_imgInactive.enabled = this.m_uiCooldown.IsActive == false && isUsable == false;
    }

    public void SetSelect(bool bSelect)
    {
        this.m_imgSelect.enabled = bSelect;
    }

    public void OpenSummonInfo()
    {
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.OpenSummonInfo(this.Summon.SummonID);
    }

    public void CloseSummonInfo()
    {
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.CloseSummonInfo();
    }
}