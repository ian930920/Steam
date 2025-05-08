using System.Collections.Generic;
using TableData;

public class UserData_Session : UserData<JsonData_Session>
{
	protected override string StrKey => "Session";
	public eSESSION_TYPE CurrSessionType => base.Data.CurrSessionType;
	public bool IsSessionStart => base.Data.IsSessionStart;
	public bool IsScenarioWatch => base.Data.IsScenarioWatch;

	public Stat_Character DefaultStat => base.Data.DefaultStat;

	protected override void dataProcessing()
	{

	}

	public void StartSession()
    {
		var dataChar = ProjectManager.Instance.Table.User.GetData((int)TableUser.eID.User);
        base.Data.DefaultStat.Reset();
        base.Data.DefaultStat.SetStat(Stat_Character.eTYPE.HP, dataChar.hp);
        base.Data.DefaultStat.SetStat(Stat_Character.eTYPE.Mana, dataChar.maxMana);

		base.Data.ListStage.Clear();

		base.Data.IsSessionStart = true;

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
}

public class JsonData_Session : BaseJsonData
{
	public eSESSION_TYPE CurrSessionType = eSESSION_TYPE.Station;
	public bool IsSessionStart = false;
	public bool IsScenarioWatch = false;

	//유저 기본 스탯
    public Stat_Character DefaultStat = new Stat_Character();

	public List<uint> ListStage = new List<uint>();
}

public enum eSESSION_TYPE
{
	Station,
	Battle,
	Event,
}