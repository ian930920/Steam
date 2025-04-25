using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_TurnInfo : MonoBehaviour
{
    private enum eTURN
    {
        Player,
        Enemy
    }

    private static readonly string[] STR_ARR_TITLE =
    {
        "내 턴",
        "적 턴",
    };

    [SerializeField] private Image m_imgBack = null;
    [SerializeField] private Sprite[] m_arrSprite = null;

    [SerializeField] private TextMeshProUGUI m_textTitle = null;

    public void SetTurn(bool isPlayerTurn)
    {
        eTURN eTurn = isPlayerTurn == true ? eTURN.Player : eTURN.Enemy;
        this.m_imgBack.sprite = this.m_arrSprite[(int)eTurn];
        this.m_textTitle.text = STR_ARR_TITLE[(int)eTurn];
    }
}