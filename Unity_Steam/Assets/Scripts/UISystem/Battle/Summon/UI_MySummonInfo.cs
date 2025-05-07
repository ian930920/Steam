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
        var dataSummon = ProjectManager.Instance.Table.Summon.GetData(summonID);
        var userSummon = ProjectManager.Instance.UserData.Summon.GetSummon(summonID);

        this.m_imgChar.sprite = ProjectManager.Instance.Table.Summon.GetSprite(summonID);

        this.m_textSummonName.text = ProjectManager.Instance.Table.String.GetString(dataSummon.strID);
        
        this.m_textSkillName.text = ProjectManager.Instance.Table.Skill.GetString_Title(dataSummon.skillID);
        this.m_textSkillDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(dataSummon.skillID, userSummon.Damage);

        this.m_uiSkillTurn.RefreshTurn(userSummon.Cooldown, true);
        this.m_uiCostInfo.Init(userSummon.StatDefault.GetStat(Stat_Character.eTYPE.Mana));
        this.m_uiRuneGroup.Init(dataSummon.tableID, false);
    }
}