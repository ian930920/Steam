using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class UserData_Summon : UserData<JsonData_Summon>
{
	public class SummonData
	{
		public uint SummonID { get; private set; } = 0;
		public uint SkillID { get; private set; } = 0;

		public List<uint> ListRuneID { get; private set; } = new List<uint>();

		public Stat_Character StatDefault { get; private set; } = new Stat_Character();
		public Stat_Additional StatAdditional { get; private set; } = new Stat_Additional();
		public ulong Damage => ProjectManager.Instance.Table.Skill.GetDefaultDamage(this.SkillID, this.StatDefault, this.StatAdditional);

		public SummonData() { }

		[JsonConstructor]
		public SummonData(uint summonID)
		{
			this.SummonID = summonID;
			var data = ProjectManager.Instance.Table.Summon.GetData(this.SummonID);

			this.SkillID = data.skillID;

			this.StatDefault.Reset();
			this.StatDefault.SetStat(Stat_Character.eTYPE.Strength, 1);
			this.StatDefault.SetStat(Stat_Character.eTYPE.Mana, data.cost);
		
			//스탯에 룬 적용
			for(int i = 0, nMax = this.ListRuneID.Count; i < nMax; ++i)
			{
				this.addRuneStat(this.ListRuneID[i]);
			}
		}

		public SummonData(SummonData org)
		{
			this.SummonID = org.SummonID;
			this.SkillID = org.SkillID;

			this.StatDefault = new Stat_Character(org.StatDefault);
			this.StatAdditional = new Stat_Additional(org.StatAdditional);

			this.ListRuneID = new List<uint>(org.ListRuneID);
		}

		public void AddRune(uint runeID)
		{
			//중복룬 가능?
			this.ListRuneID.Add(runeID);

			this.addRuneStat(runeID);
		}

		public void RemoveRune(uint runeID)
		{
			if(this.ListRuneID.Contains(runeID) == false) return;

			this.ListRuneID.Remove(runeID);

			var data = ProjectManager.Instance.Table.Rune.GetData(runeID);
			switch((TableData.TableRune.eID)runeID)
			{
				case TableData.TableRune.eID.Anger:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Coe, data.value);
				}
				break;

				case TableData.TableRune.eID.Focus:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Acc, data.value);
				}
				break;

				case TableData.TableRune.eID.Bold:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Crit, data.value);
				}
				break;

				case TableData.TableRune.eID.Vitality:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Cooldown, data.value);
				}
				break;

				case TableData.TableRune.eID.Calm:
				{
					this.StatDefault.AddStat(Stat_Character.eTYPE.Mana, (ulong)data.value);
				}
				break;

				case TableData.TableRune.eID.Bonding:
				{
					//정령 소환 시 { value } 턴 동안 내 캐릭터에게 방어력 증가를 부여합니다.
					var statusID = ProjectManager.Instance.Table.Rune.GetData(runeID).statusID;
					this.StatAdditional.RemoveStatus(statusID);
				}
				break;

				case TableData.TableRune.eID.Disgust:
				{
					//정령 소환 시 { value } 턴 동안 모든 적에게 방어력 약화를 부여합니다.
					var statusID = ProjectManager.Instance.Table.Rune.GetData(runeID).statusID;
					this.StatAdditional.RemoveStatus(statusID);
				}
				break;

				case TableData.TableRune.eID.Comfort:
				{
					//행동형~
				}
				break;
			}
		}

		private void addRuneStat(uint runeID)
		{
			var data = ProjectManager.Instance.Table.Rune.GetData(runeID);
			switch((TableData.TableRune.eID)runeID)
			{
				case TableData.TableRune.eID.Anger:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Coe, data.value);
				}
				break;

				case TableData.TableRune.eID.Focus:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Acc, data.value);
				}
				break;

				case TableData.TableRune.eID.Bold:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Crit, data.value);
				}
				break;

				case TableData.TableRune.eID.Vitality:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Cooldown, data.value);
				}
				break;

				case TableData.TableRune.eID.Calm:
				{
					this.StatDefault.RemoveStat(Stat_Character.eTYPE.Mana, (ulong)data.value);
				}
				break;

				case TableData.TableRune.eID.Bonding:
				{
					//정령 소환 시 { value } 턴 동안 내 캐릭터에게 방어력 증가를 부여합니다.
					var statusID = ProjectManager.Instance.Table.Rune.GetData(runeID).statusID;
					this.StatAdditional.AddStatus(statusID, new stStatus(stStatus.eTARGET_TYPE.User, (ulong)data.value));
				}
				break;

				case TableData.TableRune.eID.Disgust:
				{
					//정령 소환 시 { value } 턴 동안 모든 적에게 방어력 약화를 부여합니다.
					var statusID = ProjectManager.Instance.Table.Rune.GetData(runeID).statusID;
					this.StatAdditional.AddStatus(statusID, new stStatus(stStatus.eTARGET_TYPE.EnemyAll, (ulong)data.value));
				}
				break;

				case TableData.TableRune.eID.Comfort:
				{
					//행동형~
				}
				break;
			}
		}
	}

	protected override string StrKey => "User";

	public int SummonCount => base.Data.DicSummon.Count;

	protected override void dataProcessing()
	{
	}

#region Summon
	public bool IsContainsSummon(uint summonID)
	{
		return base.Data.DicSummon.ContainsKey(summonID);
	}

	public void AddSummon(uint summonID)
	{
		if(this.IsContainsSummon(summonID) == true) return;

		base.Data.DicSummon.Add(summonID, new SummonData(summonID));
		base.SaveClientData();
	}

	public SummonData GetSummon(uint summonID)
	{
		if(this.IsContainsSummon(summonID) == false) return null;

		return base.Data.DicSummon[summonID];
	}

	public List<SummonData> GetSummonDataList()
	{
		return base.Data.DicSummon.Values.ToList();
	}

#region Rune
	public List<uint> GetRuneList(uint summonID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return new List<uint>();

		return base.Data.DicSummon[summonID].ListRuneID;
	}

	public bool IsEquipRune(uint summonID, uint runeID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return false;
		
		return base.Data.DicSummon[summonID].ListRuneID.Contains(runeID);
	}

    public void AddRune(uint summonID, uint runeID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return;

		base.Data.DicSummon[summonID].AddRune(runeID);
		//TODO 살리기 base.SaveClientData();
	}

	public void RemoveRune(uint summonID, uint runeID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return;

		base.Data.DicSummon[summonID].RemoveRune(runeID);
		//TODO 살리기 base.SaveClientData();
	}
    #endregion
    #endregion

#region Debug
	public void Debug_AddSummon()
	{
		var enumData = ProjectManager.Instance.Table.Summon.GetEnumerator();
		while(enumData.MoveNext())
        {
			this.AddSummon(enumData.Current.Key);
        }
	}

	public void Debug_RemoveSummon()
	{
		base.Data.DicSummon.Clear();
		base.SaveClientData();
	}
	#endregion
}

public class JsonData_Summon : BaseJsonData
{
	//TODO 룬
	//TODO 유물

	//Key : InvenType, Value : <Key : ItemID, Value : Value>
	public Dictionary<uint, UserData_Summon.SummonData> DicSummon = new Dictionary<uint, UserData_Summon.SummonData>();
}