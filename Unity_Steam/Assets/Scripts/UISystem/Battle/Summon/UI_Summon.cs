using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Summon : MonoBehaviour
{
    [SerializeField] private UI_SummonSlot[] m_arrSlot = null;
    [SerializeField] private TextMeshProUGUI m_textSummonInfo = null;

    private int m_nSelectedIdx = 0;

    public void Init(List<Summon> listSummon)
    {
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            if(i >= listSummon.Count)
            {
                this.m_arrSlot[i].gameObject.SetActive(false);
                continue;
            }

            this.m_arrSlot[i].RefreshSlot(listSummon[i].Data);
        }

        //맨 처음꺼 선택
        this.SetSelect(0);
    }

    public void OnSelectClicked(int nIdx)
    {
        this.m_arrSlot[this.m_nSelectedIdx].SetSelect(false);

        this.SetSelect(nIdx);
    }

    public void SetSelect(int nIdx)
    {
        this.m_nSelectedIdx = nIdx;
        this.m_arrSlot[this.m_nSelectedIdx].SetSelect(true);
        uint summonID = this.m_arrSlot[this.m_nSelectedIdx].SummonID;
        this.m_textSummonInfo.text = $"{ProjectManager.Instance.Table.Summon.GetSkillString(summonID)}\n{ProjectManager.Instance.Table.Summon.GetSkillString(summonID, TableData.TableString.eTYPE.Description)}";

        //유저 스킬 저장
        ProjectManager.Instance.Scene.GetCurrScene<BattleScene>().CharUser.SetCurrSkill(this.m_nSelectedIdx);
    }
}