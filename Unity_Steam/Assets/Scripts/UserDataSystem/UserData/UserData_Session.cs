using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserData_Session : UserData<JsonData_Session>
{
	protected override string StrKey => "Session";
	public eSESSION_TYPE CurrSessionType => base.Data.CurrSessionType;
	public bool IsSessionStart => base.Data.IsSessionStart;
	public bool IsScenarioWatch => base.Data.IsScenarioWatch;
	public int Stage => base.Data.Stage;
	public uint RouteID => base.Data.RouteID;
	public int Step => base.Data.Step;

	public Stat_Character DefaultStat => base.Data.CurrStat;
	public Dictionary<uint, int>.Enumerator EnumDicStatus => base.Data.DicStatus.GetEnumerator();
	public List<uint> ListRoute => base.Data.ListRoute;

	public eSHOP_TYPE ShopType => base.Data.ShopType;
	public int ShopCount => base.Data.ListShop.Count;
	public bool IsEnding => this.Step >= TableData.TableRouteStep.MAX_STEP && this.Stage >= TableData.TableRouteStep.MAX_STAGE;


	protected override void dataProcessing()
	{

	}

	public void StartSession()
    {
		//기본 초기화
		base.Data.CurrSessionType = eSESSION_TYPE.Station;
		base.Data.IsSessionStart = true;

		//기본 스탯
		var dataChar = TableManager.Instance.User.GetData((int)TableData.TableUser.eID.User);
        base.Data.CurrStat.Reset();
        base.Data.CurrStat.SetStat(Stat_Character.eTYPE.HP, dataChar.hp);
        base.Data.CurrStat.SetStat(Stat_Character.eTYPE.Mana, dataChar.maxMana);

		//상태 이상 초기화
		base.Data.DicStatus.Clear();

		//스테이지 초기화
		base.Data.ListStep.Clear();

		//이벤트 초기화
		base.Data.ListEvent.Clear();

		base.Data.Stage = 0;
		base.Data.RouteID = 0;
		base.Data.Step = 0;

		//상점 초기화
		base.Data.ShopType = eSHOP_TYPE.Rune;
		base.Data.ListShop.Clear();

		this.SaveClientData();
	}

	public void FinishSession()
    {
		//게임 끝 ㅠ
		base.Data.IsScenarioWatch = false;
		base.Data.IsSessionStart = false;
		this.SaveClientData();
	}

	public void WatchScenario()
	{
		base.Data.IsScenarioWatch = true;
		this.SaveClientData();
	}

	public void SetSessionType(eSESSION_TYPE eSessionType)
	{
		base.Data.CurrSessionType = eSessionType;
        switch(base.Data.CurrSessionType)
        {
            case eSESSION_TYPE.Station:
			{
				this.saveShop();
				this.saveRoute();
			}
            break;
        }
        this.SaveClientData();
    }

	private void saveShop()
	{
		//랜덤 저장
		base.Data.ShopType = (eSHOP_TYPE)UnityEngine.Random.Range(0, (int)eSHOP_TYPE.End);

		base.Data.ListShop.Clear();
		switch(base.Data.ShopType)
		{
			case eSHOP_TYPE.Rune:
			{
				var list = TableManager.Instance.Rune.GetRandomList(Popup_StationShop.COUNT_MAX_GOODS);
				for(int i = 0, nMax = list.Count; i < nMax; ++i)
				{
					base.Data.ListShop.Add(new stShop(list[i].tableID));
				}
			}
            break;
            case eSHOP_TYPE.Summon:
			{
				int nCount = TableManager.Instance.Summon.GetRandomListCount(Popup_StationShop.COUNT_MAX_GOODS);
				while(base.Data.ListShop.Count < nCount)
				{
					var summonID = TableManager.Instance.Summon.GetRandomData().tableID;
					if(UserDataManager.Instance.Summon.IsContainsSummon(summonID) == true) continue;
					if(base.Data.ListShop.Any(data => data.ItemID == summonID) == true) continue;

					base.Data.ListShop.Add(new stShop(summonID));
				}
			}
            break;
        }
		this.SaveClientData();
    }

	private void saveRoute()
	{
		base.Data.ListRoute.Clear();
		var list = TableManager.Instance.Route.GetRandomList(3);
		for(int i = 0, nMax = list.Count; i < nMax; ++i)
		{
			base.Data.ListRoute.Add(list[i].tableID);
		}
		this.SaveClientData();
    }

	public stShop GetShop(int nIdx)
	{
		if(base.Data.ListShop.Count == 0) this.saveShop();

		return base.Data.ListShop[nIdx];
	}

	public void SaveShopSoldOut(uint itemID)
	{
		if(base.Data.ListShop.Any(data => data.ItemID == itemID) == false) return;

		int nIdx = base.Data.ListShop.FindIndex(data => data.ItemID == itemID);
		base.Data.ListShop[nIdx] = new stShop(itemID, true);

		this.SaveClientData();
	}

    public void SaveBattleInfo(int nHP, List<Status> listStatus)
    {
		base.Data.CurrStat.SetStat(Stat_Character.eTYPE.HP, nHP);

		//이전 상태이상 모두 삭제
		base.Data.DicStatus.Clear();

		//지금까지 상태이상 저장
		for(int i = 0, nMax = listStatus.Count; i < nMax; ++i)
		{
			base.Data.DicStatus.Add((uint)listStatus[i].eStatusID, listStatus[i].RemainTurn);
		}
		this.SaveClientData();
    }

	public void Heal(int nHP)
	{
		var currHP = base.Data.CurrStat.GetStat(Stat_Character.eTYPE.HP);
		var resultHP = Mathf.Clamp(currHP + nHP, nHP, TableManager.Instance.User.GetData((int)TableData.TableUser.eID.User).hp);
		base.Data.CurrStat.SetStat(Stat_Character.eTYPE.HP, resultHP);
		this.SaveClientData();
	}

	public void SaveRoute(uint routeID)
	{
		//스테이지 증가
		base.Data.Stage++;

		//루트 저장하고
		base.Data.RouteID = routeID;
		
		//지금 진행중인 스텝 초기화
		base.Data.Step = 0;

		//앞으로 진행할 스텝 미리 저장
		this.saveStep();
	}

	private void saveStep()
	{
		base.Data.ListStep.Clear();

		//전투 최소 2개
		base.Data.ListStep.Add((int)TableData.TableRouteStep.eSTEP_TYPE.Battle);
		base.Data.ListStep.Add((int)TableData.TableRouteStep.eSTEP_TYPE.Battle);
		
		//나머지 랜덤
		int nLevel = TableManager.Instance.Route.GetData(base.Data.RouteID).level;
		while(base.Data.ListStep.Count < TableData.TableRouteStep.MAX_STEP - 1)
		{
			base.Data.ListStep.Add((int)TableManager.Instance.RouteStep.GetRandomTableID(nLevel));
		}

		//섞고
		base.Data.ListStep = base.Data.ListStep.OrderBy(g => Guid.NewGuid()).ToList();
		
		//마지막은 보스전
		base.Data.ListStep.Add((int)TableData.TableRouteStep.eSTEP_TYPE.Boss);
		
		this.SaveClientData();
	}

	public TableData.TableRouteStep.eSTEP_TYPE GetCurrStep()
	{
		if(base.Data.ListStep.Count == 0) this.saveStep();

		return (TableData.TableRouteStep.eSTEP_TYPE)base.Data.ListStep[base.Data.Step];
	}

	public void NextStep()
	{
		base.Data.Step++;
		this.SaveClientData();
	}

	public void AddEvent(uint eventID)
	{
		if(base.Data.ListEvent.Contains(eventID) == true) return;

		base.Data.ListEvent.Add(eventID);
		this.SaveClientData();
	}

	public bool IsContainEvent(uint eventID)
	{
		return base.Data.ListEvent.Contains(eventID);
	}
}

public class JsonData_Session : BaseJsonData
{
	public eSESSION_TYPE CurrSessionType = eSESSION_TYPE.Station;
	public bool IsSessionStart = false;
	public bool IsScenarioWatch = false;

	public int Stage = 0;
	public uint RouteID = 0;
	public int Step = 0;

	//유저 기본 스탯
    public Stat_Character CurrStat = new Stat_Character();

	//Key : statusID, turn
    public Dictionary<uint, int> DicStatus = new Dictionary<uint, int>();

	public List<int> ListStep = new List<int>();
	public List<uint> ListEvent = new List<uint>();

	public eSHOP_TYPE ShopType = eSHOP_TYPE.Rune;
	public List<stShop> ListShop = new List<stShop>();

	public List<uint> ListRoute = new List<uint>();
}

public enum eSESSION_TYPE
{
	Station,
	Battle,
	Event,
}

public enum eSHOP_TYPE
{
	Rune,
	Summon,
	End,
}

public struct stShop
{
	public uint ItemID;
	public bool IsSoldOut;

	public stShop(uint itemID, bool isSoldOut = false)
	{
		this.ItemID = itemID;
		this.IsSoldOut = isSoldOut;
	}
}