using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseTableData<T, D> : MonoBehaviour where T : BaseTableData<T, D>  where D : class
{
    static private T s_instance = null;

    static public T Instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = ProjectManager.Instance.Table.GameObject.GetComponent<T>();
                if(s_instance == null) s_instance = ProjectManager.Instance.Table.GameObject.AddComponent<T>();
            }
            return s_instance;
        }
    }

    //Key : ID, Value : Data
	private Dictionary<int, D> m_dicData = new Dictionary<int, D>();
	protected List<D> m_listData = new List<D>();
    public int DataCount { get => this.m_dicData.Count; }

    /// <summary>
    /// 테이블의 데이터를 모두 비웁니다.
    /// </summary>
    private T clearData()
    {
        this.m_dicData.Clear();

        return s_instance;
    }

    public T LoadTable(string strPath)
    {
        this.clearData();

        this.addData(Utility_Json.JsonToOject<TableJson<D>>(Resources.Load<TextAsset>($"{strPath}").text).Json);

        this.dataProcessing();

        return s_instance;
    }

    public T LoadTables(string[] arrJson)
    {
        this.clearData();

        TableJson<D> table;
        for(int i = 0, nMax = arrJson.Length; i < nMax; ++i)
        {
            table = Utility_Json.JsonToOject<TableJson<D>>(Resources.Load<TextAsset>($"{arrJson[i]}").text);
            this.addData(table.Json);
        }

        this.dataProcessing();

        return s_instance;
    }

    public T LoadRemoteTable(string strJson)
    {
        this.clearData();

        this.addData(Utility_Json.JsonToOject<TableJson<D>>(strJson).Json);

        this.dataProcessing();

        return s_instance;
    }

    public T LoadRemoteTables(string[] strJson)
    {
        this.clearData();

        for(int i = 0, nMax = strJson.Length; i < nMax; ++i)
        {
            this.addData(Utility_Json.JsonToOject<TableJson<D>>(strJson[i]).Json);
        }

        this.dataProcessing();

        return s_instance;
    }

    /// <summary>
    /// 로드한 테이블을 기존 데이터에 추가합니다.
    /// </summary>
    /// <param name="dicData">로드한 테이블</param>
    /// <returns>로드해온 테이블이 비어있었는지</returns>
    private bool addData(Dictionary<int, D> dicData)
	{
        if(dicData == null) return false;

        this.m_dicData = this.m_dicData.Concat(dicData).ToDictionary(item => item.Key, item => item.Value);
        this.m_listData = this.m_dicData.Values.ToList();

		return true;
	}

    public D GetData(int nID)
    {
        if(this.m_dicData.ContainsKey(nID) == false)
        {
            //ProjectManager.Instance.Log($"Table : {this.name}, tableID : {nID} is Null");
            return null;
        }

        return this.m_dicData[nID];
    }

    public D GetDataByListIdx(int nIdx)
    {
        return this.m_listData[Mathf.Clamp(nIdx, 0, this.m_listData.Count - 1)];
    }

    public D GetRandomData()
    {
        return this.m_listData[UnityEngine.Random.Range(0, this.m_listData.Count)];
    }

    public Dictionary<int, D>.Enumerator GetEnumerator()
    {
        return this.m_dicData.GetEnumerator();
    }

    public bool ContainsKey(int nID)
    {
        return this.m_dicData.ContainsKey(nID);
    }

    protected virtual void dataProcessing()
    {
        iTableData data = null;
        Dictionary<int, D>.Enumerator enumData = this.GetEnumerator();
        while(enumData.MoveNext())
        {
            data = enumData.Current.Value as iTableData;
            if(data == null) continue;

            data.tableID = enumData.Current.Key;
        }

        this.m_dicData = this.m_dicData.OrderBy(data => data.Key).ToDictionary(data => data.Key, data => data.Value);
        this.m_listData = this.sortListData(this.m_listData);
    }

    protected virtual List<D> sortListData(List<D> listData) { return listData; }
}