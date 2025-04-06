using UnityEngine;
using System.Collections.Generic;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_renderer = null;

    [Header("UI")][Space(5)]
    [SerializeField] private UI_Reactive_Gauge m_uiHP = null;
    [SerializeField] private Transform m_targetUI = null;

    public uint CharID { get; private set; } = 0; 
    private ulong m_nMaxHP = 0;
    private ulong m_nCurrHP = 0;
    private List<Skill> m_listSkill = new List<Skill>();
    private bool isUser => this.CharID == (uint)TableData.TableCharacter.eID.User;

    public void Init(TableData.TableCharacter.eID eID)
    {
        this.Init((uint)eID);
    }

    public void Init(uint charID)
    {
        this.CharID = charID;
        this.m_renderer.sprite = ProjectManager.Instance.Table.Character.GetSprite(this.CharID);
        this.m_renderer.flipX = this.isUser;

        TableData.TableData_Character dataChar = ProjectManager.Instance.Table.Character.GetData(this.CharID);
        this.m_nMaxHP = dataChar.hp;
        this.m_nCurrHP = this.m_nMaxHP;
        this.m_listSkill.Clear();
        for(int i = 0, nMax = dataChar.listSkillID.Count; i < nMax; ++i)
        {
            this.m_listSkill.Add(new Skill(dataChar.listSkillID[i]));
        }

        this.gameObject.SetActive(true);
        this.transform.localPosition = Vector3.zero;
        //this.m_uiHP.ActiveUI(new );
    }

    public void Attack(BaseCharacter charTarget)
    {
        //TODO 스킬 인덱스 정하기 
        charTarget.Damaged(this.GetAttackDamage());
    }

    public ulong GetAttackDamage(int nSkillIdx = 0)
    {
        ulong nDamage = this.m_listSkill[nSkillIdx].GetDamage();
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup($"공격 데미지 {nDamage}");
        return nDamage;
    }

    public void Damaged(ulong nDamage)
    {
        this.m_nCurrHP -= nDamage;
        if(this.m_nCurrHP <= 0) this.death();
        else ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup($"피격 체력 {this.m_nCurrHP}");
    }

    private void death()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("사망");
        this.gameObject.SetActive(false);
    }

    private void OnMouseUp()
    {
        if(this.isUser == true) return;
        if(ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().IsUserTurn == false) return;

        //공격 선텍
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().Attack(this);
    }
}

public class Skill
{
    public SkillData Status { get; private set; } = new SkillData();

    public Skill(uint skillID)
    {
        //TODO 스킬 테이블참조
        this.Status.Damage = 1;
    }

    public ulong GetDamage()
    {
        return this.Status.Damage;
    }
}

public class SkillData
{
    public ulong Damage { get; set; } = 0;
    public int Speed { get; set; } = 0;
    public int Amor { get; set; } = 0;

    public ushort Cost { get; set; } = 0;
    public ushort Turn { get; set; } = 0;
}