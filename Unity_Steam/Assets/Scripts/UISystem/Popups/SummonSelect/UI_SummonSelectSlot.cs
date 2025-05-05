using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SummonSelectSlot : MonoBehaviour
{
    [SerializeField] private Image m_imgChar = null;
    [SerializeField] private TextMeshProUGUI m_textSummonName = null;

    [SerializeField] private TextMeshProUGUI m_textSkillName = null;
    [SerializeField] private TextMeshProUGUI m_textSkillDesc = null;
    [SerializeField] private TextMeshProUGUI m_textSkillTurn = null;

    [SerializeField] private UI_ManaSlot[] m_arrMana = null;
    [SerializeField] private UI_RuneGroup m_uiRuneGroup = null;

    [SerializeField] private GameObject m_gobjSelect = null;

    public void RefreshSlot(TableData.TableData_Summon dataSummon)
    {
        this.m_imgChar.sprite = ProjectManager.Instance.Table.Summon.GetSprite(dataSummon.tableID);
        this.m_textSummonName.text = ProjectManager.Instance.Table.String.GetString(dataSummon.strID);
        
        this.m_textSkillName.text = ProjectManager.Instance.Table.Skill.GetString_Title(dataSummon.skillID);
        this.m_textSkillDesc.text = ProjectManager.Instance.Table.Summon.GetString_SkillDesc(dataSummon.tableID);

        var dataSkill = ProjectManager.Instance.Table.Skill.GetData(dataSummon.skillID);
        this.m_textSkillTurn.text = dataSkill.cooldown.ToString();

        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            this.m_arrMana[i].gameObject.SetActive(dataSummon.cost > i);
        }

        this.m_uiRuneGroup.Init(dataSummon.tableID);
    }

    public void RefreshSlot(UserData_Summon.SummonData userData)
    {
        this.m_imgChar.sprite = ProjectManager.Instance.Table.Summon.GetSprite(userData.SummonID);

        var dataSummon = ProjectManager.Instance.Table.Summon.GetData(userData.SummonID);

        this.m_textSummonName.text = ProjectManager.Instance.Table.String.GetString(dataSummon.strID);
        
        this.m_textSkillName.text = ProjectManager.Instance.Table.Skill.GetString_Title(dataSummon.skillID);
        this.m_textSkillDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(dataSummon.skillID, userData.Damage);

        var dataSkill = ProjectManager.Instance.Table.Skill.GetData(dataSummon.skillID);
        this.m_textSkillTurn.text = dataSkill.cooldown.ToString();

        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            this.m_arrMana[i].gameObject.SetActive(dataSummon.cost > i);
        }
    }

    public void RefreshSelect(bool isSelect)
    {
        this.m_gobjSelect.SetActive(isSelect);
    }
}
