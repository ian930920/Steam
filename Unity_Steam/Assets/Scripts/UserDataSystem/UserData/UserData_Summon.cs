using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class UserData_Summon : UserData<JsonData_Summon>
{
	public class MySummon
	{
		public uint SummonID { get; private set; } = 0;
		private TableData.TableData_Summon m_data = null;
		public uint SkillID => this.m_data.skillID;

		public List<Item_Rune> ListRune { get; private set; } = new List<Item_Rune>();
		public bool IsRuneMax => this.ListRune.Count == TableManager.Instance.Summon.GetData(this.SummonID).maxRune;

		public Stat_Character StatDefault { get; private set; } = new Stat_Character();
		public Stat_Additional StatAdditional { get; private set; } = new Stat_Additional();
		public int Damage => TableManager.Instance.Skill.GetDefaultDamage(this.SkillID, this.StatDefault, this.StatAdditional);
		public int Cooldown => TableManager.Instance.Skill.GetCooldownTurn(this.SkillID, this.StatAdditional);

		public MySummon() { }

		[JsonConstructor]
		public MySummon(uint summonID)
		{
			this.SummonID = summonID;
			this.m_data = TableManager.Instance.Summon.GetData(this.SummonID);

			this.StatDefault.Reset();
			this.StatDefault.SetStat(Stat_Character.eTYPE.Strength, 1);
			this.StatDefault.SetStat(Stat_Character.eTYPE.Mana, this.m_data.cost);

			this.StatAdditional.Reset();
		}

		public void InitRuneStat()
		{
			//추가 스탯에 룬 적용
			for(int i = 0, nMax = this.ListRune.Count; i < nMax; ++i)
			{
				this.addRuneStat(this.ListRune[i].RuneID);
			}
		}

		public MySummon(MySummon org)
		{
			this.SummonID = org.SummonID;
			this.m_data = TableManager.Instance.Summon.GetData(this.SummonID);

			this.StatDefault = new Stat_Character(org.StatDefault);
			this.StatAdditional = new Stat_Additional(org.StatAdditional);

			this.ListRune = new List<Item_Rune>(org.ListRune);
		}

		public void AddRune(Item_Rune rune)
		{
			//중복룬 불가능
			if(this.ListRune.Any(data => data.UniqueRuneID == rune.UniqueRuneID) == true) return;

			this.ListRune.Add(rune);
			this.addRuneStat(rune.RuneID);
		}

		public void RemoveRune(Item_Rune rune)
		{
			if(this.ListRune.Any(data => data.UniqueRuneID == rune.UniqueRuneID) == false) return;

			var idx = this.ListRune.FindIndex(data => data.UniqueRuneID == rune.UniqueRuneID);
			this.ListRune.RemoveAt(idx);
			this.removeRuneStat(rune.RuneID);
		}

		private void addRuneStat(uint runeID)
		{
			var data = TableManager.Instance.Rune.GetData(runeID);
			switch((TableData.TableRune.eID)runeID)
			{
				case TableData.TableRune.eID.Anger:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Coe, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Coe, TableData.TableStatus.eEFFECT_TYPE.Positive);
				}
				break;

				case TableData.TableRune.eID.Focus:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Acc, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Acc, TableData.TableStatus.eEFFECT_TYPE.Positive);
				}
				break;

				case TableData.TableRune.eID.Bold:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Crit, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Crit, TableData.TableStatus.eEFFECT_TYPE.Positive);
				}
				break;

				case TableData.TableRune.eID.Vitality:
				{
					this.StatAdditional.AddStat(Stat_Additional.eTYPE.Cooldown, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Cooldown, TableData.TableStatus.eEFFECT_TYPE.Positive);
				}
				break;

				case TableData.TableRune.eID.Calm:
				{
					this.StatDefault.RemoveStat(Stat_Character.eTYPE.Mana, (int)data.value);
					this.StatDefault.SetEffectType(Stat_Character.eTYPE.Mana, TableData.TableStatus.eEFFECT_TYPE.Positive);
				}
				break;

				case TableData.TableRune.eID.Bonding:
				{
					//정령 소환 시 { value } 턴 동안 내 캐릭터에게 방어력 증가를 부여합니다.
					this.StatAdditional.AddStatus(data.statusID, new stStatus(stStatus.eTARGET_TYPE.User, (int)data.value));
				}
				break;

				case TableData.TableRune.eID.Disgust:
				{
					//정령 소환 시 { value } 턴 동안 모든 적에게 방어력 약화를 부여합니다.
					this.StatAdditional.AddStatus(data.statusID, new stStatus(stStatus.eTARGET_TYPE.EnemyAll, (int)data.value));
				}
				break;

				case TableData.TableRune.eID.Comfort:
				{
					//행동형~
				}
				break;
			}
		}

		private void removeRuneStat(uint runeID)
		{
			var data = TableManager.Instance.Rune.GetData(runeID);
			switch((TableData.TableRune.eID)runeID)
			{
				case TableData.TableRune.eID.Anger:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Coe, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Coe, TableData.TableStatus.eEFFECT_TYPE.None);
				}
				break;

				case TableData.TableRune.eID.Focus:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Acc, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Acc, TableData.TableStatus.eEFFECT_TYPE.None);
				}
				break;

				case TableData.TableRune.eID.Bold:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Crit, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Crit, TableData.TableStatus.eEFFECT_TYPE.None);
				}
				break;

				case TableData.TableRune.eID.Vitality:
				{
					this.StatAdditional.RemoveStat(Stat_Additional.eTYPE.Cooldown, data.value);
					this.StatAdditional.SetEffectType(Stat_Additional.eTYPE.Cooldown, TableData.TableStatus.eEFFECT_TYPE.None);
				}
				break;

				case TableData.TableRune.eID.Calm:
				{
					this.StatDefault.AddStat(Stat_Character.eTYPE.Mana, (int)data.value);
					this.StatDefault.SetEffectType(Stat_Character.eTYPE.Mana, TableData.TableStatus.eEFFECT_TYPE.None);
				}
				break;

				case TableData.TableRune.eID.Bonding:
				case TableData.TableRune.eID.Disgust:
				{
                    //정령 소환 시 { value } 턴 동안 내 캐릭터에게 방어력 증가를 부여합니다.
					this.StatAdditional.RemoveStatus(data.statusID);
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
		var enumSummon = base.Data.DicSummon.GetEnumerator();
		while(enumSummon.MoveNext())
        {
			enumSummon.Current.Value.InitRuneStat();
        }
	}

