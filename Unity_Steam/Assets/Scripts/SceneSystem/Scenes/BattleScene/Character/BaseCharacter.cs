using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

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
    public CharacterStat Stat { get; protected set; } = null;
    protected ulong m_nCurrHP = 0;
    protected ulong m_nCurrMana = 0;
    protected ulong m_nCurrShield = 0;
    protected bool IsDead { get; private set; } = false;

    protected List<Skill> m_listSkill = new List<Skill>();
    protected Skill m_currSkill = null;

    //Key : statusID, Status
    private Dictionary<uint, Status> m_dicStatus = new Dictionary<uint, Status>();
    private List<Status> m_listStatus = new List<Status>();

    private UI_CharacterStatusBar m_uiStatusBar = null;

    protected List<BaseCharacter> m_listTarget = new List<BaseCharacter>();

    public virtual void Init(uint charID)
    {
        this.CharID = charID;
        this.m_renderer.sprite = ProjectManager.Instance.Table.Enemy.GetSprite(this.CharID);
        this.IsDead = false;

        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);
        this.m_listTarget.Clear();
        this.m_dicStatus.Clear();

        //캐릭터 상태바 세팅
        if(this.m_uiStatusBar == null) this.m_uiStatusBar = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<UI_CharacterStatusBar>(TableData.TableObjectPool.eID.UI_CharaterStatusBar);
        this.m_uiStatusBar.Init(Camera.main.WorldToScreenPoint(this.transform.position), this.Stat.HP);

        this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eSTATE.Idle]);
    }

    public virtual void SetMyTurn()
    {
        //타겟 모두 비우고
        this.m_currSkill = null;

        this.UpdateRemainTurn();

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

    public virtual void UseSkill()
    {
        if(this.m_currSkill.IsUseable(this.m_nCurrMana) == false || this.getStatus(TableData.TableStatus.eID.Fainting) != null)
        {
            ProjectManager.Instance.BattleScene?.ChangeTurn();
            //this.CheckFinishTurn(); TODO Delete
            return;
        }

        //지금 설정된 스킬 사용
        StartCoroutine("coUseSkill");
    }

    private IEnumerator coUseSkill()
    {
        this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eSTATE.Attack]);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(0.2f);

        this.useCurrSkill();

        /* TODO Delete
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(2.0f);

        //턴 바꾸기~
        this.checkFinishTurn();
        */
    }

    public void UpdateRemainTurn()
    {
        for(int i = 0, nMax = this.m_listSkill.Count; i < nMax; ++i)
        {
            this.m_listSkill[i].UpdateTurn();
        }
    }

    public void UpdateStatus()
    {
        this.m_listStatus = this.m_dicStatus.Values.ToList();
        for(int i = this.m_listStatus.Count - 1; i >= 0; --i)
        {
            this.m_listStatus[i].UpdateTurn();
            
            //실행
            this.m_listStatus[i].DoStatus(this);

            //죽어버리면 ㄴㄴ 그만
            if(this.gameObject.activeSelf == false) return;
         
            bool isRemove = this.m_uiStatusBar.UpdateStatus((uint)this.m_listStatus[i].eStatusID, this.m_listStatus[i].RemainTurn);
            if(isRemove == true) this.m_dicStatus.Remove((uint)this.m_listStatus[i].eStatusID);
        }
    }

    /* TODO Delete
    //public abstract void CheckFinishTurn();
    public void TurnFinish()
    {
        //턴 바꾸기~
        //ProjectManager.Instance.BattleScene?.ChangeTurn();
    }
    */

    protected virtual void useCurrSkill()
    {
        this.m_currSkill.UseSkill(this.m_listTarget);
    }

    public void Damaged(stDamage stDamage)
    {
        //턴끝
        ProjectManager.Instance.BattleScene.RemoveTurnEvent(this);

        if(this.IsDead == true)
        {
            ProjectManager.Instance.LogWarning("죽었는데 딜 왜 들어오냐?");
            return;
        }

        //TODO 다른 상태이상이랑 중첩 되는지


        if(stDamage.Damage < 1)
        {
            ProjectManager.Instance.ObjectPool.PlayCountEffectByUlong(stDamage, this.transform.position);
            return;
        }

        if(this.getStatus(TableData.TableStatus.eID.Weakened_Def) != null) stDamage.Damage = (ulong)(stDamage.Damage * 1.5f);
        if(this.getStatus(TableData.TableStatus.eID.Attack_Enhancement) != null) stDamage.Damage = (ulong)(stDamage.Damage * 1.5f);
        if(this.getStatus(TableData.TableStatus.eID.Weakened_Atk) != null) stDamage.Damage = (ulong)(stDamage.Damage * 0.5f);
        if(this.getStatus(TableData.TableStatus.eID.Defense_Enhancement) != null) stDamage.Damage = (ulong)(stDamage.Damage * 0.5f);

        if(this.m_nCurrShield > stDamage.Damage) this.m_nCurrShield -= stDamage.Damage;
        else
        {
            ulong damage = stDamage.Damage -= this.m_nCurrShield;
            if(this.m_nCurrHP <= damage) damage = this.m_nCurrHP;
            ProjectManager.Instance.ObjectPool.PlayCountEffectByUlong(stDamage, this.transform.position);
            this.m_nCurrHP -= damage;
       
            this.m_uiStatusBar.RefreshGauge(this.m_nCurrHP);
        
            this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eSTATE.Damaged]);
        }

        if(this.m_nCurrHP < 1) this.death();
    }

    public void Heal(stDamage stDamage)
    {
        //턴끝
        ProjectManager.Instance.BattleScene.RemoveTurnEvent(this);

        if(this.IsDead == true)
        {
            ProjectManager.Instance.LogWarning("죽었는데 힐 왜 들어오냐?");
            return;
        }

        stDamage.IsHeal = true;
        this.m_nCurrHP = (ulong)Mathf.Clamp(stDamage.Damage + this.m_nCurrHP, this.m_nCurrHP, this.Stat.HP);
        this.m_uiStatusBar.RefreshGauge(this.m_nCurrHP);
        ProjectManager.Instance.ObjectPool.PlayCountEffectByUlong(stDamage, this.transform.position);
    }

    public void AddShield(ulong value)
    {
        this.m_nCurrShield += value;

        //턴끝
        ProjectManager.Instance.BattleScene.RemoveTurnEvent(this);
    }

    public void ResetShield()
    {
        this.m_nCurrShield = 0;
    }

    public void AddStatus(uint statusID, ulong turn)
    {
        if(this.m_dicStatus.ContainsKey(statusID) == false)
        {
            this.m_dicStatus.Add(statusID, new Status(statusID, turn));
            //this.m_dicStatus[statusID].AddStatus(turn);
        }
        else
        {
            this.m_dicStatus[statusID].AddTurn(turn);
        }

        this.m_uiStatusBar.UpdateStatus(statusID, this.m_dicStatus[statusID].RemainTurn);
    }

    protected virtual void death()
    {
        ProjectManager.Instance.Log("사망");
        
        this.gameObject.SetActive(false);
        this.IsDead = true;

        //상태바 지우기
        if(this.m_uiStatusBar != null)
        {
            this.m_uiStatusBar.gameObject.SetActive(false);
            this.m_uiStatusBar = null;
        }
    }

    protected CharacterStat getStat()
    {
        return this.Stat;
    }

    protected Status getStatus(TableData.TableStatus.eID eStatusID)
    {
        uint statusID = (uint)eStatusID;
        if(this.m_dicStatus.ContainsKey(statusID) == false) return null;

        return this.m_dicStatus[statusID];
    }

    public void AddMana(ulong cost)
    {
        this.m_nCurrMana += cost;
    }

    public void UseMana(ulong cost)
    {
        this.m_nCurrMana -= cost;
    }
}