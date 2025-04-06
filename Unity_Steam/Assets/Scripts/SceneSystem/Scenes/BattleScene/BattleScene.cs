using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private GameObject m_gobjScrChar = null;
    [SerializeField] private Transform m_transUser = null;
    private BaseCharacter m_charUser = null;

    [SerializeField] private Transform[] m_arrTransEnemy = null;
    private List<BaseCharacter> m_listCharEnemy = new List<BaseCharacter>();

    public bool IsUserTurn { get; private set; }  = true;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        //스테이지 세팅
        this.InitStage();
    }

    public void InitStage()
    {
        //유저 세팅
        this.m_charUser = Instantiate(this.m_gobjScrChar).GetComponent<BaseCharacter>();
        this.m_charUser.transform.SetParent(this.m_transUser);
        this.m_charUser.Init(TableData.TableCharacter.eID.User);

        //적 세팅
        int nEnemyCount = 3;
        this.m_listCharEnemy.Clear();
        for(int i = 0; i < nEnemyCount; ++i)
        {
            this.m_listCharEnemy.Add(Instantiate(this.m_gobjScrChar).GetComponent<BaseCharacter>());
            this.m_listCharEnemy[i].transform.SetParent(this.m_arrTransEnemy[i]);
            this.m_listCharEnemy[i].Init(TableData.TableCharacter.eID.Enemy_1);
        }

        //일단 유저 선공
        this.setTurn(true);
    }

    private int m_nEnemyAttackIdx = 0;

    public void ChangeTurn()
    {
        if(this.IsUserTurn == false && this.m_nEnemyAttackIdx < this.m_listCharEnemy.Count - 1)
        {
            this.m_nEnemyAttackIdx++;
            this.enemyAttack(this.m_nEnemyAttackIdx);
            return;
        }

        this.setTurn(!this.IsUserTurn);
    }

    private void setTurn(bool isUserTuren)
    {
        this.IsUserTurn = isUserTuren;
        if(this.IsUserTurn == true) this.m_charAttacker = this.m_charUser;
        else
        {
            this.m_nEnemyAttackIdx = 0;
            this.enemyAttack(this.m_nEnemyAttackIdx);
        }
    }

    private void enemyAttack(int nIdx)
    {
        this.m_charAttacker = this.m_listCharEnemy[nIdx];
        this.Attack(this.m_charUser);
    }

    private BaseCharacter m_charAttacker = null;
    //private BaseCharacter m_charTarget = null;

    public void Attack(BaseCharacter charTarget)
    {
        this.m_charAttacker.Attack(charTarget);
        this.ChangeTurn();
    }
}