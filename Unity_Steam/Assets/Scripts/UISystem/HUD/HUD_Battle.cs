using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Battle : BaseHUD
{
    [Space(5)][Header("소환수")]
    [SerializeField] private UI_Summon m_uiSummon = null;
    [SerializeField] private UI_SummonSkill m_uiSummonSkill = null;
    [SerializeField] private UI_RuneInfo m_uiRuneInfo = null;

    [Space(5)][Header("전투 정보")]
    [SerializeField] private UI_Mana m_uiMana = null;
    [SerializeField] private UI_TurnInfo m_uiTurnInfo = null;
    [SerializeField] private UI_StatusInfo m_uiStatusInfo = null;

    public int SelectedSkillIdx => this.m_uiSummon.SelectedIdx;

    public override void Init()
    {
        base.Init();
    }

    public override void RefreshUI()
    {

    }

    public void InitSummonUI(List<Summon> listSummon)
    {
        this.m_uiSummon.Init(listSummon);
    }

    public void RefreshSummonUI()
    {
        this.m_uiSummon.Refresh();
    }

    public void RefreshSummonSlot()
    {
        this.m_uiSummon.RefreshSlot();
    }

    public void SetMaxMana(ulong nMaxMana)
    {
        this.m_uiMana.SetMaxMana(nMaxMana);
    }

    public void RefreshMana(ulong nCurrMana)
    {
        this.m_uiMana.Refresh(nCurrMana);
    }

    public void OpenStatusInfo(uint statusID, Vector3 vecPos)
    {
        this.m_uiStatusInfo.Open(statusID, vecPos);
    }

    public void CloseStatusInfo()
    {
        this.m_uiStatusInfo.Close();
    }

    public float ActiveSummonSkill(uint summonID)
    {
        return this.m_uiSummonSkill.Init(summonID);
    }

    public void SetTurn(bool isPlayerTurn)
    {
        this.m_uiTurnInfo.SetTurn(isPlayerTurn);
    }

    public void OpenRuneInfo(uint runeID, Vector3 vecPos)
    {
        this.m_uiRuneInfo.Open(runeID, vecPos);
    }

    public void CloseRuneInfo()
    {
        this.m_uiRuneInfo.Close();
    }
}