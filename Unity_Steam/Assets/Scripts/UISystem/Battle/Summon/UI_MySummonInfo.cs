using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MySummonInfo : MonoBehaviour
{
    [SerializeField] private Image m_imgChar = null;
    [SerializeField] private TextMeshProUGUI m_textSummonName = null;

    [SerializeField] private TextMeshProUGUI m_textSkillName = null;
    [SerializeField] private TextMeshProUGUI m_textSkillDesc = null;

    [SerializeField] private UI_SkillTurn m_uiSkillTurn = null;
    [SerializeField] private UI_RuneGroup m_uiRuneGroup = null;
    [SerializeField] private UI_CostInfo m_uiCostInfo = null;

    public void Refresh(uint summonID)
    {
        var dataSummon = TableManager.Instance.Summon.GetData(summonID);
        var userSummon = UserDataManager.Instance.Summon.GetSummon(summonID);
        this.m_imgChar.sprite = TableManager.Instance.Summon.GetIcon(summonID);

        this.m_textSummonName.text = TableManager.Instance.String.GetString(dataSummon.strID);
        
        this.m_textSkillName.text = TableManager.Instance.Skill.GetString_Title(dataSummon.skillID);

        var damage = userSummon.Damage;
        if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle) damage = SceneManager.Instance.GetCurrScene<BattleScene>().User_GetSummonDamage(summonID);
        this.m_textSkillDesc.text = TableManager.Instance.Skill.GetString_Desc(dataSummon.skillID, damage);

        this.m_uiSkillTurn.RefreshTurn(userSummon.Cooldown, true);
        this.m_uiSkillTurn.SetTextColor(userSummon.StatAdditional.GetEffectType(Stat_Additional.eTYPE.Cooldown));

        this.m_uiCostInfo.Init(userSummon.StatDefault.GetStat(Stat_Character.eTYPE.Mana));
        this.m_uiCostInfo.SetTextColor(userSummon.StatDefault.GetEffectType(Stat_Character.eTYPE.Mana));

        this.m_uiRuneGroup.Init(dataSummon.tableID, false);
    }
}