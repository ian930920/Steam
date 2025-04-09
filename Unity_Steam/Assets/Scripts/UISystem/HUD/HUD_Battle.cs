using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD_Battle : BaseHUD
{
    [SerializeField] private UI_Summon m_uiSummon = null;

    public override void RefreshUI()
    {

    }

    public void InitSummonUI(List<Summon> listSummon)
    {
        this.m_uiSummon.Init(listSummon);
    }
}