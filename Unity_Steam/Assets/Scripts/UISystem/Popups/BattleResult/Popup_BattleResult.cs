using UnityEngine;

public class Popup_BattleResult : BasePopup
{
    public enum eRESULT
    {
        Win,
        Defeat,
    }

    private static readonly string[] ARR_STR_ANIM =
    {
        "Anim_Win",
        "Anim_Defeat",
    };

    [SerializeField] private Animation m_anim = null;
    [SerializeField] private GameObject[] m_gobjResult = null;

    private eRESULT m_eResult = eRESULT.Win;

    public void SetResult(eRESULT eResult)
    {
        this.m_eResult = eResult;
        for(int i = 0, nMax = this.m_gobjResult.Length; i < nMax; ++i)
        {
            this.m_gobjResult[i].SetActive((int)this.m_eResult == i);
        }

        this.m_anim.Play(ARR_STR_ANIM[(int)this.m_eResult]);
    }

    public override void OnCloseClicked()
    {
        base.OnCloseClicked();

        switch(this.m_eResult)
        {
            case eRESULT.Win:
            {
                SceneManager.Instance.GetCurrScene<BattleScene>().BattleFinish();
            }
            break;

            case eRESULT.Defeat:
            {
                //타이틀로 돌아가기..
                SceneManager.Instance.ChangeScene(SceneManager.eSCENE_ID.Title);
            }
            break;
        }
    }
}