using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.Intrinsics;

public class UI_SummonSlot : MonoBehaviour
{
    public Summon Summon { get; private set; } = null;

    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private Image m_imgSelect = null;
    [SerializeField] private Image m_imgTurn = null;
    [SerializeField] private TextMeshProUGUI m_textTurn = null;

    public void Init(Summon summon)
    {
        this.Summon = summon;

        this.SetSelect(false);
        this.gameObject.SetActive(true);

        this.m_imgIcon.sprite = ProjectManager.Instance.Table.Summon.GetSprite(this.Summon.Data.tableID);
        this.RefreshSlot();
    }

    public void RefreshSlot()
    {
        bool isCoolTime = this.Summon.RemainTurn > 0;
        this.m_textTurn.text = isCoolTime ? this.Summon.RemainTurn.ToString(): "";
        this.m_imgTurn.enabled = isCoolTime;
    }

    public void SetSelect(bool bSelect)
    {
        this.m_imgSelect.enabled = bSelect;
    }
}