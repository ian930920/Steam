using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.Events;

public class UserData_Time : UserData<JsonData_Time>
{
	public enum eSAVE_TYPE
    {
		Define,
    }

	public abstract class BaseTimeData
	{
		public uint ID { get; set; }
		public double RemainTime { get; set; }
		public bool IsActive { get => this.RemainTime > 0; }

		//TimeManager 이벤트 삭제 함수
		private UnityAction m_funcRemoveTimeEvent = null;

		//끝나고 실행할 함수
		private UnityAction m_onFinishedEvent = null;

		virtual public void SetTime(uint timeID, double dRemainTime)
        {
			this.ID = timeID;
			this.RemainTime = dRemainTime;
        }

		virtual public void AddTime(uint timeID, double dRemainTime)
        {
			this.ID = timeID;
			this.RemainTime += dRemainTime;
        }

		public void AddTimeUpdateEvent()
        {
			if(this.m_funcRemoveTimeEvent != null) return;

			//타임 매니저에 등록
			this.m_funcRemoveTimeEvent = TimeManager.Instance.AddTimeUpdateEvent(this.updateEvent);

			if(this.IsActive == false) this.onTimeUpdateFinished();
        }

		protected void refreshTimeUpdate()
		{
			if(this.m_funcRemoveTimeEvent != null)
			{
				this.m_funcRemoveTimeEvent.Invoke();
				this.m_funcRemoveTimeEvent = null;
			}

			this.AddTimeUpdateEvent();
		}

		private void updateEvent()
        {
			this.RemainTime -= Utility_Time.CONST_TIME_ONESEC;

			//끝난 후처리
			if(this.IsActive == false) this.onTimeUpdateFinished();
        }

		protected void onTimeUpdateFinished()
        {
			if(this.m_funcRemoveTimeEvent == null) return;

			//남은시간 0으로 고정하고
			this.RemainTime = 0;

			//타임 매니저에서 지우기
			this.m_funcRemoveTimeEvent.Invoke();
			this.m_funcRemoveTimeEvent = null;

			//끝 함수 부르기
			if(this.m_onFinishedEvent != null) this.m_onFinishedEvent.Invoke();

			//데이터 저장
			//UserDataManager.Instance.SaveData(UserDataManager.eID.Time);
		}

		public void AddFinishEvent(UnityAction funcFinished)
        {
			//한가지 일 밖에 못함 일단 (중복 세팅 걱정 안해도됨)
			this.m_onFinishedEvent = funcFinished;
        }
	}

	public class RealTimeData : BaseTimeData
    {
		public DateTime EndTime { get; set; }

		public override void SetTime(uint timeID, double dRemainTime)
        {
			base.SetTime(timeID, dRemainTime);
			this.EndTime = TimeManager.Instance.CurrDateTime.AddSeconds(dRemainTime);
			this.RemainTime = (this.EndTime - TimeManager.Instance.CurrDateTime).TotalSeconds;

			//시간 업데이트 함수 등록
			base.AddTimeUpdateEvent();
        }

		public override void AddTime(uint timeID, double dRemainTime)
        {
			base.AddTime(timeID, dRemainTime);
			this.EndTime = this.EndTime.AddSeconds(dRemainTime);
			this.RemainTime = (this.EndTime - TimeManager.Instance.CurrDateTime).TotalSeconds;

			//시간 업데이트 함수 등록
			base.AddTimeUpdateEvent();
        }

		/* 빨리감기 기능
		public void FastForward(double dFastForward)
        {
			this.EndTime = this.EndTime.AddSeconds(-dFastForward);

			this.RefreshRemainTime();
        }
		*/

        public void RefreshRemainTime()
        {
			this.RemainTime = (this.EndTime - TimeManager.Instance.CurrDateTime).TotalSeconds;

			//끝난 후처리
			if(this.IsActive == false) base.onTimeUpdateFinished();
        }
    }

	public class GameTimeData : BaseTimeData
    {
		public override void SetTime(uint timeID, double dRemainTime)
        {
			base.SetTime(timeID, dRemainTime);

			//시간 업데이트 함수 등록
			base.AddTimeUpdateEvent();
        }

		public override void AddTime(uint timeID, double dRemainTime)
        {
			base.AddTime(timeID, dRemainTime);

			//시간 업데이트 함수 등록
			base.refreshTimeUpdate();
        }
    }

	#region PP_KEY
    static public string PP_KEY_TIME = "Time";
    #endregion

    protected override string StrKey => PP_KEY_TIME;

	public DateTime LastUpdateTime => base.Data.LastUpdateTime;
	public DateTime OfflineRewardTime => base.Data.OfflineRewardTime;
	public BigInteger PlayTime => base.Data.PlayTime;

	//재접하면 무조건 false
	public bool IsOfflineRewardReceive = false;

    protected override void dataProcessing()
    {
    }

