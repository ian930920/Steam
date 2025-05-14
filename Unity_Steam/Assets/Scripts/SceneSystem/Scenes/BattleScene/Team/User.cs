using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class User : Team
{
    [SerializeField] private Transform m_transParent = null;
    [SerializeField] private Image m_imgFinishTurn = null;

    public Unit_User Unit { get; private set; } = null;
    private List<Summon> m_listSummon = new List<Summon>();
    private Summon CurrSummon => this.m_listSummon[SceneManager.Instance.GetCurrScene<BattleScene>().HUD.SelectedSummonIdx];

    public void InitStage()
    {
        //인게임 세팅
        this.initCharacter();

        //정령 세팅
        this.InitSummon();
    }

    private void initCharacter()
    {
        this.Unit = ObjectPoolManager.Instance.GetPoolObjectComponent<Unit_User>(TableData.TableObjectPool.eID.Char_User);
        this.Unit.transform.SetParent(this.m_transParent);
        this.Unit.Init((uint)TableData.TableUser.eID.User);
    }

    public void InitSummon()
    {
        //소환수 저장
        this.m_listSummon.Clear();
        var listSummon = UserDataManager.Instance.Summon.GetSummonDataList();
        for(int i = 0, nMax = listSummon.Count; i < nMax; ++i)
        {
            this.m_listSummon.Add(new Summon(listSummon[i].SummonID, this.Unit.GetStatus));
        }

        //UI 세팅
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.InitSummonUI(this.m_listSummon);
    }

    public override void TurnStart()
    {
        //소환수 스킬 업데이트
        this.updateSummonSkillTurn();

        //캐릭터 턴 세팅
        this.Unit.SetMyTurn();

        //유저 상태이상 업데이트
        this.Unit.UpdateStatus();

        //스킬 선택돼 있는걸로 세팅
        this.SelectSkill();

        //버튼 이펙트 비활성
        this.m_imgFinishTurn.enabled = false;
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();
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
        //적용된 상태이상 초기화
        this.Unit.ResetStatus();

        //정령 재사용 대기 시간
        for(int i = 0; i < this.m_listSummon.Count; i++)
        {
            this.m_listSummon[i].ResetTurn();
        }

        //버튼 이펙트 비활성
        this.m_imgFinishTurn.enabled = false;

        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();
    }

    public override void BattleFinish()
    {
        //유저 데이터에 저장
        UserDataManager.Instance.Session.SaveBattleInfo(this.Unit.CurrStat.GetStat(Stat_Character.eTYPE.HP), this.Unit.GetStatusToList());
    }

    protected override bool isTurnFinish()
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

    public override bool CheckTurnFinish()
    {
        if(this.isTurnFinish() == false)
        {
            //스킬 사용할 수 있다고 세팅
            this.m_imgFinishTurn.enabled = false;
            SceneManager.Instance.GetCurrScene<BattleScene>().User_SetClickable(true);
            return false;
        }

        //턴 끝

        //버튼 이펙트 활성
        this.m_imgFinishTurn.enabled = true;

        return true;
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
            UIManager.Instance.PopupSystem.OpenSystemTimerPopup("마나 부족!");
            return;
        }

        StartCoroutine("coUseSkill");
    }

    private IEnumerator coUseSkill()
    {
        SceneManager.Instance.GetCurrScene<BattleScene>().User_SetClickable(false);

        //스킬 이펙트
        float fDuration = SceneManager.Instance.GetCurrScene<BattleScene>().HUD.ActiveSummonSkill(this.CurrSummon.SummonID);

        this.Unit.UseMana(this.CurrSummon.Cost);

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(fDuration);

        this.CurrSummon.UseSkill();

        //슬롯 갱신
        SceneManager.Instance.GetCurrScene<BattleScene>().HUD.RefreshSummonGroupUI();
    }

    public int GetSummonDamage(uint summonID)
    {
        return this.m_listSummon.First(summon => summon.SummonID == summonID).Damage;
    }
}