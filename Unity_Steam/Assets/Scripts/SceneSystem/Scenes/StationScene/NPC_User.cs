using UnityEngine;

public class NPC_User : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_renderer = null;
    [SerializeField] private UI_CharacterStatusBar m_statusBar = null;

    public void Init()
    {
        var stat = UserDataManager.Instance.Session.DefaultStat;

        this.m_statusBar.Init(this.transform.position, stat.GetStat(Stat_Character.eTYPE.HP));

        /*
        var status = 
        for()
        {

        }
        this.m_statusBar.UpdateStatus(stat.);
        */
    }
}