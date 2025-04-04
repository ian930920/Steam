using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TableData;

public class TableManager : BaseManager<TableManager>
{
	private static readonly string STR_PATH_TABLE = "Tables";
	public GameObject GameObject { private set; get; } = null;

#region 테이블
    public TableString String { get; private set; } = null;
	public TableResource Resource { get; private set; } = null;
	public TableObjectPool ObjectPool { get; private set; } = null;
	public TableDefine Define { get; private set; } = null;
	public TableTime Time { get; private set; } = null;
	public TableItem Item { get; private set; } = null;
#endregion

    protected override void init()
    {
		if(this.GameObject == null)
		{
			this.GameObject = new GameObject(STR_PATH_TABLE);
			DontDestroyOnLoad(this.GameObject);
		}
    }

    public void LoadClientTables()
	{
		string strPath = STR_PATH_TABLE;

		this.Define = TableDefine.Instance.LoadTable($"{strPath}/DefineData");
		this.Time = TableTime.Instance.LoadTable($"{strPath}/TimeData");
		this.Item = TableItem.Instance.LoadTable($"{strPath}/ItemData");

		//String
		this.String = TableString.Instance.LoadTables(new string[]
		{
			$"{strPath}/StringData",
		});

		//Resource
		this.Resource = TableResource.Instance.LoadTables(new string[]
		{
			$"{strPath}/ResourceData",
			$"{strPath}/ResourcePopupData",
			$"{strPath}/ResourceSoundData",
		});
		this.ObjectPool = TableObjectPool.Instance.LoadTable($"{strPath}/ObjectPoolData");

#if UNITY_EDITOR
		if(true)
#else
		if(NativeManager.Instance.RemoteConfigSystem.IsRemoteTable == true)
#endif
		{
			//ex) this.Cat = TableCat.Instance.LoadTable($"{strPath}/Remote/CatData");

			//TODO Delete
		}
	}

	public void LoadRemoteTables(Dictionary<string, string> dicTable)
	{
		//ex) this.Cat = TableCat.Instance.LoadRemoteTable(dicTable["CatData"]);

#if UNITY_EDITOR
		if(false)
#else
		if(NativeManager.Instance.RemoteConfigSystem.IsRemoteTable == false)
#endif
		{
			//TODO Delete
		}
	}
}

public class TableJson<T> where T : class
{
    public Dictionary<uint, T> Json { get; set; }
}

public interface iTableData
{
    public uint tableID { get; set; }
}