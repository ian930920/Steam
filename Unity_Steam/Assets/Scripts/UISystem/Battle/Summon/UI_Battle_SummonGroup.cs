using System.Collections.Generic;
using UnityEngine;

public class UI_Battle_SummonGroup : MonoBehaviour
{
    [SerializeField] private UI_Battle_SummonSlot[] m_arrSlot = null;

    [SerializeField] private UI_RuneGroup m_uiRune = null;
    [SerializeField] private UI_Battle_SummonInfo m_uiInfo = null;

    public int SelectedIdx { get; private set; } = 0;

    private Summon m_currSummon = null;

    public void Init(List<Summon> listSummon)
    {
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(i >= listSummon.Count)
            {
                this.m_arrSlot[i].gameObject.SetActive(false);
                continue;
            }

            this.m_arrSlot[i].Init(listSummon[i]);
        }

        //맨 처음꺼 선택
        this.OnSelectClicked(0);
    }

    public void Refresh()
    {
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(this.m_arrSlot[i].gameObject.activeSelf == false) continue;

            this.m_arrSlot[i].RefreshSlot();
        }
    }

    public void RefreshSlot()
    {
        this.m_arrSlot[this.SelectedIdx].RefreshSlot();
    }

    public void OnSelectClicked(int nIdx)
    {
        if(nIdx != this.SelectedIdx) this.m_arrSlot[this.SelectedIdx].SetSelect(false);

        this.SelectedIdx = nIdx;
        this.m_arrSlot[this.SelectedIdx].SetSelect(true);
        this.m_currSummon = this.m_arrSlot[this.SelectedIdx].Summon;

        this.m_uiInfo.RefreshUI(this.m_currSummon);

        //유저 스킬 저장
        ProjectManager.Instance.BattleScene?.User_SelectSkill();
    }
}