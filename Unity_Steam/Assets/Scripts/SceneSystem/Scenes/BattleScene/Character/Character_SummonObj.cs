using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class Character_SummonObj : BaseCharacter
{
    private ulong m_nTurn = 0;

    public override void Init(uint charID)
    {
        base.Init(charID);

        base.m_renderer.sprite = ProjectManager.Instance.Table.SummonObj.GetSprite(base.CharID);
        base.m_renderer.flipX = true;

        //스킬로 만들기때문에 외부(Skill)에서 Stat 세팅해야함
        //스킬 세팅 필요없음
    }

    /// <summary>
    /// Init보다 먼저 호출해야함
    /// 스킬로 만들기때문에 외부(Skill)에서 세팅해야함
    /// </summary>
    /// <param name="stat"></param>
    public void SetStat(CharacterStat stat)
    {
        base.m_stat = stat;
        base.m_nCurrHP = base.m_stat.HP;
        this.m_nTurn = stat.Turn;
    }

    protected override void checkFinishTurn()
    {

    }

    protected override void setTarget()
    {

    }

    public void UpdateTurn()
    {
        if(this.m_nTurn < 1) return;

        //턴 감소
        this.m_nTurn--;

        if(this.m_nTurn == 0) StartCoroutine("coResrveDeath");
    }

    private IEnumerator coResrveDeath()
    {
        yield return null;

        this.death();
    }

    protected override void death()
    {
        this.transform.SetParent(ProjectManager.Instance.BattleScene.TransRootObjectPool);
        ProjectManager.Instance.BattleScene.RemoveUserSummonObj(this);

        base.death();
    }

    private void OnMouseUp()
    {
        if(ProjectManager.Instance.BattleScene?.IsUserTurn == false) return;
        if(ProjectManager.Instance.BattleScene?.IsUserClickable == false) return;

        //타겟 저장
        ProjectManager.Instance.BattleScene?.User_AddTarget(this);
    }
}