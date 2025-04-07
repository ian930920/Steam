using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer m_renderer = null;

    private UI_CharacterStatusBar m_uiStatusBar = null;

    public uint CharID { get; private set; } = 0; 
    private ulong m_nMaxHP = 0;
    private ulong m_nCurrHP = 0;
    protected List<Skill> m_listSkill = new List<Skill>();

    public void Init(TableData.TableCharacter.eID eID)
    {
        this.Init((uint)eID);
    }

    public virtual void Init(uint charID)
    {
        this.CharID = charID;
        this.m_renderer.sprite = ProjectManager.Instance.Table.Character.GetSprite(this.CharID);

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

        //캐릭터 상태바 세팅
        if(this.m_uiStatusBar == null) this.m_uiStatusBar = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<UI_CharacterStatusBar>(TableData.TableObjectPool.eID.UI_CharaterStatusBar);
        this.m_uiStatusBar.Init(Camera.main.WorldToScreenPoint(this.transform.position), this.m_nMaxHP);
    }

    public void Attack(BaseCharacter charTarget)
    {
        StartCoroutine("coAttack", charTarget);
    }

    private IEnumerator coAttack(BaseCharacter charTarget)
    {
        ProjectManager.Instance.ObjectPool.PlayEffect(TableData.TableObjectPool.eID.Effect_Attack, this.transform.position);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        //TODO 스킬 인덱스 정하기 
        charTarget.Damaged(this.getAttackDamage(Random.Range(0, this.m_listSkill.Count)));

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        //턴 바꾸기~
        this.checkFinishTurn();
    }

    protected abstract void checkFinishTurn();

    private ulong getAttackDamage(int nSkillIdx = 0)
    {
        //TODO 캐릭터 기본 스테이터스 적용
        ulong nDamage = this.m_listSkill[nSkillIdx].GetDamage(1);
        ProjectManager.Instance.Log($"공격 데미지 {nDamage}");
        return nDamage;
    }

    public void Damaged(ulong nDamage)
    {
        ProjectManager.Instance.ObjectPool.PlayEffect(TableData.TableObjectPool.eID.Effect_Damage, this.transform.position);

        this.m_nCurrHP -= nDamage;
        this.m_uiStatusBar.RefreshGauge(this.m_nCurrHP);

        if(this.m_nCurrHP <= 0) this.death();
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
}