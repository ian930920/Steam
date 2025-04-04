using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public class Utility_UI
{
    static public string GetCommaNumber<T>(T value)
    {
        if(value.Equals(0)) return value.ToString();
        if(value.Equals(0.0)) return "0";
        if(value.Equals(BigInteger.Zero)) return "0";

        return $"{value:#,##0}";
    }

    static public string GetCommaNumberForDecimalPoint(float fValue)
    {
        return GetCommaNumberForDecimalPoint((decimal)fValue);
    }

    static public string GetCommaNumberForDecimalPoint(double dValue)
    {
        return GetCommaNumberForDecimalPoint((decimal)dValue);
    }

    static public string GetCommaNumberForDecimalPoint(decimal value)
    {
        decimal dResult = Math.Truncate(value * 100) / 100;

        if(value < 1) return $"{dResult:0.##}";

        return $"{dResult:#,###.##}";
    }

    public static readonly string[] STR_WORD_TYPE =
    {
        "","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","aa","ab","ac","ad","ae","af","ag","ah","ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az"
    };

    public static string GetBigIntToABC(BigInteger value)
    {
        if(value < 1000) return value.ToString();

        int word = 0;
        BigInteger divisor = 1000;

        //word 단계 결정
        while (value >= divisor && word + 1 < STR_WORD_TYPE.Length)
        {
            divisor *= 1000;
            word++;
        }

        // word에 맞는 나눗셈 기준
        divisor /= 1000;

        BigInteger main = value / divisor;
        BigInteger remainder = (value % divisor) * 100 / divisor; // 소수점 2자리까지 정수로

        return $"{main}.{remainder:00}{STR_WORD_TYPE[word]}";
    }

    public static float Divide_to_Float(BigInteger A, BigInteger B)
    {
        if(B <= 0) B = 1;
        if(A <= 0) A = 0;

        float fResult = 0;
        string A_value = string.Format("{0}", A);
        string B_value = string.Format("{0}", B);

        if(A >= B) return 1;

        //1보다 작은 경우 float으로 바꿔서 소수점 자리를 뽑아내야함.
        //자리수가 4자리 이상 차이 날경우 그냥 0.001f로 처리.
        if(B_value.Length - A_value.Length > 3)
        {
            return 0.001f;
        }
        else
        {
            //5자리 이하면 그냥 플롯으로 바꿔서 계산해도 괜춘.
            if(B_value.Length < 6)
            {
                fResult = (float.Parse(A_value) / float.Parse(B_value));
            }
            else
            {
                int LENGTH = A_value.Length - 3;
                fResult = (float.Parse(A_value.Substring(0, A_value.Length - LENGTH)) / float.Parse(B_value.Substring(0, B_value.Length - LENGTH)));
            }

            if (fResult < 0) fResult = 0;
            return fResult;
        }
    }

    static public string GetNumberForUnitization<T>(T value)
    {
        string strValue = value.ToString();
        BigInteger biValue = 0;
        if(BigInteger.TryParse(value.ToString(), out biValue) == false) return "failed";

        if(strValue.Length < 4) return $"{value:#,##0}";

        int nUnit = (strValue.Length - 1) / 3;
        char[] str = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        BigInteger result = biValue / BigInteger.Pow(1000, nUnit);
        return $"{result:#,###}{str[nUnit - 1]}";
    }

    static public string GetCountText<T>(T nCount, T nReqCount) where T : IComparable
    {
        if(nCount.CompareTo(nReqCount) < 0) return $"<color=red>{GetNumberForUnitization(nCount)}</color>/{GetNumberForUnitization(nReqCount)}";
        
        return $"{GetNumberForUnitization(nCount)}/{GetNumberForUnitization(nReqCount)}";
    }

    static public void SetTextRectTransformWidth(Text text)
    {
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(text.preferredWidth, rectTransform.sizeDelta.y);
    }

    static public void SetTextRectTransformWidth(TextMeshProUGUI text)
    {
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(text.preferredWidth, rectTransform.sizeDelta.y);
    }

    static public void SetTextRectTransformHeight(TextMeshProUGUI text)
    {
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, text.preferredHeight);
    }

    static public Vector2 GetScaledSpriteSize(SpriteRenderer renderer)
    {
        Vector2 vecScaled;
        vecScaled.x = renderer.sprite.rect.size.x / renderer.sprite.pixelsPerUnit * renderer.transform.lossyScale.x;
		vecScaled.y = renderer.sprite.rect.size.y / renderer.sprite.pixelsPerUnit * renderer.transform.lossyScale.y;
        return vecScaled;
    }
}