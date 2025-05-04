using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollItem_Summon : ScrollItem<UserData_User.SummonData>
{
    [SerializeField] private Image m_imgChar = null;
    [SerializeField] private TextMeshProUGUI m_textSummonName = null;

    [SerializeField] private TextMeshProUGUI m_textSkillName = null;
    [SerializeField] private TextMeshProUGUI m_textSkillDesc = null;
    [SerializeField] private TextMeshProUGUI m_textSkillTurn = null;

    [SerializeField] private UI_ManaSlot[] m_arrMana = null;

    public override void RefreshCellView()
    {
        base.RefreshCellView();

        this.m_imgChar.sprite = ProjectManager.Instance.Table.Summon.GetSprite(base.Data.SummonID);

        var dataSummon = ProjectManager.Instance.Table.Summon.GetData(base.Data.SummonID);

        this.m_textSummonName.text = ProjectManager.Instance.Table.String.GetString(dataSummon.strID);
        
        this.m_textSkillName.text = ProjectManager.Instance.Table.Skill.GetString_Title(dataSummon.skillID);
        this.m_textSkillDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(dataSummon.skillID, base.Data.Damage);

        var dataSkill = ProjectManager.Instance.Table.Skill.GetData(dataSummon.skillID);
        this.m_textSkillTurn.text = dataSkill.cooldown.ToString();

        for(int i = 0, nMax = this.m_arrMana.Length; i < nMax; ++i)
        {
            this.m_arrMana[i].gameObject.SetActive(dataSummon.cost > i);
        }
    }
}