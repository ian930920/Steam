using System.Linq;
using UnityEngine;

public class UI_Battle_SummonInfo : MonoBehaviour
{
    [SerializeField] private UI_MySummonInfo m_uiSummonInfo = null;
    [SerializeField] private UI_StatusInfo[] m_arrStatusInfo = null;

    public void Active(uint summonID)
    {
        this.gameObject.SetActive(true);

        this.m_uiSummonInfo.Refresh(summonID);

        //스킬에 상태이상이있다면 활성화
        int nStatusIdx = 0;
        var skillStatus = TableManager.Instance.Skill.GetDataBySummonID(summonID).listStatusID;
        for(int i = nStatusIdx, nMax = skillStatus.Count; i < nMax; ++i)
        {
            this.m_arrStatusInfo[i].SetStatusInfo(skillStatus[i]);
        }

        //룬에 상태이상이있다면 활성화
        nStatusIdx = skillStatus.Count;
        var summonRuneStatus = UserDataManager.Instance.Summon.GetSummon(summonID).StatAdditional.DicStatus.Keys.ToArray();
        for(int i = 0, nMax = summonRuneStatus.Length; i < nMax; ++i)
        {
            this.m_arrStatusInfo[i + nStatusIdx].SetStatusInfo(summonRuneStatus[i]);
        }
    }

    public void Inactive()
    {
        this.gameObject.SetActive(false);

        //상태이상 모두 닫기
        for(int i = 0, nMax = this.m_arrStatusInfo.Length; i < nMax; ++i)
        {
            this.m_arrStatusInfo[i].Inactive();
        }
    }
}