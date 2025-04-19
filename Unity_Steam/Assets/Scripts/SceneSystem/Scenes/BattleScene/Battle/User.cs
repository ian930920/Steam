using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class User : Team
{
    [SerializeField] private Transform m_transParent = null;
    [SerializeField] private Transform[] m_arrTransSummonObjParent = null;

    [SerializeField] private SummonSkill m_summonSkill = null;

    public Character_User CharUser { get; private set; } = null;

    private List<Character_SummonObj> m_listSummonObj = new List<Character_SummonObj>();

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
        for(int i = this.m_listSummonObj.Count - 1; i >= 0; --i)
        {
            this.m_listSummonObj[i].UpdateTurn();
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
        ProjectManager.Instance.BattleScene?.User_SetClickable(false);
        this.CharUser.UseSkill();
    }

    private void addAutoTarget(BaseCharacter charAttacker, int nTargetCount)
    {
        int nAddCount = 0;
        for(int i = 0, nMax = this.m_listSummonObj.Count; i < nMax; ++i)
        {
            charAttacker.AddTarget(this.m_listSummonObj[i]);
            nAddCount++;
            if(nAddCount == nTargetCount) return;
        }

        charAttacker.AddTarget(this.CharUser);
    }

    public override void AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        switch(eTarget)
        {
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Select_1:
            {
                this.addAutoTarget(charAttacker, 1);
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            {
                this.addAutoTarget(charAttacker, 2);
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            {
                this.addAutoTarget(charAttacker, 100);
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Self:
            {
                charAttacker.AddTarget(charAttacker);
            }
            break;
        }
    }

    public void AddFriendlyTarget()
    {
        for(int i = 0, nMax = this.m_listSummonObj.Count; i < nMax; ++i)
        {
            this.CharUser.AddTarget(this.m_listSummonObj[i]);
        }
    }

    public void AddSummonObject(uint summonObjID, CharacterStat stat)
    {
        Character_SummonObj summonObj = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Character_SummonObj>(TableData.TableObjectPool.eID.Char_SummonObj);
        summonObj.transform.SetParent(this.m_arrTransSummonObjParent.First(trans => trans.childCount == 0));
        summonObj.SetStat(stat);
        summonObj.Init(summonObjID);
        this.m_listSummonObj.Add(summonObj);

        ProjectManager.Instance.ObjectPool.PlayEffect(TableData.TableObjectPool.eID.Effect_Summon_Defence, summonObj.transform.position);
    }

    public void RemoveSummonObject(Character_SummonObj summonObj)
    {
        if(this.m_listSummonObj.Contains(summonObj) == false) return;

        this.m_listSummonObj.Remove(summonObj);
    }
}