using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public abstract class BaseUnit : MonoBehaviour
{
    protected enum eSTATE
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
    [SerializeField] private Animator m_animator = null;

    public uint CharID { get; private set; } = 0;
    public Stat_Character DefaultStat { get; protected set; } = new Stat_Character();
    public Stat_Character CurrStat { get; protected set; } = new Stat_Character();

    protected Skill CurrSkill { get; private set; } = null;

    //Key : statusID, Status
    private Dictionary<uint, Status> m_dicStatus = new Dictionary<uint, Status>();

    protected UI_CharacterStatusBar m_uiStatusBar = null;

    public virtual void Init(uint charID)
    {
        this.CharID = charID;

        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);
        this.m_dicStatus.Clear();

        //캐릭터 상태바 세팅
        this.initStatusBar();

        this.playAnim(eSTATE.Idle);
    }

    abstract protected void initStatusBar();

    protected void playAnim(eSTATE eState)
    {
        this.m_animator.SetTrigger(STR_ANIM_TRIGGER[(int)eState]);
    }

    public virtual void SetMyTurn()
    {
        //타겟 모두 비우고
        this.CurrSkill = null;

        //각 하위 클래스에서 사용할 스킬 정해
    }

    public void SetCurrSkill(Skill skill)
    {
        this.CurrSkill = skill;
        this.CurrSkill.ResetTarget();

        //스킬따라서 타겟도 정해버리기~
        this.setTarget();
    }

    abstract protected void initStat(uint charID);
    abstract protected void setTarget();

    public void AddTarget(BaseUnit target)
    {
        this.CurrSkill.AddTarget(target);
    }

    public void UpdateStatus()
    {
        var listStatus = this.m_dicStatus.Values.ToList();
        for(int i = listStatus.Count - 1; i >= 0; --i)
        {
            listStatus[i].UpdateTurn();
            
            //실행
            listStatus[i].DoStatus(this);

            //죽어버리면 ㄴㄴ 그만
            if(this.gameObject.activeSelf == false) return;
         
            bool isRemove = this.m_uiStatusBar.UpdateStatus((uint)listStatus[i].eStatusID, listStatus[i].RemainTurn);
            if(isRemove == true) this.m_dicStatus.Remove((uint)listStatus[i].eStatusID);
        }
    }

    public void Damaged(stDamage stDamage)
    {
        //턴끝
        ProjectManager.Instance.BattleScene.RemoveTurnEvent(this);

        if(this.gameObject.activeSelf == false)
        {
            ProjectManager.Instance.LogWarning("죽었는데 딜 왜 들어오냐?");
            return;
        }

        //TODO 다른 상태이상이랑 중첩 되는지

        if(stDamage.Value < 1)
        {
            ProjectManager.Instance.ObjectPool.PlayCountEffectByUlong(stDamage, this.transform.position);
            return;
        }

        if(this.GetStatus(TableData.TableStatus.eID.Weakened_Def) != null) stDamage.Value = (int)(stDamage.Value * 1.5f);
        if(this.GetStatus(TableData.TableStatus.eID.Attack_Enhancement) != null) stDamage.Value = (int)(stDamage.Value * 1.5f);
        if(this.GetStatus(TableData.TableStatus.eID.Weakened_Atk) != null) stDamage.Value = (int)(stDamage.Value * 0.5f);
        if(this.GetStatus(TableData.TableStatus.eID.Defense_Enhancement) != null) stDamage.Value = (int)(stDamage.Value * 0.5f);

        if(this.CurrStat.GetStat(Stat_Character.eTYPE.Shield) > stDamage.Value)
        {
            var shield = this.CurrStat.GetStat(Stat_Character.eTYPE.Shield) - stDamage.Value;
            this.CurrStat.SetStat(Stat_Character.eTYPE.Shield, shield);
        }
        else
        {
            var damage = stDamage.Value -= this.CurrStat.GetStat(Stat_Character.eTYPE.Shield);
            var currHP = this.CurrStat.GetStat(Stat_Character.eTYPE.HP);
            this.CurrStat.SetStat(Stat_Character.eTYPE.HP, currHP - damage);
        }

        //데미지
        ProjectManager.Instance.ObjectPool.PlayCountEffect_Damage(stDamage, this.transform.position);
        
        //피격 애니
        this.playAnim(eSTATE.Damaged);

        if(this.CurrStat.GetStat(Stat_Character.eTYPE.HP) <= 0) this.death(); //죽음
        else this.m_uiStatusBar.RefreshGauge(this.CurrStat.GetStat(Stat_Character.eTYPE.HP)); //UI 갱신
    }

    public void Heal(stDamage stDamage)
    {
        //턴끝
        ProjectManager.Instance.BattleScene.RemoveTurnEvent(this);

        if(this.gameObject.activeSelf == false)
        {
            ProjectManager.Instance.LogWarning("죽었는데 힐 왜 들어오냐?");
            return;
        }

        stDamage.eSkillType = stDamage.eSKILL_TYPE.Heal;

        var currHP = this.CurrStat.GetStat(Stat_Character.eTYPE.HP);
        this.CurrStat.SetStat(Stat_Character.eTYPE.HP, Mathf.Clamp(stDamage.Value + currHP, currHP, this.DefaultStat.GetStat(Stat_Character.eTYPE.HP)));
        this.m_uiStatusBar.RefreshGauge(this.CurrStat.GetStat(Stat_Character.eTYPE.HP));
        ProjectManager.Instance.ObjectPool.PlayCountEffect_Heal(stDamage, this.transform.position);
    }

    public void AddShield(int value)
    {
        var shield = this.CurrStat.GetStat(Stat_Character.eTYPE.Shield) + value;
        this.CurrStat.SetStat(Stat_Character.eTYPE.Shield, shield);

        //턴끝
        ProjectManager.Instance.BattleScene.RemoveTurnEvent(this);
    }

    public void ResetShield()
    {
        this.CurrStat.SetStat(Stat_Character.eTYPE.Shield, 0);
    }

    public void AddStatus(uint statusID, int turn)
    {
        if(this.m_dicStatus.ContainsKey(statusID) == false)
        {
            this.m_dicStatus.Add(statusID, new Status(statusID, turn));
            this.m_dicStatus[statusID].AddStatus(this);
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

        //상태바 지우기
        if(this.m_uiStatusBar != null)
        {
            this.m_uiStatusBar.gameObject.SetActive(false);
            this.m_uiStatusBar = null;
        }
    }

    public Status GetStatus(TableData.TableStatus.eID eStatusID)
    {
        uint statusID = (uint)eStatusID;
        if(this.m_dicStatus.ContainsKey(statusID) == false) return null;

        return this.m_dicStatus[statusID];
    }

    protected void resetMana()
    {
        this.CurrStat.SetStat(Stat_Character.eTYPE.Mana, this.DefaultStat.GetStat(Stat_Character.eTYPE.Mana));
        ProjectManager.Instance.BattleScene?.HUD.RefreshMana(this.CurrStat.GetStat(Stat_Character.eTYPE.Mana));
    }

    public void AddMana(int cost)
    {
        var mana = this.CurrStat.GetStat(Stat_Character.eTYPE.Mana) + cost;
        this.CurrStat.SetStat(Stat_Character.eTYPE.Mana, mana);
        if(this.CurrStat.GetStat(Stat_Character.eTYPE.Mana) > this.DefaultStat.GetStat(Stat_Character.eTYPE.Mana))
        {
            ProjectManager.Instance.BattleScene?.HUD.SetMaxMana(mana);
        }
        ProjectManager.Instance.BattleScene?.HUD.RefreshMana(mana);
    }

    public void UseMana(int cost)
    {
        var mana = this.CurrStat.GetStat(Stat_Character.eTYPE.Mana) - cost;
        this.CurrStat.SetStat(Stat_Character.eTYPE.Mana, mana);
        ProjectManager.Instance.BattleScene?.HUD.RefreshMana(mana);
    }
}