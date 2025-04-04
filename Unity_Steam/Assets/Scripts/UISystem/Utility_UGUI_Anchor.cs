using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}

public static class RectTransformExtensions
{
    public static void SetAnchor(this RectTransform trSource, AnchorPresets allign, float fOffsetX = 0.0f, float fOffsetY = 0.0f)
    {
        trSource.anchoredPosition = new Vector3(fOffsetX, fOffsetY, 0);

        switch(allign)
        {
            case AnchorPresets.TopLeft:
            {
                trSource.anchorMin = new Vector2(0, 1);
                trSource.anchorMax = new Vector2(0, 1);
            }
            break;
            case AnchorPresets.TopCenter:
            {
                trSource.anchorMin = new Vector2(0.5f, 1);
                trSource.anchorMax = new Vector2(0.5f, 1);
            }
            break;
            case AnchorPresets.TopRight:
            {
                trSource.anchorMin = new Vector2(1, 1);
                trSource.anchorMax = new Vector2(1, 1);
            }
            break;

            case AnchorPresets.MiddleLeft:
            {
                trSource.anchorMin = new Vector2(0, 0.5f);
                trSource.anchorMax = new Vector2(0, 0.5f);
            }
            break;
            case AnchorPresets.MiddleCenter:
            {
                trSource.anchorMin = new Vector2(0.5f, 0.5f);
                trSource.anchorMax = new Vector2(0.5f, 0.5f);
            }
            break;
            case AnchorPresets.MiddleRight:
            {
                trSource.anchorMin = new Vector2(1, 0.5f);
                trSource.anchorMax = new Vector2(1, 0.5f);
            }
            break;

            case AnchorPresets.BottomLeft:
            {
                trSource.anchorMin = new Vector2(0, 0);
                trSource.anchorMax = new Vector2(0, 0);
            }
            break;
            case AnchorPresets.BottonCenter:
            {
                trSource.anchorMin = new Vector2(0.5f, 0);
                trSource.anchorMax = new Vector2(0.5f, 0);
            }
            break;
            case AnchorPresets.BottomRight:
            {
                trSource.anchorMin = new Vector2(1, 0);
                trSource.anchorMax = new Vector2(1, 0);
            }
            break;

            case AnchorPresets.HorStretchTop:
            {
                trSource.anchorMin = new Vector2(0, 1);
                trSource.anchorMax = new Vector2(1, 1);
            }
            break;
            case AnchorPresets.HorStretchMiddle:
            {
                trSource.anchorMin = new Vector2(0, 0.5f);
                trSource.anchorMax = new Vector2(1, 0.5f);
            }
            break;
            case AnchorPresets.HorStretchBottom:
            {
                trSource.anchorMin = new Vector2(0, 0);
                trSource.anchorMax = new Vector2(1, 0);
            }
            break;

            case AnchorPresets.VertStretchLeft:
            {
                trSource.anchorMin = new Vector2(0, 0);
                trSource.anchorMax = new Vector2(0, 1);
            }
            break;
            case AnchorPresets.VertStretchCenter:
            {
                trSource.anchorMin = new Vector2(0.5f, 0);
                trSource.anchorMax = new Vector2(0.5f, 1);
            }
            break;
            case AnchorPresets.VertStretchRight:
            {
                trSource.anchorMin = new Vector2(1, 0);
                trSource.anchorMax = new Vector2(1, 1);
            }
            break;

            case AnchorPresets.StretchAll:
            {
                trSource.anchorMin = new Vector2(0, 0);
                trSource.anchorMax = new Vector2(1, 1);
            }
            break;
        }
    }
}
