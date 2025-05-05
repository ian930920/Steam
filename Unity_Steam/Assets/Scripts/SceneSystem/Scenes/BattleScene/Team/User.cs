using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Team
{
    [SerializeField] private Transform m_transParent = null;

    public Unit_User Unit { get; private set; } = null;
    private List<Summon> m_listSummon = new List<Summon>();
    private Summon CurrSummon => this.m_listSummon[ProjectManager.Instance.BattleScene.HUD.SelectedSummonIdx];

    public void InitStage()
    {
        //인게임 세팅
        this.initCharacter();

        //정령 세팅
        this.initSummon();
    }

    private void initCharacter()
    {
        this.Unit = ProjectManager.Instance.ObjectPool.GetPoolObjectComponent<Unit_User>(TableData.TableObjectPool.eID.Char_User);
        this.Unit.transform.SetParent(this.m_transParent);
        this.Unit.Init((uint)TableData.TableUser.eID.User);
    }

    private void initSummon()
    {
        //소환수 저장
        this.m_listSummon.Clear();
        var listSummon = ProjectManager.Instance.UserData.Summon.GetSummonDataList();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            this.m_listSummon.Add(new Summon(listSummon[i].SummonID, this.Unit.GetStatus));
        }

        //UI 세팅
        ProjectManager.Instance.BattleScene?.HUD.InitSummonUI(this.m_listSummon);
    }

    public override void TurnStart()
    {
        //소환수 스킬 업데이트
        this.updateSummonSkillTurn();

        //유저 상태이상 업데이트
        this.Unit.UpdateStatus();

        //캐릭터 턴 세팅
        this.Unit.SetMyTurn();

        //스킬 선택돼 있는걸로 세팅
        this.SelectSkill();
    }

    private void updateSummonSkillTurn()
    {
        //소환수 스킬 쿨 돌리기
        for(int i = this.m_listSummon.Count - 1; i >= 0; --i)
        {
            this.m_listSummon[i].Skill.UpdateTurn();
        }
    }

    public override void ResetTeam()
    {
        this.Unit?.gameObject.SetActive(false);
        this.Unit = null;
    }

    public override bool IsTurnFinish()
    {
        //캐릭터 체크
        if(this.Unit.IsFinishTurn() == true) return true;

        //사용할 수 있는 정령 있는 지 확인
        for(int i = 0; i < this.m_listSummon.Count; i++)
        {
            if(this.m_listSummon[i].IsUseable(this.Unit.CurrStat.GetStat(Stat_Character.eTYPE.Mana)) == true) return false;
        }

        return true;
    }

    public override void CheckTurnFinish()
    {
        if(this.IsTurnFinish() == false)
        {
            //TODO 스킬 선택
            this.Unit.SetMyTurn();
            return;
        }

        //턴 끝
        this.turnFinish();
    }

    public override void AddTarget(BaseUnit charTarget)
    {
        this.Unit.AddTarget(charTarget);
    }

    public void SelectSkill()
    {
        this.Unit.SetCurrSkill(this.CurrSummon.Skill);
    }

    public void UseSkill()
    {
        //스킬쓸 수 있는지 확인
        if(this.CurrSummon.IsUseable(this.Unit.CurrStat.GetStat(Stat_Character.eTYPE.Mana)) == false)
        {
            ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("마나 부족!");
            return;
        }

        StartCoroutine("coUseSkill");
    }

    private IEnumerator coUseSkill()
    {
        ProjectManager.Instance.BattleScene?.User_SetClickable(false);

        //스킬 이펙트
        float fDuration = ProjectManager.Instance.BattleScene.HUD.ActiveSummonSkill(this.CurrSummon.SummonID);

        this.Unit.UseMana(this.CurrSummon.Cost);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(fDuration);

        this.CurrSummon.UseSkill();

        //슬롯 갱신
        ProjectManager.Instance.BattleScene?.HUD.RefreshSummonSlot();
    }
}