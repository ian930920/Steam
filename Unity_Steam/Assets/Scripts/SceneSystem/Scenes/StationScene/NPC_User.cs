using UnityEngine;

public class NPC_User : MonoBehaviour
{
    [SerializeField] private UI_CharacterStatusBar m_statusBar = null;
    [SerializeField] private Transform m_transTartgetUI = null;

    private static readonly string[] STR_DESC =
    {
        "냐옹",
        "여행이다옹",
        "닝겐조아",
        "게임 대박나라옹ㅋㅋ",
    };

    public void Init()
    {
        var stat = UserDataManager.Instance.Session.DefaultStat;

        this.m_statusBar.Init(this.transform.position, stat.GetStat(Stat_Character.eTYPE.HP));

        var enumStatus = UserDataManager.Instance.Session.EnumDicStatus;
        while(enumStatus.MoveNext())
        {
            this.m_statusBar.UpdateStatus(enumStatus.Current.Key, enumStatus.Current.Value);
        }

        ObjectPoolManager.Instance.ActiveDialogue("이곳은 뭐하는 곳이지?", Camera.main.WorldToScreenPoint(this.m_transTartgetUI.position));
    }


    private void OnMouseDown()
    {
        //TODO 말풍선
        ObjectPoolManager.Instance.ActiveDialogue(STR_DESC[Random.Range(0, STR_DESC.Length)], Camera.main.WorldToScreenPoint(this.m_transTartgetUI.position));
    }
}