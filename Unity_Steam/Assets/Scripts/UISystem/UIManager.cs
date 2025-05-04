using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : BaseManager<UIManager>
{
    public enum eUI_TYPE
    {
        HUD,
        Popup_Main,
        Popup_Small,
    }

    public enum eUI_BUTTON_STATE
    {
        Active,
        Inactive
    }

    #region Screen
    static public float PIXELS_PER_UINIT = 0.01f;
    static public Vector2 VEC2_SCREEN_REF = new Vector2(1920, 1080);
    static public float SCREEN_REF_RATE = VEC2_SCREEN_REF.x / VEC2_SCREEN_REF.y;

    //화면 비율
    private float m_fScreenRate = 0.0f;
    public Vector2 ScaledScreen { get; private set; } = new Vector2();
    #endregion

    #region Define Color
    static readonly public Color COLOR_BTN_ACTIVE = Color.white;
    static readonly public Color COLOR_BTN_INACTIVE = Color.grey;
    #endregion

    public PopupSystem PopupSystem { get; private set; } = new PopupSystem();

    public UI_Indicator Indicator { get; private set; } = null;

    protected override void init()
    {
        //화면 비율 저장
        this.m_fScreenRate = Screen.width / (float)Screen.height;
        this.ScaledScreen = new Vector2(1 / (Screen.width * PIXELS_PER_UINIT), 1 / (Screen.height * PIXELS_PER_UINIT));

        this.Indicator = UI_Indicator.Init(this.transform, UI_Indicator.STR_PATH_INDICATOR);
        this.SetUIScaler(this.Indicator.CanvasScaler);
    }

    public void SetUIScaler(CanvasScaler scaler)
    {
        //Screen Mode 변경
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = VEC2_SCREEN_REF;

        //기본은 height 매치
        //screen size가 refer보다 작다면 width로 변경
        scaler.matchWidthOrHeight = this.m_fScreenRate > SCREEN_REF_RATE ? 1 : 0;
    }
}