    public void InitTimeManagerEvent()
    {
		Dictionary<int, Dictionary<uint, RealTimeData>>.Enumerator enumRealDic = base.Data.DicRealTime.GetEnumerator();
		Dictionary<uint, RealTimeData>.Enumerator enumRealTime;
		while(enumRealDic.MoveNext())
        {
			enumRealTime = enumRealDic.Current.Value.GetEnumerator();
			while(enumRealTime.MoveNext())
			{
				enumRealTime.Current.Value.AddTimeUpdateEvent();
			}
        }

		Dictionary<int, Dictionary<uint, GameTimeData>>.Enumerator enumGameDic = base.Data.DicGameTime.GetEnumerator();
		Dictionary<uint, GameTimeData>.Enumerator enumGameTime;
		while(enumGameDic.MoveNext())
        {
			enumGameTime = enumGameDic.Current.Value.GetEnumerator();
			while(enumGameTime.MoveNext())
			{
				enumGameTime.Current.Value.AddTimeUpdateEvent();
			}
        }
    }

	private void saveLastUpdateTime()
    {
		DateTime currTime = TimeManager.Instance.CurrDateTime;

		//저장
		base.Data.LastUpdateTime = currTime;

		//저장
		base.SaveClientData();
    }

	public void CheckDailyReset(DateTime currTime)
    {
		if(DateTime.Compare(this.LastUpdateTime.Date, currTime.Date) >= 0) return;

		base.Data.LastUpdateTime = currTime;

		//저장
		base.SaveClientData();
    }

	public void SaveOfflineRewardTime()
    {
		//저장
		base.Data.OfflineRewardTime = TimeManager.Instance.CurrDateTime;
		
		//저장
		base.SaveClientData();
	}

	public void SetTime(eSAVE_TYPE eSaveType, uint timeID, float fTime)
    {
        switch(this.getType(eSaveType, timeID))
        {
            case TimeManager.eTYPE.Real:
            {
				if(base.Data.DicRealTime.ContainsKey((int)eSaveType) == false) base.Data.DicRealTime.Add((int)eSaveType, new Dictionary<uint, RealTimeData>());
				if(base.Data.DicRealTime[(int)eSaveType].ContainsKey(timeID) == false) base.Data.DicRealTime[(int)eSaveType].Add(timeID, new RealTimeData());
				base.Data.DicRealTime[(int)eSaveType][timeID].SetTime(timeID, fTime);
            }
            break;
            case TimeManager.eTYPE.Game:
            {
				if(base.Data.DicGameTime.ContainsKey((int)eSaveType) == false) base.Data.DicGameTime.Add((int)eSaveType, new Dictionary<uint, GameTimeData>());
				if(base.Data.DicGameTime[(int)eSaveType].ContainsKey(timeID) == false) base.Data.DicGameTime[(int)eSaveType].Add(timeID, new GameTimeData());
				base.Data.DicGameTime[(int)eSaveType][timeID].SetTime(timeID, fTime);
            }
            break;
        }

		//저장
		base.SaveClientData();
    }

	public void SetTime(TableData.TableTime.eID eTimeID, float fTime)
    {
		this.SetTime((int)eSAVE_TYPE.Define, (uint)eTimeID, fTime);
    }

	public void AddTime(eSAVE_TYPE eSaveType, uint timeID, float fTime)
    {
        switch(this.getType(eSaveType, timeID))
        {
            case TimeManager.eTYPE.Real:
            {
				if(base.Data.DicRealTime.ContainsKey((int)eSaveType) == false) base.Data.DicRealTime.Add((int)eSaveType, new Dictionary<uint, RealTimeData>());
				if(base.Data.DicRealTime[(int)eSaveType].ContainsKey(timeID) == false) base.Data.DicRealTime[(int)eSaveType].Add(timeID, new RealTimeData());
				base.Data.DicRealTime[(int)eSaveType][timeID].AddTime(timeID, fTime);
            }
            break;
            case TimeManager.eTYPE.Game:
            {
				if(base.Data.DicGameTime.ContainsKey((int)eSaveType) == false) base.Data.DicGameTime.Add((int)eSaveType, new Dictionary<uint, GameTimeData>());
				if(base.Data.DicGameTime[(int)eSaveType].ContainsKey(timeID) == false) base.Data.DicGameTime[(int)eSaveType].Add(timeID, new GameTimeData());
				base.Data.DicGameTime[(int)eSaveType][timeID].AddTime(timeID, fTime);
            }
            break;
        }

		//저장
		base.SaveClientData();
	}

	private TimeManager.eTYPE getType(eSAVE_TYPE eSaveType, uint timeID)
    {
		switch(eSaveType)
        {
            case (int)eSAVE_TYPE.Define: return TableManager.Instance.Time.GetType(timeID);

            default: return TimeManager.eTYPE.Real;
        }
    }

