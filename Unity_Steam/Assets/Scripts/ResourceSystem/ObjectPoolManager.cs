using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : BaseManager<ObjectPoolManager>
{
    public static readonly int CONST_OBJECTPOOL_ADD = 10;
    public static readonly string STR_NAME_OBJECTPOOL = "Panel_ObjectPool";
    public static readonly string STR_NAME_SOUND = "Panel_Sound";

    private Dictionary<int, ObjectPool> m_dicObjectPool = new Dictionary<int, ObjectPool>();
	private Dictionary<int, ObjectPool> m_dicFx = new Dictionary<int, ObjectPool>();
	private ObjectPool m_opSound = null;

    public GameObject GobjParent { private set; get; } = null;
    private GameObject m_gobjSound = null;

    private struct stObjectPool
    {
        public TableData.TableObjectPool.eID eID { get; set; }
        public int nCount { get; set; }
        public Transform trnasParent { get; set; }

        public stObjectPool(TableData.TableObjectPool.eID eID, int nCount, Transform trnasParent)
        {
            this.eID = eID;
            this.nCount = nCount;
            this.trnasParent = trnasParent;
        }
    }

    private struct stObjectPoolByPath
    {
        public TableData.TableObjectPool.eID eID { get; set; }
        public string strPath { get; set; }
        public int nCount { get; set; }
        public Transform trnasParent { get; set; }

        public stObjectPoolByPath(TableData.TableObjectPool.eID eID, string strPath, int nCount, Transform trnasParent)
        {
            this.eID = eID;
            this.strPath = strPath;
            this.nCount = nCount;
            this.trnasParent = trnasParent;
        }
    }

    protected override void init()
    {
        this.GobjParent = new GameObject(STR_NAME_OBJECTPOOL);
        this.GobjParent.transform.SetParent(ProjectManager.Instance.transform);

        this.m_gobjSound = new GameObject(STR_NAME_SOUND);
        this.m_gobjSound.transform.SetParent(this.GobjParent.transform);
        this.m_opSound = new ObjectPool(ResourceManager.PATH_ANI_AUDIOSORCE, CONST_OBJECTPOOL_ADD, this.m_gobjSound.transform);
    }

    public override void ResetManager()
    {
        this.inactiveAllObjects();

        this.m_dicObjectPool.Clear();
        this.m_dicFx.Clear();
        this.m_opSound = null;
    }

    public void InitObjectPool()
    {
        this.m_dicObjectPool.Clear();
        this.m_dicFx.Clear();

        //로드
        this.loadByTable();
    }

    private void loadByTable()
    {
        //EX) transParent
        Transform transReactiveUI = null; //ProjectManager.Instance.Scene.GetCurrScene<TitleScene>().transform;
        Transform transFxUI = null; //CustomSceneManager.Instance.MainScene.TransFxUIRoot;

        TableData.TableData_ObjectPool data = null;
        Dictionary<int, TableData.TableData_ObjectPool>.Enumerator enumData = ProjectManager.Instance.Table.ObjectPool.GetEnumerator();
        while(enumData.MoveNext())
        {
            data = enumData.Current.Value;
            if(data.poolCount == 0) continue;

            int nKey = data.tableID;
            switch((TableData.TableObjectPool.eTYPE)data.type)
            {
                case TableData.TableObjectPool.eTYPE.Fx:
                {
                    if(this.m_dicFx.ContainsKey(nKey) == true) continue;

                    ObjectPool objectPool = new ObjectPool(data.path, data.poolCount, transFxUI);
                    this.m_dicFx.Add(nKey, objectPool);
                }
                break;
                case TableData.TableObjectPool.eTYPE.UI:
                {
                    if(this.m_dicObjectPool.ContainsKey(nKey) == true) continue;

                    ObjectPool objectPool = new ObjectPool(data.path, data.poolCount, transReactiveUI);
                    this.m_dicObjectPool.Add(nKey, objectPool);
                }
                break;
            }
        }
    }

    public T GetPoolObjectComponent<T>(TableData.TableObjectPool.eID eID) where T : class
    {
        int nID = (int)eID;
        if(this.m_dicObjectPool.ContainsKey(nID) == false)
        {
            //Debug.Log(eID);
            return null;
        }

		return this.m_dicObjectPool[nID].GetObjectComponent<T>();
    }

    public GameObject GetPoolObject(TableData.TableObjectPool.eID eID)
    {
        int nID = (int)eID;

        if(this.m_dicObjectPool.ContainsKey(nID) == false) return null;

        return this.m_dicObjectPool[nID].GetObejct();
    }

    private void inactiveAllObjects()
    {
        Dictionary<int, ObjectPool>.Enumerator enumOP = this.m_dicObjectPool.GetEnumerator();
        while(enumOP.MoveNext())
        {
            enumOP.Current.Value.InactiveAllObjects();
        }
        
        enumOP = this.m_dicFx.GetEnumerator();
        while(enumOP.MoveNext())
        {
            enumOP.Current.Value.InactiveAllObjects();
        }

        this.m_opSound.InactiveAllObjects();
    }

    public void DeactiveAllObjects(TableData.TableObjectPool.eID eID)
    {
        int nID = (int)eID;
        if(this.m_dicObjectPool.ContainsKey(nID) == false) return;

        this.m_dicObjectPool[nID].InactiveAllObjects();
    }

    #region Effect
    public void PlayEffect(TableData.TableObjectPool.eID eEffectID, Vector3 vecPos)
    {
        int nResKey = (int)eEffectID;
        if(this.m_dicFx.ContainsKey(nResKey) == false) return;

        //재생
        this.m_dicFx[nResKey].GetObjectComponent<BaseFx>().Play(vecPos);
    }

    public void PlayItemDropEffect(int nItemID, Vector3 vecPos)
    {
        int nResKey = (int)TableData.TableObjectPool.eID.Effect_ItemDrop;
        if(this.m_dicFx.ContainsKey(nResKey) == false) return;

        //재생
        this.m_dicFx[nResKey].GetObjectComponent<Fx_Particle_Item>().SetItem(nItemID, vecPos);
    }
    
    public void PlayCountEffectByBigInt(System.Numerics.BigInteger nValue, Vector3 vecPos)
    {
        int nResKey = (int)TableData.TableObjectPool.eID.Effect_Count;
        if(this.m_dicObjectPool.ContainsKey(nResKey) == false) return;

        //재생
        this.m_dicObjectPool[nResKey].GetObjectComponent<Fx_Animation_Count>().Init(nValue, vecPos);
    }

    public void PlayCountEffectByUlong(ulong uValue, Vector3 vecPos)
    {
        int nResKey = (int)TableData.TableObjectPool.eID.Effect_Count;
        if(this.m_dicObjectPool.ContainsKey(nResKey) == false) return;

        //재생
        this.m_dicObjectPool[nResKey].GetObjectComponent<Fx_Animation_Count>().Init(uValue, vecPos);
    }
    #endregion

    #region Sound
    private BaseSound m_soundBGM = null;

    private void playSound(BaseSound.eID eSoundID, BaseSound.eTYPE eType, float fVolume = 1.0f)
    {
        if(this.m_opSound == null) return;

        this.m_opSound.GetObjectComponent<BaseSound>()?.Play(eSoundID, eType, fVolume);
    }

    public void PlayBGM(bool bActive)
    {
        if(this.m_soundBGM == null) this.m_soundBGM = this.m_opSound.GetObjectComponent<BaseSound>();
        
        if(bActive == true) this.m_soundBGM.Play(BaseSound.eID.BGM, BaseSound.eTYPE.BGM, BaseSound.BGM_VOLUME);
        else this.m_soundBGM.Stop();
    }

    public void SetBGMVolume()
    {
        if(this.m_soundBGM == null) return;

        this.m_soundBGM.SetVolume(BaseSound.eTYPE.BGM, BaseSound.BGM_VOLUME);
    }

    public void PlayEffectSound(BaseSound.eID eSoundID, float fVolume = 1.0f)
    {
        //TODO UserDataManager
        //if(UserDataManager.Instance.Setting.SoundEffect.IsActive == false) return;

        this.playSound(eSoundID, BaseSound.eTYPE.Effect, fVolume);
    }
    #endregion
}

