using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_RuneEquip : ScrollPopup
{
    private enum eBTN
    {
        Equip,
        Unequip,
        Change
    }

    [SerializeField] private Image m_imgChar = null;
    [SerializeField] private TextMeshProUGUI m_textSummonName = null;

    [SerializeField] private TextMeshProUGUI m_textSkillName = null;
    [SerializeField] private TextMeshProUGUI m_textSkillDesc = null;

    [SerializeField] private TextMeshProUGUI m_textRuneName = null;
    [SerializeField] private TextMeshProUGUI m_textRuneDesc = null;

    [SerializeField] private UI_SkillTurn m_uiSkillTurn = null;
    [SerializeField] private UI_RuneGroup m_uiRuneGroup = null;
    [SerializeField] private UI_CostInfo m_uiCostInfo = null;

    [SerializeField] private GameObject[] m_arrBtn = null;
    [SerializeField] private Button_ChangeState m_csbtnChange = null;

    [SerializeField] private UI_Scroll_Rune m_scrollUI = null;

    private UserData_Summon.MySummon m_summonData = null;
    public uint SummonID
    {
        get
        {
            if(this.m_summonData == null) return 0;
            return this.m_summonData.SummonID;
        }
    }

    private Item_Rune m_currRune = null;

    public void SetSummon(uint summonID)
    {
        this.m_summonData = new UserData_Summon.MySummon(ProjectManager.Instance.UserData.Summon.GetSummon(summonID));

        this.setDefaultInfo();

        this.refreshStatInfo();

        this.m_scrollUI.UpdateData();
        this.m_currRune = this.m_scrollUI.GetFirstRune();
        this.m_scrollUI.ResetScroller();
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

        var defaultCooldown = ProjectManager.Instance.Table.Skill.GetData(this.m_summonData.SkillID).cooldown;
        var addCooldown = this.m_summonData.StatAdditional.GetStat(Stat_Additional.eTYPE.Cooldown);
        this.m_uiSkillTurn.RefreshTurn((uint)(Mathf.Clamp(defaultCooldown - addCooldown, 0, defaultCooldown)), true);

        this.m_uiCostInfo.Init(this.m_summonData.StatDefault.GetStat(Stat_Character.eTYPE.Mana));
        this.m_uiRuneGroup.Init(this.m_summonData.SummonID, false);
    }

    private void refreshRuneInfo()
    {
        this.m_textRuneName.text = ProjectManager.Instance.Table.Rune.GetString_Title(this.m_currRune.RuneID);
        this.m_textRuneDesc.text = ProjectManager.Instance.Table.Rune.GetString_Desc(this.m_currRune.RuneID);

        //장착 버튼
        this.refreshBtn();
    }

    private void refreshBtn()
    {
        //장착중인지 확인
        var rune = ProjectManager.Instance.UserData.Inventory.GetRune(this.m_currRune.UniqueRuneID);
        this.m_arrBtn[(int)eBTN.Equip].SetActive(rune.SummonID == 0);
        this.m_arrBtn[(int)eBTN.Unequip].SetActive(rune.SummonID != 0);
        this.m_arrBtn[(int)eBTN.Change].SetActive(rune.SummonID != 0);
        this.m_csbtnChange.RefreshActive(rune.SummonID != this.SummonID);
    }

    private void refreshSummonData()
    {
        this.m_summonData = new UserData_Summon.MySummon(ProjectManager.Instance.UserData.Summon.GetSummon(this.SummonID));
        this.refreshStatInfo();

        this.m_uiRuneGroup.Init(this.m_summonData.SummonID, false);

        this.ResetScroller();

        //장착 버튼
        this.refreshBtn();
    }

    public void EquipRunePreview(Item_Rune rune)
    {
        if(this.m_currRune != null && this.m_currRune.UniqueRuneID == rune.UniqueRuneID) return;

        var orgSummon = ProjectManager.Instance.UserData.Summon.GetSummon(this.SummonID);

        //이전에 장착한 룬 삭제
        if(this.m_currRune != null && orgSummon.ListRune.Any(data => data.UniqueRuneID == this.m_currRune.UniqueRuneID) == false) this.m_summonData.RemoveRune(this.m_currRune);

        //지금 룬 저장
        this.m_currRune = rune;

        //룬 장착했다고,,,,,,, 예상
        if(orgSummon.ListRune.Any(data => data.UniqueRuneID == this.m_currRune.UniqueRuneID) == false)this.m_summonData.AddRune(this.m_currRune);

        //갱신
        this.refreshStatInfo();
        this.refreshRuneInfo();

        this.RefreshScroller();
    }

    public bool IsSelectedRune(uint uniqueRuneID)
    {
        if(this.m_currRune == null) return false;

        return this.m_currRune.UniqueRuneID == uniqueRuneID;
    }

    public void OnEquipClicked()
    {
        //장착 가능 개수 확인
        if(ProjectManager.Instance.UserData.Summon.GetSummon(this.SummonID).IsRuneMax == true)
        {
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("룬이 가득찼습니다");
            return;
        }

        ProjectManager.Instance.UserData.EquipRune(this.SummonID, this.m_currRune);

        this.refreshSummonData();
    }

    public void OnUnequipClicked()
    {
        ProjectManager.Instance.UserData.UnequipRune(this.SummonID, this.m_currRune);

        this.refreshSummonData();
    }

    public void OnChangeClicked()
    {
        if(this.m_csbtnChange.State == UIManager.eUI_BUTTON_STATE.Inactive) return;

        ProjectManager.Instance.UserData.ChangeRune(this.SummonID, this.m_currRune);

        this.refreshSummonData();
    }

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();

        //내 정령 팝업 열려있다면 갱신
        ProjectManager.Instance.UI.PopupSystem.RefreshPopup(ePOPUP_ID.Summon);
    }
}