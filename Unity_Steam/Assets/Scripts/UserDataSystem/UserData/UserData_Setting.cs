using System.Collections.Generic;

public class UserData_Setting : UserData<JsonData_Setting>
{
	public struct stSound
    {
		public bool IsActive;
		public float Volume;

		public stSound(bool isActive, float fVolume)
        {
			this.IsActive = isActive;
			this.Volume = fVolume;
        }
    }

	#region PP_KEY
	static private string PP_KEY = "Setting";
	static private string PP_KEY_SOUND = "sound";
    #endregion

    protected override string StrKey => PP_KEY;

    private stSound[] m_arrSound = null;
	public stSound SoundBGM { get => this.m_arrSound[(int)BaseSound.eTYPE.BGM]; }
	public stSound SoundEffect { get => this.m_arrSound[(int)BaseSound.eTYPE.Effect]; }

	public override void Reset()
    {
		//넣어주기~
		this.m_arrSound = new stSound[]
		{
			new stSound(true, 1.0f),
			new stSound(true, 1.0f),
		};
		base.SetStringData(PP_KEY_SOUND, Utility_Json.ObjectToJson(this.m_arrSound));

		base.Reset();
    }

	override public void LoadClientData()
	{
		base.LoadClientData();

		string strData = base.GetStringData(PP_KEY_SOUND, "");
		if(string.IsNullOrEmpty(strData) == false) this.m_arrSound = Utility_Json.JsonToOject<stSound[]>(strData);
		else
        {
			//넣어주기~
			this.m_arrSound = new stSound[]
			{
				new stSound(true, 1.0f),
				new stSound(true, 1.0f),
			};
        }
	}

	override public void SaveClientData()
	{
		base.SaveClientData();

		base.SetStringData(PP_KEY_SOUND, Utility_Json.ObjectToJson(this.m_arrSound));
	}

    protected override void dataProcessing()
    {
    }

    public void SetActive(BaseSound.eTYPE eType, bool isActive)
	{
		this.m_arrSound[(int)eType] = new stSound(isActive, this.m_arrSound[(int)eType].Volume);
		this.SaveClientData();
	}

	public void SetVolume(BaseSound.eTYPE eType, float fVolume)
	{
		this.m_arrSound[(int)eType] = new stSound(this.m_arrSound[(int)eType].IsActive, fVolume);
		this.SaveClientData();
	}
}

public class JsonData_Setting : BaseJsonData
{

}