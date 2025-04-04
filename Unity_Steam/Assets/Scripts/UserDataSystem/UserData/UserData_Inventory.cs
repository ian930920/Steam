using System.Collections.Generic;

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

    #region Cheat
	public void Cheat_AddItem()
    {

	}
    #endregion
}

public class JsonData_Inventory : BaseJsonData
{
	//Key : InvenType, Value : <Key : ItemID, Value : Value>
	public Dictionary<int, Dictionary<uint, uint>> DicInventory = new Dictionary<int, Dictionary<uint, uint>>();
}