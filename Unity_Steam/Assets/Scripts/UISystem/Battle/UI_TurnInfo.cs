using UnityEngine;

public class UI_TurnInfo : MonoBehaviour
{
    private enum eTURN
    {
        Player,
        Enemy,
        End
    }

    [SerializeField] private GameObject[] m_arrGobj = null;

    public void ResetUI()
    {
        for(int i = 0, nMax = (int)eTURN.End; i < nMax; ++i)
        {
            this.m_arrGobj[i].SetActive(false);
        }
    }

    public void SetTurn(bool isPlayerTurn)
    {
        eTURN eTurn = isPlayerTurn == true ? eTURN.Player : eTURN.Enemy;
        for(int i = 0, nMax = (int)eTURN.End; i < nMax; ++i)
        {
            this.m_arrGobj[i].SetActive(i == (int)eTurn);
        }
    }
}