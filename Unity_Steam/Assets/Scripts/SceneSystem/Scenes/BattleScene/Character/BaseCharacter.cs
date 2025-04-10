using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseCharacter : MonoBehaviour
{
    private enum eSTATE
    {
        Idle,
        Attack,
        Damaged,
    }

    private static readonly string[] STR_ANIM_TRIGGER =
    {
        "Idle",
        "Attack",
        "Damaged",
    };

    [SerializeField] protected SpriteRenderer m_renderer = null;
    [SerializeField] protected Animator m_animator = null;

    public uint CharID { get; private set; } = 0; 
    protected Character_Stat m_stat = null;
    protected ulong m_nCurrHP = 0;
    protected ulong m_nCurrMP = 0;

    protected List<Skill> m_listSkill = new List<Skill>();
    protected Skill m_currSkill = null;

    private UI_CharacterStatusBar m_uiStatusBar = null;

    protected List<BaseCharacter> m_listTarget = new List<BaseCharacter>();

    public virtual void Init(uint charID)
    {
        this.CharID = charID;
        this.m_renderer.sprite = ProjectManager.Instance.Table.Enemy.GetSprite(this.CharID);

        this.gameObject.SetActive(true);
        this.transform.localPosition = Vector3.zero;
        this.m_listTarget.Clear();

        //캐릭터 상태바 세팅
        if(this.m_uiStatusBar == null) this.m_uiStatusBar = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<UI_CharacterStatusBar>(TableData.TableObjectPool.eID.UI_CharaterStatusBar);
        this.m_uiStatusBar.Init(Camera.main.WorldToScreenPoint(this.transform.position), this.m_stat.HP);

        this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eSTATE.Idle]);
    }

    public virtual void SetMyTurn()
    {
        //타겟 모두 비우고
        this.m_currSkill = null;

        //각 하위 클래스에서 사용할 스킬 정해
    }

    public void SetCurrSkill(int nSkillIdx)
    {
        this.m_currSkill = this.m_listSkill[nSkillIdx];

        //스킬타입에 따른 타겟 설정
        this.m_listTarget.Clear();
        this.setTarget();
    }

    abstract protected void setTarget();

    public void AddTarget(BaseCharacter charTarget)
    {
        if(this.m_currSkill.TargetCount == this.m_listTarget.Count) return;
        if(this.m_currSkill.isValiedTatget(charTarget) == false) return;
        if(this.m_listTarget.Contains(charTarget) == true) return;

        this.m_listTarget.Add(charTarget);
    }

    public void UseSkill()
    {
        //지금 설정된 스킬 사용
        StartCoroutine("coUseSkill");
    }

    private IEnumerator coUseSkill()
    {
        this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eSTATE.Attack]);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(0.2f);
        
        this.m_currSkill.UseSkill(this.m_listTarget);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        //턴 바꾸기~
        this.checkFinishTurn();
    }

    protected abstract void checkFinishTurn();

    public void Damaged(ulong nDamage)
    {
        ProjectManager.Instance.ObjectPool.PlayEffect(TableData.TableObjectPool.eID.Effect_Damage, this.transform.position);

        if(this.m_nCurrHP <= nDamage) nDamage = this.m_nCurrHP;
        this.m_nCurrHP -= nDamage;
        this.m_uiStatusBar.RefreshGauge(this.m_nCurrHP);
        ProjectManager.Instance.ObjectPool.PlayCountEffectByUlong(nDamage, this.transform.position);
        this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eSTATE.Damaged]);

        if(this.m_nCurrHP < 1) this.death();
    }

    public void Heal(ulong nHeal)
    {
        ProjectManager.Instance.ObjectPool.PlayEffect(TableData.TableObjectPool.eID.Effect_Damage, this.transform.position);

        this.m_nCurrHP = (ulong)Mathf.Clamp(nHeal + this.m_nCurrHP, this.m_nCurrHP, this.m_stat.HP);
        this.m_uiStatusBar.RefreshGauge(this.m_nCurrHP);
        ProjectManager.Instance.ObjectPool.PlayCountEffectByUlong(nHeal, this.transform.position);
    }

    protected virtual void death()
    {
        ProjectManager.Instance.Log("사망");
        
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //상태바 지우기
        if(this.m_uiStatusBar != null)
        {
            this.m_uiStatusBar.gameObject.SetActive(false);
            this.m_uiStatusBar = null;
        }
    }

    protected Character_Stat getStat()
    {
        return this.m_stat;
    }
}