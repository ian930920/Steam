using System.Collections.Generic;
using UnityEngine;

public class UI_Battle_SummonGroup : MonoBehaviour
{
    [SerializeField] private UI_Battle_SummonSlot[] m_arrSlot = null;

    public int SelectedIdx { get; private set; } = 0;

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
        if(this.m_arrSlot[nIdx].IsCooldown == true) return;

        if(nIdx != this.SelectedIdx) this.m_arrSlot[this.SelectedIdx].SetSelect(false);

        this.SelectedIdx = nIdx;
        this.m_arrSlot[this.SelectedIdx].SetSelect(true);

        //유저 스킬 저장
        ProjectManager.Instance.BattleScene?.User_SelectSkill();
    }
}