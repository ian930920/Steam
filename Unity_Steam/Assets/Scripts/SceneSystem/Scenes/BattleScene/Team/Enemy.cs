using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Team
{
    [SerializeField] private Transform[] m_arrTransParent = null;

    private List<Unit_Enemy> m_listChar = new List<Unit_Enemy>();
    private int m_nCurrAttackerIdx = 0;

    public void InitStage(int nCount)
    {
        this.ResetTeam();

        //TODO 스테이지 데이터로
        for(int i = 0; i < nCount; ++i)
        {
            this.m_listChar.Add(ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Unit_Enemy>(TableData.TableObjectPool.eID.Char_Enemy));
            this.m_listChar[i].transform.SetParent(this.m_arrTransParent[i]);
            this.m_listChar[i].Init((uint)(TableData.TableEnemy.eID.Enemy_1 + i));
        }
    }

    public override void ResetTeam()
    {
        for(int i = 0, nMax = this.m_listChar.Count; i < nMax; ++i)
        {
            this.m_listChar[i].gameObject.SetActive(false);
        }
        this.m_listChar.Clear();
    }

    public override void TurnStart()
    {
        //상태이상 업데이트
        for(int i = this.m_listChar.Count - 1; i >= 0; --i)
        {
            this.m_listChar[i].UpdateStatus();
        }

        //맨 처음 적이 공격
        this.m_nCurrAttackerIdx = 0;
        StartCoroutine("coAttack");
    }

    private IEnumerator coAttack()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        this.m_listChar[this.m_nCurrAttackerIdx].SetMyTurn();
    }

    public override bool IsTurnFinish()
    {
        return this.m_nCurrAttackerIdx == this.m_listChar.Count - 1;
    }

    public override void CheckTurnFinish()
    {
        if(this.IsTurnFinish() == false)
        {
            //다음 공격
            this.m_nCurrAttackerIdx++;
            StartCoroutine("coAttack");
            return;
        }

        //턴 끝
        this.turnFinish();
    }

    public override void AddTarget(BaseUnit charTarget)
    {
        this.m_listChar[this.m_nCurrAttackerIdx].AddTarget(charTarget);
    }

    public void RemoveChar(Unit_Enemy charEnemy)
    {
        if(this.m_listChar.Contains(charEnemy) == false) return;

        this.m_listChar.Remove(charEnemy);

        if(this.m_listChar.Count == 0) ProjectManager.Instance.BattleScene?.StageWin();
    }

    /* TODO Delete
    public void AddTargetFromAttacker(BaseUnit charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        switch(eTarget)
        {
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            {
                int nCount = 2;
                if(this.m_listChar.Count < nCount) nCount = this.m_listChar.Count;
                var listTarget = this.m_listChar.OrderBy(g => Guid.NewGuid()).Take(nCount).ToArray();
                for(int i = 0; i < nCount; ++i)
                {
                    charAttacker.AddTarget(listTarget[i]);
                }
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
            {
                charAttacker.AddTarget(this.m_listChar.OrderBy(g => Guid.NewGuid()).Take(1).First());
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            {
                for(int i = 0, nMax = this.m_listChar.Count; i < nMax; ++i)
                {
                    charAttacker.AddTarget(this.m_listChar[i]);
                }
            }
            break;
        }
    }
    */

    public List<Unit_Enemy> GetTargetList(TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        switch(eTarget)
        {
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            {
                int nCount = 2;
                if(this.m_listChar.Count < nCount) nCount = this.m_listChar.Count;
                return this.m_listChar.OrderBy(g => Guid.NewGuid()).Take(nCount).ToList();
            }

            case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
            return this.m_listChar.OrderBy(g => Guid.NewGuid()).Take(1).ToList();

            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            return this.m_listChar;
        }

        return new List<Unit_Enemy>();
    }
}