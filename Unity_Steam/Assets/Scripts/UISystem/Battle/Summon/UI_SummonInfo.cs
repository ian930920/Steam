using UnityEngine;
using TMPro;

public class UI_SummonInfo : MonoBehaviour
{
    [SerializeField] private UI_ManaSlot[] m_arrManaSlot = null;
    [SerializeField] private UI_Rune m_uiRune = null;

    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TextMeshProUGUI m_textDesc = null;
    [SerializeField] private TextMeshProUGUI m_textTurn = null;

    [SerializeField] private LayoutUpdater m_layoutUpdater = null;

    public void RefreshUI(Summon summon)
    {
        //설명
        this.m_textTitle.text = ProjectManager.Instance.Table.Skill.GetString_Title(summon.Skill.SkillID);
        this.m_textDesc.text = ProjectManager.Instance.Table.Skill.GetString_Desc(summon.Skill.SkillID, summon.Damage);
        
        //쿨타임
        this.m_textTurn.text = $"{ProjectManager.Instance.Table.Skill.GetData(summon.Skill.SkillID).cooldown}";

        //마나 비용
        int nCost = (int)summon.Cost;
        for(int i = 0, nMax = this.m_arrManaSlot.Length; i < nMax; ++i)
        {
            this.m_arrManaSlot[i].gameObject.SetActive(i < nCost);
        }
        this.m_layoutUpdater.Refresh();

        //룬 표기
        this.m_uiRune.Init(summon.SummonID);
    }
}