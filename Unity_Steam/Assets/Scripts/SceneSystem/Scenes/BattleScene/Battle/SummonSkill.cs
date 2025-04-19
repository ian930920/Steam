using UnityEngine;

public class SummonSkill : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_renderer = null;
    [SerializeField] private Animation m_animation = null;
    [SerializeField] private Fx_SpriteAnimation m_fx = null;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void Init(uint summonID)
    {
        this.gameObject.SetActive(true);

        this.m_renderer.sprite = ProjectManager.Instance.Table.Summon.GetSprite(summonID);
        this.m_animation.Play();
        this.m_fx.Play();
    }

    public void InactiveGameOjbect()
    {
        this.gameObject.SetActive(false);
    }
}