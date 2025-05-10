using System.Collections.Generic;
using TableData;

public class UserData_Session : UserData<JsonData_Session>
{
	protected override string StrKey => "Session";
	public eSESSION_TYPE CurrSessionType => base.Data.CurrSessionType;
	public bool IsSessionStart => base.Data.IsSessionStart;
	public bool IsScenarioWatch => base.Data.IsScenarioWatch;

	public Stat_Character DefaultStat => base.Data.CurrStat;
	public Dictionary<uint, int>.Enumerator EnumDicStatus => base.Data.DicStatus.GetEnumerator();

	protected override void dataProcessing()
	{

	}

	public void StartSession()
    {
		//기본 초기화
		base.Data.CurrSessionType = eSESSION_TYPE.Station;
		base.Data.IsScenarioWatch = false;
		base.Data.IsSessionStart = true;

		//기본 스탯
		var dataChar = TableManager.Instance.User.GetData((int)TableUser.eID.User);
        base.Data.CurrStat.Reset();
        base.Data.CurrStat.SetStat(Stat_Character.eTYPE.HP, dataChar.hp);
        base.Data.CurrStat.SetStat(Stat_Character.eTYPE.Mana, dataChar.maxMana);

		//상태 이상 초기화
		base.Data.DicStatus.Clear();

		//스테이지 초기화
		base.Data.ListStage.Clear();

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
}

public class JsonData_Session : BaseJsonData
{
	public eSESSION_TYPE CurrSessionType = eSESSION_TYPE.Station;
	public bool IsSessionStart = false;
	public bool IsScenarioWatch = false;

	//유저 기본 스탯
    public Stat_Character CurrStat = new Stat_Character();

	//Key : statusID, turn
    public Dictionary<uint, int> DicStatus = new Dictionary<uint, int>();

	public List<uint> ListStage = new List<uint>();
}

public enum eSESSION_TYPE
{
	Station,
	Battle,
	Event,
}