using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_RuneEquip : ScrollPopup
{
    [SerializeField] private Image m_imgChar = null;
    [SerializeField] private TextMeshProUGUI m_textSummonName = null;

    [SerializeField] private TextMeshProUGUI m_textSkillName = null;
    [SerializeField] private TextMeshProUGUI m_textSkillDesc = null;

    [SerializeField] private UI_SkillTurn m_uiSkillTurn = null;
    [SerializeField] private UI_RuneGroup m_uiRuneGroup = null;
    [SerializeField] private UI_CostInfo m_uiCostInfo = null;

    private UserData_Summon.SummonData m_summonData = null;
    public uint SummonID => this.m_summonData.SummonID;
    private uint m_equipUniqueRuneID = 0;
    private uint m_equipRuneID = 0;

    public void SetSummon(uint summonID)
    {
        this.m_summonData = new UserData_Summon.SummonData(ProjectManager.Instance.UserData.Summon.GetSummon(summonID));

        this.setDefaultInfo();

        this.refreshStatInfo();

        this.RefreshScroller();
    }

    private void setDefaultInfo()
    {
        uint summonID = this.m_summonData.SummonID;
        this.m_imgChar.sprite = ProjectManager.Instance.Table.Summon.GetSprite(summonID);

        var dataSummon = ProjectManager.Instance.Table.Summon.GetData(summonID);

        this.m_textSummonName.text = ProjectManager.Instance.Table.String.GetString(dataSummon.strID);
        
        this.m_textSkillName.text = ProjectManager.Instance.Table.Skill.GetString_Title(dataSummon.skillID);
    }

    private void refreshStatInfo()
    {
        this.m_textSkillDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(this.m_summonData.SkillID, this.m_summonData.Damage);
        this.m_uiSkillTurn.RefreshTurn((ulong)this.m_summonData.StatAdditional.GetStat(Stat_Additional.eTYPE.Cooldown), true);

        this.m_uiCostInfo.Init(this.m_summonData.StatDefault.GetStat(Stat_Character.eTYPE.Mana));
        this.m_uiRuneGroup.Init(this.m_summonData.SummonID);
    }

    public void EquipRunePreview(uint uniqueRuneID, uint runeID)
    {
        //이전에 장착한 룬 삭제
        this.m_summonData.RemoveRune(this.m_equipRuneID);

        //지금 룬 저장
        this.m_equipUniqueRuneID = uniqueRuneID;
        this.m_equipRuneID = runeID;

        //룬 장착했다고,,,,,,, 예상
        this.m_summonData.AddRune(this.m_equipRuneID);

        //갱신
        this.refreshStatInfo();

        this.RefreshScroller();
    }

    public bool IsSelectedRune(uint uniqueRuneID)
    {
        return this.m_equipUniqueRuneID == uniqueRuneID;
    }

    public void OnEquipClicked()
    {
        ProjectManager.Instance.UserData.EquipRune(this.SummonID, this.m_equipUniqueRuneID);

        this.RefreshScroller();
    }
}
