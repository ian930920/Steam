using UnityEngine;

public class StationScene : BaseScene
{
    [Space(5)]
    [SerializeField] private NPC_User m_user = null;
    [SerializeField] private NPC m_npc = null;

    public HUD_Station HUD => base.BaseHUD as HUD_Station;

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        this.m_user.Init();
    }
}