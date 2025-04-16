using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD_Battle : BaseHUD
{
    [SerializeField] private UI_Summon m_uiSummon = null;
    [SerializeField] private UI_Mana m_uiMana = null;

    public int SelectedSkillIdx => this.m_uiSummon.SelectedIdx;

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

    public void InitManaUI(ulong nMaxMana)
    {
        this.m_uiMana.Init(nMaxMana);
    }

    public void RefreshMana(ulong nCurrMana)
    {
        this.m_uiMana.Refresh(nCurrMana);
    }
}