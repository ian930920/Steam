using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserData_User : UserData<JsonData_User>
{
	public class SummonData
	{
		public uint SummonID { get; private set; } = 0;
		public List<uint> ListRuneID { get; private set; } = new List<uint>();

		public SummonData(uint summonID)
		{
			this.SummonID = summonID;
		}

		public void AddRune(uint runeID)
		{
			//중복룬 가능?
			this.ListRuneID.Add(runeID);
		}

		public void RemoveRune(uint runeID)
		{
			if(this.ListRuneID.Contains(runeID) == false) return;

			this.ListRuneID.Remove(runeID);
		}
	}

	protected override string StrKey => "User";

	protected override void dataProcessing()
	{
	}

#region Summon
    public void AddSummon(uint summonID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == true) return;

		base.Data.DicSummon.Add(summonID, new SummonData(summonID));
		base.SaveClientData();
	}

	public List<SummonData> GetSummonDataByList()
	{
		return base.Data.DicSummon.Values.ToList();
	}

#region Rune
    public void AddRune(uint summonID, uint runeID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return;

		base.Data.DicSummon[summonID].AddRune(runeID);
		base.SaveClientData();
	}

	public void RemoveRune(uint summonID, uint runeID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return;

		base.Data.DicSummon[summonID].RemoveRune(runeID);
		base.SaveClientData();
	}
#endregion
#endregion
}

public class JsonData_User : BaseJsonData
{
	//TODO 룬
	//TODO 유물

	//Key : InvenType, Value : <Key : ItemID, Value : Value>
	public Dictionary<uint, UserData_User.SummonData> DicSummon = new Dictionary<uint, UserData_User.SummonData>();
}