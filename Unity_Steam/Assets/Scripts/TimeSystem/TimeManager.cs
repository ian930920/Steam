using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : BaseManager<TimeManager>
{
    public enum eTYPE
    {
        Real = 1,
        Game,
    }

    public enum eUNIT_TYPE
    {
		Sec = 1,
		Min,
		Hour
    }

    private float m_fTimeScale { get; set; } = 1.0f;
    public float DeltaTime { get => this.m_fTimeScale * Time.deltaTime; }

    public readonly float TIME_UPDATE = Utility_Time.CONST_TIME_ONESEC;

    private UTime m_utime = null;

    public delegate float GetTime();

    private List<UnityAction> m_listTimeUpdate = new List<UnityAction>();
    private List<UnityAction> m_listRefreshUI = new List<UnityAction>();

    private UnityEvent m_funcReceiveTime = new UnityEvent();
    public DateTime CurrDateTime { get; private set; }

    public string DailyResetTime { get => Utility_Time.GetTimeString(this.MidnightRemainTime); }

    public float MidnightRemainTime
    {
        get
        {
            DateTime nextDay = this.CurrDateTime.AddDays(1);
            //ProjectManager.Instance.Log($"nextDay {nextDay}");

            nextDay = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
            //ProjectManager.Instance.Log($"new nextDay {nextDay}");
            return (float)(nextDay - this.CurrDateTime).TotalSeconds;
        }
    }

    protected override void init()
    {
        this.m_utime = this.gameObject.AddComponent<UTime>();
        this.m_utime.HasConnection(connection =>
        {
            ProjectManager.Instance.Log($"Connection: {connection}");

            //지금 시간 저장
            this.GetCurrDateTime();
        });
    }

    public override void ResetManager()
    {
        this.m_utime = null;
        this.m_funcReceiveTime.RemoveAllListeners();
        this.m_listTimeUpdate.Clear();
        this.m_listRefreshUI.Clear();
        StopAllCoroutines();
    }

    public void StartTimeUpdate()
    {
        StopAllCoroutines();
        StartCoroutine("coTimeUpdate");
    }

    private IEnumerator coTimeUpdate()
    {
        while(true)
        {
            yield return Utility_Time.YieldInstructionCache.WaitForSeconds(TIME_UPDATE);

            this.timeEventUpdate();

            //현재시간 업데이트
            DateTime currDataTime = this.CurrDateTime.AddSeconds(TIME_UPDATE);

            //TODO 일일 초기화
            //UserDataManager.Instance.Time.CheckDailyReset(currDataTime);

            this.CurrDateTime = currDataTime;
        }
    }

    private void timeEventUpdate()
    {
        for(int i = 0, nMax = this.m_listTimeUpdate.Count; i < nMax; ++i)
        {
            this.m_listTimeUpdate[i].Invoke();
        }

        for(int i = 0, nMax = this.m_listRefreshUI.Count; i < nMax; ++i)
        {
            this.m_listRefreshUI[i].Invoke();
        }
    }

    public UnityAction AddTimeUpdateEvent(UnityAction funcUpdate)
    {
        this.m_listTimeUpdate.Add(funcUpdate);

        return () => StartCoroutine("coRemoveTimeUpdateEvent", funcUpdate);
    }

    private IEnumerator coRemoveTimeUpdateEvent(UnityAction funcUpdate)
    {
        yield return null;

        this.m_listTimeUpdate.Remove(funcUpdate);
    }

    public void AddRefreshUIEvent(UnityAction funcUpdate)
    {
        this.m_listRefreshUI.Add(funcUpdate);
    }

    public void GetCurrDateTime(UnityAction onReceiveTime = null)
    {
        //TODO
        //UIManager.Instance.Indicator.Active = true;

        if(onReceiveTime != null) this.m_funcReceiveTime.AddListener(onReceiveTime);

        this.m_utime.GetUtcTimeAsync(this.onCurrTimeReceived);
    }

    private void onCurrTimeReceived(bool success, string error, DateTime time)
    {
        //TODO
        //UIManager.Instance.Indicator.Active = false;

        if(success == false)
        {
            ProjectManager.Instance.Log(error);
            return;
        }

        //시간 저장
        this.CurrDateTime = time.ToLocalTime();
        //ProjectManager.Instance.Log($"Network time (UTC+0) = {time}\nTo local time = {time.ToLocalTime()}");

        //TODO 새로운 시간 저장할 때마다 유저데이터에 저장된거 갱신하자
        //UserDataManager.Instance.Time.RefreshCurrTime();

        //후처리 함수
        if(this.m_funcReceiveTime == null) return;

        this.m_funcReceiveTime.Invoke();
        this.m_funcReceiveTime.RemoveAllListeners();
    }

    /* 실시간 유저데이터 업데이트용 코드
    #region ServerUpdate
    //private readonly static float TIME_SERVERUPDATE = 5 * Utility_Time.CONST_TIME_ONEMIN;
    private readonly static float TIME_SERVERUPDATE = Utility_Time.CONST_TIME_ONEMIN;
    
    private Dictionary<UserDataManager.eID, Coroutine> m_dicUpdateData = new Dictionary<UserDataManager.eID, Coroutine>();

    public void ServerDataUpdate(UserDataManager.eID eDataID)
    {
        if(this.m_dicUpdateData.ContainsKey(eDataID) == false)
        {
            this.m_dicUpdateData.Add(eDataID, StartCoroutine("coServerDataUpdate", eDataID));
        }
        else
        {
            StopCoroutine(this.m_dicUpdateData[eDataID]);
            this.m_dicUpdateData[eDataID] = StartCoroutine("coServerDataUpdate", eDataID);
        }
    }

    private IEnumerator coServerDataUpdate(UserDataManager.eID eDataID)
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(TIME_SERVERUPDATE);

        //업데이트!!
        yield return UserDataManager.Instance.GetData(eDataID).SaveServerData();
        ProjectManager.Instance.Log($"@@@@@@@@@@주기 저장 완료 {eDataID}");

        //다시 시작!
        this.ServerDataUpdate(eDataID);
    }
    #endregion
    */
}

public class TimerEvent
{
    public float Time { get; private set; } = 0.0f;
    public UnityAction FinishEvent { get; private set; } = null;
    public bool IsFinish { get; private set; } = false;

    public TimerEvent(float fTime, UnityAction funcFinish)
    {
        this.Time = fTime;
        this.FinishEvent = funcFinish;
    }

    public void UpdateTime()
    {
        if(this.IsFinish == true) return;

        this.Time -= Utility_Time.CONST_TIME_ONESEC;

        if(this.Time > 0) return;

        this.IsFinish = true;
        this.FinishEvent?.Invoke();
    }
}