using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TableData;

public class TableManager : BaseSingleton<TableManager>
{
	private static readonly string STR_PATH_TABLE = "Tables";
	public GameObject Parent { private set; get; } = null;

#region 테이블
    public TableString String { get; private set; } = null;
	public TableResource Resource { get; private set; } = null;
	public TableObjectPool ObjectPool { get; private set; } = null;
	public TableDefine Define { get; private set; } = null;
	public TableTime Time { get; private set; } = null;
	public TableItem Item { get; private set; } = null;
	public TableUser User { get; private set; } = null;
	public TableEnemy Enemy { get; private set; } = null;
	public TableSkill Skill { get; private set; } = null;
	public TableSummon Summon { get; private set; } = null;
	public TableSummonObj SummonObj { get; private set; } = null;
	public TableStatus Status { get; private set; } = null;
	public TableRune Rune { get; private set; } = null;
	#endregion

	public override void Initialize()
    {
        //필수
        if(base.IsInitialized == true) return;

        if(this.Parent == null)
		{
			this.Parent = new GameObject(STR_PATH_TABLE);
			this.Parent.transform.SetParent(this.transform);
			DontDestroyOnLoad(this.Parent);
		}

        base.IsInitialized = true;
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
			$"{strPath}/StringCharacterData",
			$"{strPath}/StringStatusData",
			$"{strPath}/StringItemData",
		});

		//Resource
		this.Resource = TableResource.Instance.LoadTables(new string[]
		{
			$"{strPath}/ResourceData",
			$"{strPath}/ResourcePopupData",
			$"{strPath}/ResourceSoundData",
		});
		this.ObjectPool = TableObjectPool.Instance.LoadTable($"{strPath}/ObjectPoolData");

		//캐릭터
		this.User = TableUser.Instance.LoadTable($"{strPath}/UserData");
		this.Enemy = TableEnemy.Instance.LoadTable($"{strPath}/EnemyData");
		this.Skill = TableSkill.Instance.LoadTable($"{strPath}/SkillData");
		this.Summon = TableSummon.Instance.LoadTable($"{strPath}/SummonData");
		this.SummonObj = TableSummonObj.Instance.LoadTable($"{strPath}/SummonObjData");
		this.Status = TableStatus.Instance.LoadTable($"{strPath}/StatusData");
		this.Rune = TableRune.Instance.LoadTable($"{strPath}/RuneData");

#if UNITY_EDITOR
		if(true)
#else
		if(true)
		//if(NativeManager.Instance.RemoteConfigSystem.IsRemoteTable == true)
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
		if(false)
		//if(NativeManager.Instance.RemoteConfigSystem.IsRemoteTable == false)
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