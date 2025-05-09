using System;
using System.Collections.Generic;
using UnityEngine;

public class Utility_Time
{
    //Time
    static public int CONST_TIME_ONEMIN = 60;
    static public int CONST_TIME_ONEHOUR = 60 * 60;
    static public int CONST_TIME_ONEDAY = 60 * 60 * 24;
    static public float CONST_TIME_ONESEC = 1.0f;
    static public float CONST_TIME_ONEMIN_FOR_MUL = 1 / 60.0f;
    static public float CONST_TIME_ONEHOUR_FOR_MUL = 1 / (60.0f * 60);
    static public float CONST_TIME_ONEDAY_FOR_MUL = 1 / (60.0f * 60 * 24);

    internal static class YieldInstructionCache
    {
        class FloatComparer : IEqualityComparer<float>
        {
            bool IEqualityComparer<float>.Equals(float x, float y)
            {
                return Mathf.Abs(x - y) <= Mathf.Epsilon;
            }

            int IEqualityComparer<float>.GetHashCode(float obj)
            {
                return obj.GetHashCode();
            }
        }

        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

        private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

        public static WaitForSeconds WaitForSeconds(double dSeconds)
        {
            float fSecond = (float)dSeconds;
            WaitForSeconds wfs;
            if(!_timeInterval.TryGetValue(fSecond, out wfs)) _timeInterval.Add(fSecond, wfs = new WaitForSeconds(fSecond));
            return wfs;
        }

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if(!_timeInterval.TryGetValue(seconds, out wfs)) _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }

    static public string GetTimeString(double dTime)
    {
        //(int)(nTime % 60 * 60 / 60)
        int nDay = (int)(dTime / CONST_TIME_ONEDAY);
        int nHour = (int)((dTime % CONST_TIME_ONEDAY) * CONST_TIME_ONEHOUR_FOR_MUL);
        int nMin = (int)((dTime % CONST_TIME_ONEHOUR) * CONST_TIME_ONEMIN_FOR_MUL);
        int nSec = (int)(dTime % CONST_TIME_ONEMIN);

        if(nDay == 0) return $"{nHour:D2}:{nMin:D2}:{nSec:D2}";
        else return $"{nDay}D {nHour:D2}:{nMin:D2}";
    }

    static public string GetTimeStringToText(float fTime)
    {
        int nMin = (int)(fTime * CONST_TIME_ONEMIN_FOR_MUL);
        int nSec = (int)(fTime % CONST_TIME_ONEMIN);

        return "";
        //TODO
        //if(nMin < 1)        return $"{nSec}{TableManager.Instance.String.GetString_TimeUnitType(TimeManager.eUNIT_TYPE.Sec)}";
        //else if(nSec < 1)   return $"{nMin}{TableManager.Instance.String.GetString_TimeUnitType(TimeManager.eUNIT_TYPE.Min)}";
        //else                return $"{nMin}{TableManager.Instance.String.GetString_TimeUnitType(TimeManager.eUNIT_TYPE.Min)}{nSec}{TableManager.Instance.String.GetString_TimeUnitType(TimeManager.eUNIT_TYPE.Sec)}";
    }

    static public string GetCurrTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// resutl : strTime2 - strTime1
    /// </summary>
    /// <returns>The time diff.</returns>
    /// <param name="strTime1">String time1.</param>
    /// <param name="strTime2">String time2.</param>
    static public int GetTimeDiff(string strTime1, string strTime2 = "")
    {
        if(strTime2.Length == 0)
        {
            strTime2 = GetCurrTime();
        }
        DateTime prevTime = Convert.ToDateTime(strTime1);
        DateTime currTime = Convert.ToDateTime(strTime2);

        return GetTimeDiff(prevTime,currTime);
    }

    static public int GetTimeDiff(DateTime prevTime, DateTime currTime)
    {
        TimeSpan timeDiff = currTime - prevTime;

        return (int)timeDiff.TotalSeconds;
    }
}
