using System;
using System.Collections.Generic;
using System.Linq;

public class UserData_Account : UserData<JsonData_Account>
{
	public enum eACCOUNT_TYPE
	{
		None = 0,
		Guest,
		Google,
		Apple,
	};

	#region PP_KEY
	static private string PP_KEY = "Account";

	static private string PP_KEY_ACCOUNTTYPE = "accountType";
	#endregion

	protected override string StrKey => PP_KEY;

#region 클라에서만 쓰는 데이터
	//계정 타입
	public eACCOUNT_TYPE AccountType = eACCOUNT_TYPE.Guest;
#endregion

    override public void Reset()
	{
		base.SetIntData(PP_KEY_ACCOUNTTYPE, (int)eACCOUNT_TYPE.None);

		//데이터 초기화
		base.Reset();
	}

	override public void LoadClientData()
	{
		base.LoadClientData();

		this.AccountType = (eACCOUNT_TYPE)base.GetIntData(PP_KEY_ACCOUNTTYPE, (int)eACCOUNT_TYPE.None);
	}

    override public void SaveClientData()
	{
		base.SaveClientData();

		base.SetIntData(PP_KEY_ACCOUNTTYPE, (int)this.AccountType);
	}

    protected override void dataProcessing()
    {
    }
}

public class JsonData_Account : BaseJsonData
{
}