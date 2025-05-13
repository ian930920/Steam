using System.Collections.Generic;
using System.Linq;

public class UserData_Inventory : UserData<JsonData_Inventory>
{
	#region PP_KEY
	static private string PP_KEY = "Inventory";
	#endregion

    protected override string StrKey => PP_KEY;

	public int RuneCount => base.Data.DicRune.Count;

	protected override void dataProcessing()
	{
		var enumItem = TableManager.Instance.Item.GetEnumerator();
		while(enumItem.MoveNext())
		{
			if(base.Data.DicInventory.ContainsKey(enumItem.Current.Key) == true) continue;

			this.addItem(enumItem.Current.Key, 0);
		}
	}

	public stItem GetItem(uint itemID)
    {
		return new stItem(itemID, this.getItemCount(itemID));
    }

	public stItem GetItem(TableData.TableItem.eID eItemID)
    {
		return this.GetItem((uint)eItemID);
    }

	private int getItemCount(uint itemID)
    {
		return base.Data.DicInventory[itemID];
    }

	public bool IsUseable(stItem stItem)
	{
		return this.isUseable(stItem.ItemID, stItem.Count);
	}

	private bool isUseable(uint itemID, int nUseCount)
	{
		return nUseCount <= this.getItemCount(itemID);
	}

	public void AddItem(stItem stItem)
    {
        switch(stItem.ItemType)
        {
            case TableData.TableItem.eTYPE.Cunsume:
			{
				this.addItem(stItem.ItemID, stItem.Count);

				//업데이트
				UserDataManager.Instance.DoItemCountRefreshEvent(stItem.ItemID);
			}
            break;

            case TableData.TableItem.eTYPE.Rune:
			{
				this.addRune(stItem.ItemID);
			}
			break;
        }
    }

    private void addItem(uint itemID, int nCount)
    {
		if(base.Data.DicInventory.ContainsKey(itemID) == false) base.Data.DicInventory.Add(itemID, nCount);
		else base.Data.DicInventory[itemID] += nCount;

		//저장
		base.SaveClientData();
	}

	public void UseItem(stItem stItem)
    {
		this.useItem(stItem.ItemID, stItem.Count);
	}

    private void useItem(uint itemID, int nCount)
    {
		//0이면 할 필요 없음
		if(nCount == 0) return;

		base.Data.DicInventory[itemID] -= nCount;

		//저장
		base.SaveClientData();
	}

#region Rune
	public Item_Rune GetRune(uint uniqueRuneID)
	{
		if(base.Data.DicRune.ContainsKey(uniqueRuneID) == false) return null;

		return base.Data.DicRune[uniqueRuneID];
	}

	private void addRune(uint runeID)
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
		var enumData = TableManager.Instance.Rune.GetEnumerator();
		while(enumData.MoveNext())
		{
			this.addRune(enumData.Current.Key);
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
	//Key : ItemID, Value : Value
	public Dictionary<uint, int> DicInventory = new Dictionary<uint, int>();

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