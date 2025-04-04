using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_ChangeState_Cost : Button_ChangeState
{
    [SerializeField] private Image m_imgIcon = null;
    [SerializeField] private TextMeshProUGUI m_textCost = null;

    private stItem m_stItemInfo;

    public override void InitStateButton(ChangeStateBtnClickedEvent onActive, ChangeStateBtnClickedEvent onInactive = null)
    {
        if(onInactive == null) base.InitStateButton(onActive, this.onFailClicked);
        else base.InitStateButton(onActive, onInactive);
    }

    public void RefreshItemInfo(stItem stItemInfo, bool isActive = true)
    {
        this.m_stItemInfo = stItemInfo;
        this.m_imgIcon.sprite = ProjectManager.Instance.Table.Item.GetSprite(this.m_stItemInfo.ItemID);
        this.m_textCost.text = ProjectManager.Instance.Table.Item.GetString_ItemCount(this.m_stItemInfo);

        //TODO UserDataManager 재화있는지 확인
        //if(isActive == true) isActive = UserDataManager.Instance.IsUseableItem(this.m_stItemInfo);

        base.RefreshActive(isActive);
    }

    private void onFailClicked()
    {
        ProjectManager.Instance.UI.PopupSystem.OpenSystemTimerPopup($"{ProjectManager.Instance.Table.Item.GetString(this.m_stItemInfo.ItemID)} +STR 부족");
    }
}