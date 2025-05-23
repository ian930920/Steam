using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Popup_Event : BasePopup
{
    [Header("이벤트 내용")]
    [SerializeField] private GameObject m_gobjEvent = null;
    [SerializeField] private TextMeshProUGUI m_textTitle = null;
    [SerializeField] private TypewriterByCharacter m_textDesc = null;
    [SerializeField] private GameObject m_gobjBtn = null;
    [SerializeField] private Button_Event_Option[] m_arrBtnOption = null;
    
    [Space(5)][Header("이벤트 결과")]
    [SerializeField] private GameObject m_gobjResult = null;
    [SerializeField] private TypewriterByCharacter m_textResult = null;
    [SerializeField] private Button m_btnClose = null;

    private TableData.TableData_Event m_data = null;
    private TableData.TableData_EventOption m_dataOption = null;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        this.m_gobjEvent.SetActive(true);
        this.m_gobjResult.SetActive(false);
        this.m_gobjBtn.SetActive(false);

        return this;
    }

    public void SetEvent(uint eventID)
    {
        this.m_data = TableManager.Instance.Event.GetData(eventID);
        if(this.m_data == null)
        {
            //진행할 이벤트 없으면 팝업 닫기
            this.OnCloseClicked();
            return;
        }
        
        this.m_textTitle.text = TableManager.Instance.Event.GetString(eventID);
        this.m_textDesc.ShowText(TableManager.Instance.Event.GetString(eventID, TableData.TableString.eTYPE.Description));

        for(int i = 0, nMax = this.m_arrBtnOption.Length; i < nMax; ++i)
        {
            if(this.m_data.arrOption.Count <= i)
            {
                this.m_arrBtnOption[i].gameObject.SetActive(false);
                continue;
            }

            this.m_arrBtnOption[i].Init(this.m_data.arrOption[i]);
        }
    }

    public void ReciveReward(uint eventOptionID)
    {
        this.m_dataOption = TableManager.Instance.EventOption.GetData(eventOptionID);

        //결과
        this.setResult(TableManager.Instance.EventOption.GetString_Result(eventOptionID));

        //이벤트 저장
        UserDataManager.Instance.Session.AddEvent(this.m_data.tableID);
    }

    private void setResult(string strDesc)
    {
        this.m_gobjEvent.SetActive(false);
        this.m_gobjResult.SetActive(true);
        this.m_btnClose.enabled = false;
        this.m_textResult.ShowText(strDesc);
    }

    private void doResult()
    {
        switch((TableData.TableEventOption.eREWARD_TYPE)this.m_dataOption.rewardType)
        {
            case TableData.TableEventOption.eREWARD_TYPE.Tichek:
            case TableData.TableEventOption.eREWARD_TYPE.Rune:
            {
                stItem itemReward = new stItem((uint)this.m_dataOption.rewardID, this.m_dataOption.rewardValue);
                UserDataManager.Instance.Inventory.AddItem(itemReward);
                UIManager.Instance.PopupSystem.OpenRewardItemPopup(itemReward, this.nextStep);
            }
            break;

            case TableData.TableEventOption.eREWARD_TYPE.Summon:
            {
                uint summonID = (uint)this.m_dataOption.rewardID;
                UserDataManager.Instance.Summon.AddSummon(summonID);
                UIManager.Instance.PopupSystem.OpenRewardSummonPopup(summonID, () =>
                {
                    if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle)
                    {
                        SceneManager.Instance.GetCurrScene<BattleScene>().NextStep();
                        SceneManager.Instance.GetCurrScene<BattleScene>().User_InitSummon();
                    }
                });
            }
            break;

            case TableData.TableEventOption.eREWARD_TYPE.Heal:
            {
                UserDataManager.Instance.Session.Heal(this.m_dataOption.rewardValue);
            }
            break;
        }

        switch((TableData.TableEventOption.eREWARD_TYPE)this.m_dataOption.rewardType)
        {
            case TableData.TableEventOption.eREWARD_TYPE.Tichek:
            case TableData.TableEventOption.eREWARD_TYPE.Rune:
            case TableData.TableEventOption.eREWARD_TYPE.Summon:
            {
            }
            break;

            default:
            {
                if(this.m_dataOption.isBattle == 1)
                {
                    if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle)
                    {
                        SceneManager.Instance.GetCurrScene<BattleScene>().BattleStart();
                    }
                }
                else this.nextStep();
            }
            break;
        }
    }

    private void nextStep()
    {
        if(SceneManager.Instance.CurrSceneID != SceneManager.eSCENE_ID.Battle) return;

        SceneManager.Instance.GetCurrScene<BattleScene>().NextStep();
    }

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();

        //결과 실행
        this.doResult();
    }
}