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

        this.m_nSelectIdx = -1;

        this.m_listSummon = TableManager.Instance.Summon.GetRandomList(this.m_arrSlot.Length);
        for(int i = 0, nMax = this.m_arrSlot.Length; i < nMax; ++i)
        {
            this.m_arrSlot[i].RefreshSlot(this.m_listSummon[i].tableID);
            this.m_arrSlot[i].RefreshSelect(false);
        }

        //선택안했으니까 안했다고
        this.m_csbtnSelect.RefreshActive(false);

        this.m_gobjMySummon.SetActive(UserDataManager.Instance.Summon.SummonCount > 0);
        
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

        //선택 소환수 저장
        UserDataManager.Instance.Summon.AddSummon(this.m_listSummon[this.m_nSelectIdx].tableID);

        //소환수 얻었다고 저장
        UIManager.Instance.PopupSystem.OpenRewardSummonPopup(this.m_listSummon[this.m_nSelectIdx].tableID, () =>
        {
            //3개인지 확인
            if(UserDataManager.Instance.Summon.SummonCount < 3)
            {
                //더 뽑기
                UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.SummonSelect);
            }
            else
            {
                //시나리오로 넘기기
                SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Scenario);
            }
        });

        //팝업닫기
        this.OnCloseClicked();
    }

    public void OnInactiveClicked()
    {
        UIManager.Instance.PopupSystem.OpenSystemTimerPopup("캐릭터를 선택하세요");
    }

    public void OnMySummonClicked()
    {
        UIManager.Instance.PopupSystem.OpenPopup(ePOPUP_ID.Summon);
    }
}
