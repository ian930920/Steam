using System.Threading.Tasks;

abstract public class BaseUserData
{
	abstract public void Reset();
	abstract public void LoadClientData();
	abstract public void SaveClientData();
	/// <summary>
	/// 이전에 클라 데이터로 저장한것들만 여기서 다시 load해서 저장
	/// </summary>
	virtual protected void createServerData() { }
	abstract public void LoadDataByString(string strData);
	virtual public async void SaveFirestoreData()
	{
		await Task.CompletedTask; // 최소한의 비동기 처리
	}

	abstract public string GetServerDataKey();
	abstract public string GetServerDataJson();

	protected void SetIntData(string strKey, int nValue)
	{
		SecurityPlayerPrefs.SetInt(strKey, nValue);
	}

	protected int GetIntData(string strKey, int nDefaultValue = 0)
	{
		int nValue = SecurityPlayerPrefs.GetInt(strKey, nDefaultValue);
		return nValue;
	}
	
	protected void SetFloatData(string strKey, float fValue)
	{
		SecurityPlayerPrefs.SetFloat(strKey, fValue);
	}

	protected float GetFloatData(string strKey, float fDefaultValue = 0.0f)
	{
		float fValue = SecurityPlayerPrefs.GetFloat(strKey, fDefaultValue);
		return fValue;
	}

	protected void SetStringData(string strKey, string strValue)
	{
		SecurityPlayerPrefs.SetString(strKey, strValue);
	}

	protected string GetStringData(string strKey, string strDefaultValue = "")
	{
		string strValue = SecurityPlayerPrefs.GetString(strKey, strDefaultValue);
		return strValue;
	}

	public static void DeleteData(string strKey)
    {
		SecurityPlayerPrefs.DeleteKey(strKey);
    }
}

abstract public class UserData<D> : BaseUserData where D : BaseJsonData, new()
{
	protected D Data = null;

    abstract protected string StrKey { get; }

	abstract protected void dataProcessing();

    public override void Reset()
    {
		this.Data = null;
		base.SetStringData(this.StrKey, "");
		this.createServerData();
		this.LoadClientData();
    }

    public override void LoadClientData()
    {
		if(this.Data != null) return;

		string strData = base.GetStringData(this.StrKey, "");
		if(string.IsNullOrEmpty(strData) == true) this.createServerData();
		else this.Data = Utility_Json.JsonToOject<D>(strData);

		if(this.Data == null) this.Data = new D();

		this.dataProcessing();
    }

    public override void SaveClientData()
    {
		base.SetStringData(this.StrKey, Utility_Json.ObjectToJson(this.Data));
    }

    public override void LoadDataByString(string strData)
    {
		this.Data = Utility_Json.JsonToOject<D>(strData);
		if(this.Data == null) this.Data = new D();
		//Utility.LogWarning($"{this.StrKey} {Utility_Json.ObjectToJson(this.Data)}");

		//데이터 후처리
		this.dataProcessing();
    }

	public override async void SaveFirestoreData()
	{
		//TODO Firebase
		//if(UserDataManager.Instance.Account.IsLinkedFirebase == false) return;
		if(this.Data == null) return;

		//TODO Firebase
		//await NativeManager.Instance.FirebaseSystem.UpdateData(this.StrKey, this.GetServerDataJson());
		await Task.CompletedTask; // 최소한의 비동기 처리

		ProjectManager.Instance.Log($"{this.StrKey} 파이어스토어에 저장 완료");
    }

    public override string GetServerDataKey()
    {
		return this.StrKey;
    }

    public override string GetServerDataJson()
    {
		if(this.Data == null) return null;

        return Utility_Json.ObjectToJson(this.Data);
    }
}

public abstract class BaseJsonData
{
}