using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Popup_RewardBox : BasePopup
{
    private enum eSTATE
    {
        Idle,
        Open,
    }

    private readonly static string[] STR_ANIM =
    {
        "Anim_Idle",
        "Anim_BoxOpen",
    };

    [SerializeField] private Animation m_anim = null;

    private stItem m_stReward;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose = null)
    {
        base.OpenPopup(nOreder, funcClose);

        //TODO 랜덤
        var random = Random.Range(0, 1.0f);
        if(random > 0.7f) this.m_stReward = new stItem(TableManager.Instance.Rune.GetRandomData().tableID, 1);
        else if(random > 0.3f) this.m_stReward = new stItem(TableData.TableItem.eID.Ticket, 1);
        else this.m_stReward = new stItem(TableData.TableItem.eID.Ticket, 3);

        this.m_anim.Play(STR_ANIM[(int)eSTATE.Idle]);

        return this;
    }

    public void OnBoxClicked()
    {
        this.m_anim.Play(STR_ANIM[(int)eSTATE.Open]);

        StartCoroutine("coReciveReward");
    }

    private IEnumerator coReciveReward()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(this.m_anim.GetClip(STR_ANIM[(int)eSTATE.Open]).length);

        UIManager.Instance.PopupSystem.OpenRewardItemPopup(this.m_stReward, () =>
        {
            if(SceneManager.Instance.CurrSceneID == SceneManager.eSCENE_ID.Battle)
            {
                SceneManager.Instance.GetCurrScene<BattleScene>().NextStep();
            }
        });
        
        //아이템 저장
        UserDataManager.Instance.Inventory.AddItem(this.m_stReward);

        this.OnCloseClicked();
    }
}