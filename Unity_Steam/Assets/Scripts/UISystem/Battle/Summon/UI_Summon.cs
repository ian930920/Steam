using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Summon : MonoBehaviour
{
    [SerializeField] private UI_SummonSlot[] m_arrSlot = null;
    [SerializeField] private UI_ManaSlot[] m_arrManaSlot = null;
    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;
    [SerializeField] private TextMeshProUGUI m_textTurn = null;

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

        //설명
        this.m_textTitle.text = ProjectManager.Instance.Table.Skill.GetString_Title(this.m_currSummon.Data.skillID);
        this.m_textDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(this.m_currSummon.Data.skillID, this.m_currSummon.Skill.GetDefaultDamage());
        
        //쿨타임
        this.m_textTurn.text = $"{ProjectManager.Instance.Table.Skill.GetData(this.m_currSummon.Data.skillID).cooldown}";

        //마나 비용
        int nCost = (int)this.m_currSummon.Data.cost;
        for(int i = 0, nMax = this.m_arrManaSlot.Length; i < nMax; ++i)
        {
            this.m_arrManaSlot[i].gameObject.SetActive(i < nCost);
        }

        //유저 스킬 저장
        ProjectManager.Instance.BattleScene?.SelectUserSkill(this.SelectedIdx);
    }
}