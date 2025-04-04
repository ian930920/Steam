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
			Dictionary<int, uint>.Enumerator enumInven;
			Dictionary<int, Dictionary<int, uint>>.Enumerator enumDic = base.Data.DicInventory.GetEnumerator();
			while(enumDic.MoveNext())
			{
				int nInvenType = (int)enumDic.Current.Key;
				if(base.Data.DicInventory.ContainsKey(nInvenType) == false) base.Data.DicInventory.Add(nInvenType, new Dictionary<int, uint>());

				enumInven = enumDic.Current.Value.GetEnumerator();
				while(enumInven.MoveNext())
				{
					base.Data.DicInventory[nInvenType].Add(enumInven.Current.Key, enumInven.Current.Value);
				}
			}
		}
	}

	public bool IsContainsItem(int nItemID)
    {
		//인벤에 있는지 확인
		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(nItemID);
		if(base.Data.DicInventory.ContainsKey(nInvenType) == false) return false;

		return base.Data.DicInventory[nInvenType].ContainsKey(nItemID);
    }

	public bool IsContainsItem(TableData.TableItem.eID eItemID)
    {
		return this.IsContainsItem((int)eItemID);
    }

	public stItem GetItem(int nItemID)
    {
		return new stItem(nItemID, this.getItemCount(nItemID));
    }

	public stItem GetItem(TableData.TableItem.eID eItemID)
    {
		return this.GetItem((int)eItemID);
    }

	private uint getItemCount(int nItemID)
    {
		if(this.IsContainsItem(nItemID) == false) return 0;

		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(nItemID);
		return base.Data.DicInventory[nInvenType][nItemID];
    }

	public int GetCountByCategory(TableData.TableItem.eINVEN_TYPE eType)
	{
		if(base.Data.DicInventory.ContainsKey((int)eType) == false) return 0;

		return base.Data.DicInventory[(int)eType].Count;
	}

	public bool IsUseable(int nItemID, uint nUseCount)
	{
		return nUseCount <= this.getItemCount(nItemID);
	}

    public void AddItem(int nItemID, uint nCount)
    {
		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(nItemID);
		if(this.IsContainsItem(nItemID) == false)
		{
			if(base.Data.DicInventory.ContainsKey(nInvenType) == false) base.Data.DicInventory.Add(nInvenType, new Dictionary<int, uint>());
			base.Data.DicInventory[nInvenType].Add(nItemID, nCount);
		}
		else base.Data.DicInventory[nInvenType][nItemID] += nCount;

		//저장
		base.SaveClientData();
	}

    public void UseItem(int nItemID, uint nCount)
    {
		//0이면 할 필요 없음
		if(nCount == 0) return;

		int nInvenType = (int)ProjectManager.Instance.Table.Item.GetInvenType(nItemID);

		uint nResult = base.Data.DicInventory[nInvenType][nItemID] - nCount;
		base.Data.DicInventory[nInvenType][nItemID] = nResult;

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
	public Dictionary<int, Dictionary<int, uint>> DicInventory = new Dictionary<int, Dictionary<int, uint>>();
}