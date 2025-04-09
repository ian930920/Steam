using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserData_Summon : UserData<JsonData_Summon>
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

	#region PP_KEY
	static private string PP_KEY = "Summon";
	#endregion

	protected override string StrKey => PP_KEY;

	protected override void dataProcessing()
	{
	}
		
	public void AddSummon(uint summonID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == true) return;

		base.Data.DicSummon.Add(summonID, new SummonData(summonID));
		base.SaveClientData();
	}

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

	public List<SummonData> GetSummonDataByList()
	{
		return base.Data.DicSummon.Values.ToList();
	}
}

public class JsonData_Summon : BaseJsonData
{
	//Key : InvenType, Value : <Key : ItemID, Value : Value>
	public Dictionary<uint, UserData_Summon.SummonData> DicSummon = new Dictionary<uint, UserData_Summon.SummonData>();
}