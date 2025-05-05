using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Popup_SummonSelect : BasePopup
{
    [SerializeField] private UI_SummonSelectSlot[] m_arrSlot = null;
    [SerializeField] private Button_ChangeState m_csbtnSelect = null;
    [SerializeField] private GameObject m_gobjMySummon = null;

    private List<TableData.TableData_Summon> m_listSummon = null;

    private int m_nSelectIdx = -1;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.m_listSummon = ProjectManager.Instance.Table.Summon.GetRandomList(this.m_arrSlot.Length);
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            this.m_arrSlot[i].RefreshSlot(this.m_listSummon[i]);
            this.m_arrSlot[i].RefreshSelect(false);
        }

        //선택안했으니까 안했다고
        this.m_csbtnSelect.RefreshActive(false);

        this.m_gobjMySummon.SetActive(ProjectManager.Instance.UserData.Summon.SummonCount > 0);
        
        return this;
    }

    public void OnSelectSlot(int nIdx)
    {
        if(this.m_csbtnSelect.State == UIManager.eUI_BUTTON_STATE.Inactive) this.m_csbtnSelect.RefreshActive(true);

        if(this.m_nSelectIdx >= 0) this.m_arrSlot[this.m_nSelectIdx].RefreshSelect(false);

        this.m_nSelectIdx = nIdx;
        this.m_arrSlot[this.m_nSelectIdx].RefreshSelect(true);
    }

    public void OnGameStartClicked()
    {
        if(this.m_csbtnSelect.State == UIManager.eUI_BUTTON_STATE.Inactive)
        {
            this.m_csbtnSelect.OnClicked();
            return;
        }

        //ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("저장하고 게임 시작");
        ProjectManager.Instance.UserData.Summon.AddSummon(this.m_listSummon[this.m_nSelectIdx].tableID);

        //팝업닫기
        this.OnCloseClicked();

        //게임 시작
        ProjectManager.Instance.Scene.GetCurrScene<TitleScene>().GameStart();
        //유저 데이터에 저장
        //ProjectManager.Instance.Scene.GetCurrScene<TitleScene>().
    }

    public void OnInactiveClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup("캐릭터를 선택하세요");
    }

    public void OnMySummonClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenPopup(ePOPUP_ID.Summon);
    }
}
