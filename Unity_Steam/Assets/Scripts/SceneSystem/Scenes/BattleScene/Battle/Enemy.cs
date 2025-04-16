using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Team
{
    [SerializeField] private Transform[] m_arrTransParent = null;

    public List<Character_Enemy> ListChar { get; private set; } = new List<Character_Enemy>();
    private int m_nCurrAttackerIdx = 0;

    public void InitStage(int nCount)
    {
        this.ResetTeam();

        //TODO 스테이지 데이터로
        for(int i = 0; i < nCount; ++i)
        {
            this.ListChar.Add(ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Character_Enemy>(TableData.TableObjectPool.eID.Char_Enemy));
            this.ListChar[i].transform.SetParent(this.m_arrTransParent[i]);
            this.ListChar[i].Init((uint)(TableData.TableEnemy.eID.Enemy_1 + i));
        }
    }

    public override void ResetTeam()
    {
        for(int i = 0, nMax = this.ListChar.Count; i < nMax; ++i)
        {
            this.ListChar[i].gameObject.SetActive(false);
        }
        this.ListChar.Clear();
    }

    public override void TurnStart()
    {
        //맨 처음 적이 공격
        this.m_nCurrAttackerIdx = 0;
        StartCoroutine("coAttack");
    }

    private IEnumerator coAttack()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        this.ListChar[this.m_nCurrAttackerIdx].SetMyTurn();
        this.ListChar[this.m_nCurrAttackerIdx].UseSkill();
    }

    protected override bool isTurnFinish()
    {
        return this.m_nCurrAttackerIdx == this.ListChar.Count - 1;
    }

    public override void CheckTurnFinish()
    {
        if(this.isTurnFinish() == false)
        {
            //다음 공격
            this.m_nCurrAttackerIdx++;
            StartCoroutine("coAttack");
            return;
        }

        //턴 끝
        this.turnFinish();
    }

    public override void AddTarget(BaseCharacter charTarget)
    {
        this.ListChar[this.m_nCurrAttackerIdx].AddTarget(charTarget);
    }

    public void RemoveChar(Character_Enemy charEnemy)
    {
        if(this.ListChar.Contains(charEnemy) == false) return;

        this.ListChar.Remove(charEnemy);

        if(this.ListChar.Count == 0) ProjectManager.Instance.BattleScene.StageWin();
    }

    public override void AddTargetFromAttacker(BaseCharacter charAttacker, TableData.TableSkill.eTARGET_TYPE eTarget)
    {
        switch(eTarget)
        {
            case TableData.TableSkill.eTARGET_TYPE.Enemy_Random_2:
            {
                int nCount = 2;
                if(this.ListChar.Count < nCount) nCount = this.ListChar.Count;
                BaseCharacter[] listTarget = this.ListChar.OrderBy(g => Guid.NewGuid()).Take(nCount).ToArray();
                for(int i = 0; i < nCount; ++i)
                {
                    charAttacker.AddTarget(listTarget[i]);
                }
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Friendly_Random_1:
            {
                charAttacker.AddTarget(this.ListChar.OrderBy(g => Guid.NewGuid()).Take(1).First());
            }
            break;

            case TableData.TableSkill.eTARGET_TYPE.Enemy_All:
            case TableData.TableSkill.eTARGET_TYPE.Friendly_All:
            {
                for(int i = 0, nMax = this.ListChar.Count; i < nMax; ++i)
                {
                    charAttacker.AddTarget(this.ListChar[i]);
                }
            }
            break;
        }
    }
}