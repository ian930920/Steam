using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class UserData_Inventory : UserData<JsonData_Inventory>
{
	#region PP_KEY
	static private string PP_KEY = "Inventory";
	#endregion

    protected override string StrKey => PP_KEY;

	protected override void dataProcessing()
	{
		if(base.Data.DicInventory.Count == 0)
		{
			Dictionary<uint, uint>.Enumerator enumInven;
			Dictionary<int, Dictionary<uint, uint>>.Enumerator enumDic = base.Data.DicInventory.GetEnumerator();
			while(enumDic.MoveNext())
			{
				int nInvenType = (int)enumDic.Current.Key;
				if(base.Data.DicInventory.ContainsKey(nInvenType) == false) base.Data.DicInventory.Add(nInvenType, new Dictionary<uint, uint>());

				enumInven = enumDic.Current.Value.GetEnumerator();
				while(enumInven.MoveNext())
				{
					base.Data.DicInventory[nInvenType].Add(enumInven.Current.Key, enumInven.Current.Value);
				}
			}
		}
	}

	public bool IsContainsItem(uint itemID)
    {
		//인벤에 있는지 확인
		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(itemID);
		if(base.Data.DicInventory.ContainsKey(nInvenType) == false) return false;

		return base.Data.DicInventory[nInvenType].ContainsKey(itemID);
    }

	public bool IsContainsItem(TableData.TableItem.eID eItemID)
    {
		return this.IsContainsItem((uint)eItemID);
    }

	public stItem GetItem(uint itemID)
    {
		return new stItem(itemID, this.getItemCount(itemID));
    }

	public stItem GetItem(TableData.TableItem.eID eItemID)
    {
		return this.GetItem((uint)eItemID);
    }

	private uint getItemCount(uint itemID)
    {
		if(this.IsContainsItem(itemID) == false) return 0;

		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(itemID);
		return base.Data.DicInventory[nInvenType][itemID];
    }

	public int GetCountByCategory(TableData.TableItem.eINVEN_TYPE eType)
	{
		if(base.Data.DicInventory.ContainsKey((int)eType) == false) return 0;

		return base.Data.DicInventory[(int)eType].Count;
	}

	public bool IsUseable(uint itemID, uint nUseCount)
	{
		return nUseCount <= this.getItemCount(itemID);
	}

    public void AddItem(uint itemID, uint nCount)
    {
		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(itemID);
		if(this.IsContainsItem(itemID) == false)
		{
			if(base.Data.DicInventory.ContainsKey(nInvenType) == false) base.Data.DicInventory.Add(nInvenType, new Dictionary<uint, uint>());
			base.Data.DicInventory[nInvenType].Add(itemID, nCount);
		}
		else base.Data.DicInventory[nInvenType][itemID] += nCount;

		//저장
		base.SaveClientData();
	}

    public void UseItem(uint itemID, uint nCount)
    {
		//0이면 할 필요 없음
		if(nCount == 0) return;

		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(itemID);

		uint nResult = base.Data.DicInventory[nInvenType][itemID] - nCount;
		base.Data.DicInventory[nInvenType][itemID] = nResult;

		//저장
		base.SaveClientData();
	}

	public int GetItemCountByType(int eInvenType)
    {
		if(base.Data.DicInventory.ContainsKey(eInvenType) == false) return 0;

		return base.Data.DicInventory[eInvenType].Count;
    }

#region Rune
	public Item_Rune GetRune(uint uniqueRuneID)
	{
		if(base.Data.DicRune.ContainsKey(uniqueRuneID) == false) return null;

		return base.Data.DicRune[uniqueRuneID];
	}

	public void AddRune(uint runeID)
	{
		var uniqueRuneID = base.Data.UniqueRuneID;

		//룬에 추가
		base.Data.DicRune.Add(uniqueRuneID, new Item_Rune(uniqueRuneID, runeID));

		//룬 ID 증가
		base.Data.UniqueRuneID++;
		this.SaveClientData();
	}

	public void EquipRune(uint uniqueRuneID, uint summonID)
	{
		if(base.Data.DicRune.ContainsKey(uniqueRuneID) == false) return;

		base.Data.DicRune[uniqueRuneID].SummonID = summonID;
		this.SaveClientData();
	}

	public void UnequipRune(uint uniqueRuneID)
	{
		if(base.Data.DicRune.ContainsKey(uniqueRuneID) == false) return;

		base.Data.DicRune[uniqueRuneID].SummonID = 0;
		this.SaveClientData();
	}

	public List<Item_Rune> GetRuneList()
	{
		return base.Data.DicRune.Values.ToList();
	}
#endregion

#region Debug
	public void Debug_AddRune()
	{
		var enumData = ProjectManager.Instance.Table.Rune.GetEnumerator();
		while(enumData.MoveNext())
		{
			this.AddRune(enumData.Current.Key);
		}
	}

	public void Debug_RemoveRune()
	{
		base.Data.DicRune.Clear();
		this.SaveClientData();
	}

	public void Debug_RemoveRuneSummonID()
	{
		var enumData = base.Data.DicRune.GetEnumerator();
		while(enumData.MoveNext())
		{
			enumData.Current.Value.SummonID = 0;
		}
		this.SaveClientData();
	}
#endregion
}

public class JsonData_Inventory : BaseJsonData
{
	//Key : InvenType, Value : <Key : ItemID, Value : Value>
	public Dictionary<int, Dictionary<uint, uint>> DicInventory = new Dictionary<int, Dictionary<uint, uint>>();

	//Key : UniqueRuneID, Value : Item_Rune
	public Dictionary<uint, Item_Rune> DicRune = new Dictionary<uint, Item_Rune>();
	public uint UniqueRuneID = 0;
}

public class Item_Rune
{
	public uint UniqueRuneID;
	public uint RuneID;

	/// <summary>
	/// 장착중인 소환수 ID, 0이면 장착 안하고있음
	/// </summary>
	public uint SummonID;

	public Item_Rune(uint uniqueRuneID, uint runeID)
	{
		this.UniqueRuneID = uniqueRuneID;
		this.RuneID = runeID;
		this.SummonID = 0;
	}
}