public class ObjectPool
{
    private GameObject m_gobjSrc = null;
    private Transform m_transParent = null;

	private List<GameObject> m_listObj = new List<GameObject>();

    public ObjectPool(GameObject gobj, int nCount, Transform transParent = null)
    {
        //오브젝트 로드
        this.m_gobjSrc = gobj;

        this.init(this.m_gobjSrc, nCount, transParent);
    }

    public ObjectPool(string strPath, int nCount, Transform transParent = null)
    {
		//오브젝트 로드
        this.m_gobjSrc = Resources.Load<GameObject>(strPath);
        if(this.m_gobjSrc == null) return;

        this.init(this.m_gobjSrc, nCount, transParent);
    }

    private void init(GameObject gobj, int nCount, Transform transParent = null)
    {
        //오브젝트 로드
        this.m_gobjSrc = gobj;

        //Panel생성
        if(transParent != null) this.m_transParent = transParent;
        else this.m_transParent = ProjectManager.Instance.ObjectPool.GobjParent.transform;

        //미리생성
        this.addObject(nCount);
    }

    private void addObject(int nCount)
    {
        for(int i = 0; i < nCount; ++i)
        {
            this.m_listObj.Add(GameObject.Instantiate(this.m_gobjSrc, this.m_transParent));
            this.m_listObj[this.m_listObj.Count - 1].SetActive(false);
        }
    }

    public GameObject GetObejct()
    {
        if(this.m_listObj.Count == 0 || this.m_listObj.Any(obj => obj.activeSelf == false) == false)
        {
            //활성화할게 없다는 것 추가 생성해주자
            this.addObject(ObjectPoolManager.CONST_OBJECTPOOL_ADD);
        }

        //활성화 안된 오브젝트 넘겨주기
        return this.m_listObj.First(obj => obj.activeSelf == false);
    }

    public T GetObjectComponent<T>() where T : class
    {
        if(this.m_listObj.Count == 0) return null;

        return this.GetObejct().GetComponent<T>();
    }

    public void InactiveAllObjects()
    {
        for(int i = 0; i < this.m_listObj.Count; ++i)
        {
            if(this.m_listObj[i].activeSelf == false) continue;

            this.m_listObj[i].SetActive(false);
        }
    }
}