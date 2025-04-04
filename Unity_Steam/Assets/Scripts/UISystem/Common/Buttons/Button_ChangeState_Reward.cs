using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_ChangeState_Reward : Button_ChangeState
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textCost = null;

    public void InitRewardButton(ChangeStateBtnClickedEvent onActive)
    {
        //TODO
        //base.InitStateButton(onActive, () => ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup(ProjectManager.Instance.Table.String.GetString(TableData.TableString.eID.NotEnough_Condition)));
    }

    public void InitItem(stItem stItemInfo)
    {
        //TODO
        //this.m_imgIcon.sprite = ResourceManager.Instance.GetSpriteByItemID(stItemInfo.ItemID);
        //this.m_textCost.text = ProjectManager.Instance.Table.String.GetString_Count(stItemInfo);
    }
}