#region Summon
	public bool IsContainsSummon(uint summonID)
	{
		return base.Data.DicSummon.ContainsKey(summonID);
	}

	public void AddSummon(uint summonID)
	{
		if(this.IsContainsSummon(summonID) == true) return;

		base.Data.DicSummon.Add(summonID, new MySummon(summonID));
		base.SaveClientData();
	}

	public MySummon GetSummon(uint summonID)
	{
		if(this.IsContainsSummon(summonID) == false) return null;

		return base.Data.DicSummon[summonID];
	}

	public List<MySummon> GetSummonDataList()
	{
		return base.Data.DicSummon.Values.ToList();
	}

#region Rune
	public List<Item_Rune> GetRuneList(uint summonID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return new List<Item_Rune>();

		return base.Data.DicSummon[summonID].ListRune;
	}

	public bool IsEquipRune(uint summonID, uint runeID)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return false;
		
		return base.Data.DicSummon[summonID].ListRune.Any(data => data.RuneID == runeID);
	}

    public void AddRune(uint summonID, Item_Rune rune)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return;

		base.Data.DicSummon[summonID].AddRune(rune);
		base.SaveClientData();
	}

	public void RemoveRune(uint summonID, Item_Rune rune)
	{
		if(base.Data.DicSummon.ContainsKey(summonID) == false) return;

		base.Data.DicSummon[summonID].RemoveRune(rune);
		base.SaveClientData();
	}
    #endregion
    #endregion

#region Debug
	public void Debug_AddSummon()
	{
		var enumData = TableManager.Instance.Summon.GetEnumerator();
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

	public void Debug_RemoveSummonRune()
	{
		var enumData = base.Data.DicSummon.GetEnumerator();
        while(enumData.MoveNext())
        {
			enumData.Current.Value.ListRune.Clear();
        }
        base.SaveClientData();
	}
	#endregion
}

public class JsonData_Summon : BaseJsonData
{
	//Key : InvenType, Value : <Key : ItemID, Value : Value>
	public Dictionary<uint, UserData_Summon.MySummon> DicSummon = new Dictionary<uint, UserData_Summon.MySummon>();
}