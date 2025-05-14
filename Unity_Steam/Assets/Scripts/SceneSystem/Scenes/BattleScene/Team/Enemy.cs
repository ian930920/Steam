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

    public void InitStage(int nLevel)
    {
        this.ResetTeam();

        var enemyCount = TableManager.Instance.EnemyCount.GetEnemyCount(UserDataManager.Instance.Session.Stage, nLevel);
        for(int i = 0; i < enemyCount; ++i)
        {
            this.m_listChar.Add(ObjectPoolManager.Instance.GetPoolObjectComponent<Unit_Enemy>(TableData.TableObjectPool.eID.Char_Enemy));
            this.m_listChar[i].transform.SetParent(this.m_arrTransParent[i]);

            var enemyType = TableManager.Instance.EnemyType.GetRandomEnemyType(nLevel);
            var enemyID = TableManager.Instance.Enemy.GetRandomEnemyByType((int)enemyType);
            this.m_listChar[i].Init(enemyID);
        }
    }

    public void InitBoss()
    {
        this.ResetTeam();

        this.m_listChar.Add(ObjectPoolManager.Instance.GetPoolObjectComponent<Unit_Enemy>(TableData.TableObjectPool.eID.Char_Enemy));
        this.m_listChar[0].transform.SetParent(this.transform);
        
        var enemyID = TableManager.Instance.Enemy.GetRandomEnemyByType((int)TableData.TableEnemyType.eTYPE.Boss);
        this.m_listChar[0].Init(enemyID);
    }

    public override void ResetTeam()
    {
        for(int i = 0, nMax = this.m_listChar.Count; i < nMax; ++i)
        {
            this.m_listChar[i].gameObject.SetActive(false);
        }
        this.m_listChar.Clear();
    }

    public override void BattleFinish()
    {
        //모든 공격 멈추기
        for(int i = 0, nMax = this.m_listChar.Count; i < nMax; ++i)
        {
            this.m_listChar[i].StopAllCoroutines();
        }

        this.StopAllCoroutines();
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

        StopAllCoroutines();
        StartCoroutine("coAttack");
    }

    private IEnumerator coAttack()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        if(this.m_listChar.Count > 0) this.m_listChar[this.m_nCurrAttackerIdx].SetMyTurn();
    }

    protected override bool isTurnFinish()
    {
        if(this.m_listChar.Count == 0) return true;

        return this.m_nCurrAttackerIdx == this.m_listChar.Count - 1;
    }

    public override bool CheckTurnFinish()
    {
        if(this.isTurnFinish() == false)
        {
            //다음 공격
            this.m_nCurrAttackerIdx++;
            StartCoroutine("coAttack");
            return false;
        }

        //턴 끝
        return true;
    }

    public override void AddTarget(BaseUnit charTarget)
    {
        this.m_listChar[this.m_nCurrAttackerIdx].AddTarget(charTarget);
    }

    public void RemoveChar(Unit_Enemy charEnemy)
    {
        if(this.m_listChar.Contains(charEnemy) == false) return;

        this.m_listChar.Remove(charEnemy);

        if(this.m_listChar.Count == 0) SceneManager.Instance.GetCurrScene<BattleScene>().StageWin();
    }

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