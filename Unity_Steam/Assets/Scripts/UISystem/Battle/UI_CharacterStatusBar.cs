using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_CharacterStatusBar : MonoBehaviour
{
    [SerializeField] private Slider m_sliderHP = null;
    [SerializeField] private TextMeshProUGUI m_textHP = null;

    [SerializeField] private UI_StatusSlot[] m_arrStatus = null;

    [SerializeField] private Image m_imgFill = null;
    [SerializeField] private Color m_colorDefault;
    [SerializeField] private Color m_colorShield;

    //Key : StatusID, Value : StatusSlot
    private Dictionary<uint, UI_StatusSlot> m_dicStatus = new Dictionary<uint, UI_StatusSlot>();

    public void Init(Vector3 vecPos, int nHP)
    {
        this.transform.position = new Vector3(vecPos.x, vecPos.y - 20, vecPos.z);

        this.m_sliderHP.minValue = 0;
        this.m_sliderHP.maxValue = nHP;
        this.RefreshGauge(nHP);

        this.ResetStatus();

        this.gameObject.SetActive(true);
    }

    public void RefreshGauge(int nHP)
    {
        this.m_sliderHP.value = Mathf.Clamp(nHP, 0, int.MaxValue);
        this.m_textHP.text = $"{this.m_sliderHP.value}/{this.m_sliderHP.maxValue}";
    }

    public void AddShield(int nShield)
    {
        this.m_imgFill.color = this.m_colorShield;

        this.m_textHP.text = $"{nShield}";
    }

    public void RemoveShield()
    {
        this.m_imgFill.color = this.m_colorDefault;
    }

    public void ResetStatus()
    {
        this.m_dicStatus.Clear();
        for(int i = 0, nMax = this.m_arrStatus.Length; i < nMax; ++i)
        {
            if(this.m_arrStatus[i].gameObject.activeSelf == false) continue;

            this.m_arrStatus[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 상태이상 업데이트
    /// </summary>
    /// <param name="statusID">status 테이블 ID</param>
    /// <param name="turn">남은 턴</param>
    /// <returns>지워지는지 여부</returns>
    public bool UpdateStatus(uint statusID, int turn)
    {
        if(this.m_dicStatus.ContainsKey(statusID) == false)
        {
            for(int i = 0, nMax = this.m_arrStatus.Length; i < nMax; ++i)
            {
                if(this.m_arrStatus[i].gameObject.activeSelf == true) continue;

                this.m_dicStatus.Add(statusID, this.m_arrStatus[i]);
                this.m_dicStatus[statusID].Init(statusID);
                break;
            }
        }

        bool isRemove = this.m_dicStatus[statusID].UpdateTurn(turn);
        if(isRemove == true)
        {
            this.m_dicStatus.Remove(statusID);

            //정보 켜뒀다면 닫기
            UIManager.Instance.PopupSystem.ClosePopup(ePOPUP_ID.StatusInfo);
        }

        return isRemove;
    }
}