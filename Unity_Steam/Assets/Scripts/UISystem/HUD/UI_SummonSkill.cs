using UnityEngine;
using UnityEngine.UI;

public class UI_SummonSkill : MonoBehaviour
{
    [SerializeField] private Image m_imgSummon = null;
    [SerializeField] private Animation m_animation = null;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public float Init(uint summonID)
    {
        this.gameObject.SetActive(true);

        this.m_imgSummon.sprite = TableManager.Instance.Summon.GetSprite(summonID);
        this.m_animation.Play();

        return this.m_animation.clip.length;
    }

    public void InactiveGameOjbect()
    {
        this.gameObject.SetActive(false);
    }
}