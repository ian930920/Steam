using System.Collections.Generic;
using UnityEngine;

public class User : Team
{
    [SerializeField] private Transform m_transParent = null;
    public Character_User CharUser { get; private set; } = null;

    private List<Character_Summon> m_listSummon = new List<Character_Summon>();

    public void InitStage()
    {
        //유저 세팅
        this.CharUser = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Character_User>(TableData.TableObjectPool.eID.Char_User);
        this.CharUser.transform.SetParent(this.m_transParent);
        this.CharUser.Init((uint)TableData.TableUser.eID.User);
    }

    public override void TurnStart()
    {
        //소환수 턴 지우기
        for(int i = 0, nMax = this.m_listSummon.Count; i < nMax; ++i)
        {
            this.m_listSummon[i].UpdateTurn();
        }

        //캐릭터 턴 세팅
        this.CharUser.SetMyTurn();
    }

    public override void ResetTeam()
    {
        this.CharUser?.gameObject.SetActive(false);
        this.CharUser = null;
    }

    protected override bool isTurnFinish()
    {
        //마나 체크
        if(this.CharUser.IsFinishTurn() == false) return false;

        //TODO 사용할 소환수 있는 지 확인??

        return true;
    }

    public override void CheckTurnFinish()
    {
        if(this.isTurnFinish() == false)
        {
            //TODO 스킬 선택
            this.CharUser.SetMyTurn();
            return;
        }

        //턴 끝
        this.turnFinish();
    }

    public override void AddTarget(BaseCharacter charTarget)
    {
        this.CharUser.AddTarget(charTarget);
    }

    public void UseSkill()
    {
        this.CharUser.UseSkill();
    }

    public BaseCharacter GetAutoTarget()
    {
        //소환물 확인
        if(this.m_listSummon.Count > 0) return this.m_listSummon[0];

        return this.CharUser;
    }

    public override void AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        charAttacker.AddTarget(this.GetAutoTarget());
    }

    public void AddFriendlyTarget()
    {
        for(int i = 0, nMax = this.m_listSummon.Count; i < nMax; ++i)
        {
            this.CharUser.AddTarget(this.m_listSummon[i]);
        }
    }
}