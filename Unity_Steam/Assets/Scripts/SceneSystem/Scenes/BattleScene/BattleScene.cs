using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] private Transform m_transUser = null;
    [SerializeField] private Transform[] m_arrTransEnemy = null;

    //캐릭터 관련 변수
    private Character_User m_charUser = null;
    private List<Character_Enemy> m_listCharEnemy = new List<Character_Enemy>();

    //전투 관련 변수
    private BaseCharacter m_charAttacker = null;
    private int m_nEnemyAttackIdx = 0;
    public bool IsUserTurn { get; private set; }  = true;
    public bool IsUserAttackable { get; private set; }  = true;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        //세팅~
        ProjectManager.Instance.InitInBattleScene();

        //스테이지 리셋
        this.resetStage();
    }

#region Stage
    private void resetStage()
    {
        //유저 초기화
        this.m_charUser?.gameObject.SetActive(false);
        this.m_charUser = null;

        //적 초기화
        for(int i = 0, nMax = this.m_listCharEnemy.Count; i < nMax; ++i)
        {
            this.m_listCharEnemy[i].gameObject.SetActive(false);
        }
        this.m_listCharEnemy.Clear();

        //스테이지 세팅
        this.InitStage();
        
        //전투 변수 초기화
        this.setTurn(true);
        this.IsUserAttackable = true;
    }

    public void InitStage()
    {
        //유저 세팅
        this.m_charUser = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Character_User>(TableData.TableObjectPool.eID.Char_User);
        this.m_charUser.transform.SetParent(this.m_transUser);
        this.m_charUser.Init(TableData.TableCharacter.eID.User);

        //적 세팅
        int nEnemyCount = 3;
        for(int i = 0; i < nEnemyCount; ++i)
        {
            this.m_listCharEnemy.Add(ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Character_Enemy>(TableData.TableObjectPool.eID.Char_Enemy));
            this.m_listCharEnemy[i].transform.SetParent(this.m_arrTransEnemy[i]);
            this.m_listCharEnemy[i].Init(TableData.TableCharacter.eID.Enemy_1);
        }
    }

    private void stageWin()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemPopup("승리!");

        //스테이지 리셋
        this.resetStage();
    }

    public void StageDefeat()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemPopup("패배!");

        //스테이지 리셋
        this.resetStage();
    }
#endregion

    public void ChangeTurn()
    {
        if(this.IsUserTurn == false && this.m_nEnemyAttackIdx < this.m_listCharEnemy.Count - 1)
        {
            this.m_nEnemyAttackIdx++;
            StartCoroutine("coEnemyAttack", this.m_nEnemyAttackIdx);
            return;
        }

        this.setTurn(!this.IsUserTurn);
    }

    private void setTurn(bool isUserTuren)
    {
        this.IsUserTurn = isUserTuren;
        if(this.IsUserTurn == true)
        {
            this.IsUserAttackable = true;
            this.m_charAttacker = this.m_charUser;
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("유저 턴");
        }
        else
        {
            this.m_nEnemyAttackIdx = 0;
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("상대 턴");
            StartCoroutine("coEnemyAttack", this.m_nEnemyAttackIdx);
        }
    }

    private IEnumerator coEnemyAttack(int nIdx)
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

        this.m_charAttacker = this.m_listCharEnemy[nIdx];
        this.Attack(this.m_charUser);
    }

    public void Attack(BaseCharacter charTarget)
    {
        if(this.IsUserAttackable == true) this.IsUserAttackable = false;

        this.m_charAttacker.Attack(charTarget);
    }

    public void RemoveEnemy(Character_Enemy charEnemy)
    {
        if(this.m_listCharEnemy.Contains(charEnemy) == false) return;

        this.m_listCharEnemy.Remove(charEnemy);

        if(this.m_listCharEnemy.Count == 0) this.stageWin();
    }
}