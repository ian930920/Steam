using UnityEngine;

public class NPC : MonoBehaviour
{


    [SerializeField] private SpriteRenderer m_renderer = null;
    [SerializeField] private UI_CharacterStatusBar m_statusBar = null;

    public void Init()
    {
        if(this.m_statusBar == null) this.m_statusBar.Init(this.transform.position, 1000);
    }
}
