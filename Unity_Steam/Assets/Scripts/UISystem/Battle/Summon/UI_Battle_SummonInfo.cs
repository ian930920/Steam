using UnityEngine;
using TMPro;

public class UI_Battle_SummonInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;

    [SerializeField] private UI_CostInfo m_uiCostInfo = null;
    [SerializeField] private UI_RuneGroup m_uiRuneGroup = null;
    [SerializeField] private UI_SkillTurn m_uiSkillTurn = null;

    public void RefreshUI(Summon summon)
    {
        //설명
        this.m_textTitle.text = ProjectManager.Instance.Table.Skill.GetString_Title(summon.Skill.SkillID);
        this.m_textDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(summon.Skill.SkillID, summon.Damage);

        //쿨타임
        this.m_uiSkillTurn.RefreshTurn(ProjectManager.Instance.Table.Skill.GetData(summon.Skill.SkillID).cooldown, true);

        //마나 비용
        this.m_uiCostInfo.Init(summon.Cost);

        //룬 표기
        this.m_uiRuneGroup.Init(summon.SummonID);
    }
}