	public bool IsActive(eSAVE_TYPE eSaveType, uint timeID)
    {
		switch(this.getType(eSaveType, timeID))
        {
            case TimeManager.eTYPE.Real:
            {
				if(base.Data.DicRealTime.ContainsKey((int)eSaveType) == false) return false;
				if(base.Data.DicRealTime[(int)eSaveType].ContainsKey(timeID) == false) return false;

				return base.Data.DicRealTime[(int)eSaveType][timeID].IsActive;
            }

            case TimeManager.eTYPE.Game:
            {
				if(base.Data.DicGameTime.ContainsKey((int)eSaveType) == false) return false;
				if(base.Data.DicGameTime[(int)eSaveType].ContainsKey(timeID) == false) return false;

				return base.Data.DicGameTime[(int)eSaveType][timeID].IsActive;
            }
        }
		return false;
    }

	public bool IsActive(TableData.TableTime.eID eTimeID)
	{
		return this.IsActive((int)eSAVE_TYPE.Define, (uint)eTimeID);
	}

	public double GetRemainTime(eSAVE_TYPE eSaveType, uint timeID)
    {
		switch(this.getType(eSaveType, timeID))
        {
            case TimeManager.eTYPE.Real:
            {
				if(base.Data.DicRealTime.ContainsKey((int)eSaveType) == false) return 0.0f;
				if(base.Data.DicRealTime[(int)eSaveType].ContainsKey(timeID) == false) return 0.0f;

				return base.Data.DicRealTime[(int)eSaveType][timeID].RemainTime;
            }

            case TimeManager.eTYPE.Game:
            {
				if(base.Data.DicGameTime.ContainsKey((int)eSaveType) == false) return 0.0f;
				if(base.Data.DicGameTime[(int)eSaveType].ContainsKey(timeID) == false) return 0.0f;

				return base.Data.DicGameTime[(int)eSaveType][timeID].RemainTime;
            }
        }
		return 0.0f;
    }

	public double GetRemainTime(TableData.TableTime.eID eTimeID)
    {
		return this.GetRemainTime((int)eSAVE_TYPE.Define, (uint)eTimeID);
    }

	public void RefreshCurrTime()
    {
		if(base.Data == null) return;

		Dictionary<int, Dictionary<uint, RealTimeData>>.Enumerator enumRealDic = base.Data.DicRealTime.GetEnumerator();
		Dictionary<uint, RealTimeData>.Enumerator enumRealTime;
		while(enumRealDic.MoveNext())
        {
			enumRealTime = enumRealDic.Current.Value.GetEnumerator();
			while(enumRealTime.MoveNext())
			{
				enumRealTime.Current.Value.RefreshRemainTime();
			}
        }

		//시간 업데이트 됐다고 저장
		this.saveLastUpdateTime();
    }

	public void AddFinishEvent(eSAVE_TYPE eSaveType, uint timeID, UnityAction funcFinished)
    {
		switch(this.getType(eSaveType, timeID))
        {
            case TimeManager.eTYPE.Real:
            {
				if(base.Data.DicRealTime.ContainsKey((int)eSaveType) == false) return;
				if(base.Data.DicRealTime[(int)eSaveType].ContainsKey(timeID) == false) return;

				base.Data.DicRealTime[(int)eSaveType][timeID].AddFinishEvent(funcFinished);
            }
            break;
            case TimeManager.eTYPE.Game:
            {
				if(base.Data.DicGameTime.ContainsKey((int)eSaveType) == false) return;
				if(base.Data.DicGameTime[(int)eSaveType].ContainsKey(timeID) == false) return;

				base.Data.DicGameTime[(int)eSaveType][timeID].AddFinishEvent(funcFinished);
            }
            break;
        }
    }

	public void AddFinishEvent(TableData.TableTime.eID eTimeID, UnityAction funcFinished)
    {
		this.AddFinishEvent((int)eSAVE_TYPE.Define, (uint)eTimeID, funcFinished);
    }

	public void AddPlayTime()
	{
		base.Data.PlayTime++;

		this.SaveClientData();
	}
}

public class JsonData_Time : BaseJsonData
{
	//Key : UserData_Time.eSAVE_TYPE, Value : UserData_Time_Real
	public Dictionary<int, Dictionary<uint, UserData_Time.RealTimeData>> DicRealTime = new Dictionary<int, Dictionary<uint, UserData_Time.RealTimeData>>();

	//Key : UserData_Time.eSAVE_TYPE, Value : UserData_Time_Game
	public Dictionary<int, Dictionary<uint, UserData_Time.GameTimeData>> DicGameTime = new Dictionary<int, Dictionary<uint, UserData_Time.GameTimeData>>();

	public DateTime LastUpdateTime = DateTime.Now.AddYears(-10);
	public DateTime OfflineRewardTime = DateTime.Now;
	public BigInteger PlayTime = BigInteger.Zero;
}