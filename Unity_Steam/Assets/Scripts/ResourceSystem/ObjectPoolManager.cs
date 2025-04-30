using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using DamageNumbersPro;

public class ObjectPoolManager : BaseManager<ObjectPoolManager>
{
    public static readonly int CONST_OBJECTPOOL_ADD = 10;
    public static readonly string STR_NAME_OBJECTPOOL = "Panel_ObjectPool";
    public static readonly string STR_NAME_SOUND = "Panel_Sound";

    private Dictionary<uint, ObjectPool> m_dicObjectPool = new Dictionary<uint, ObjectPool>();
	private Dictionary<uint, ObjectPool> m_dicFx = new Dictionary<uint, ObjectPool>();
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
        Transform transRootGobj = ProjectManager.Instance.Scene.CurrScene.TransRootObjectPool;
        Transform transRootUI = ProjectManager.Instance.Scene.CurrScene.BaseHUD.TransReactiveParent;

        TableData.TableData_ObjectPool data = null;
        Dictionary<uint, TableData.TableData_ObjectPool>.Enumerator enumData = ProjectManager.Instance.Table.ObjectPool.GetEnumerator();
        while(enumData.MoveNext())
        {
            data = enumData.Current.Value;
            if(data.poolCount == 0) continue;

            uint nKey = data.tableID;
            switch((TableData.TableObjectPool.eTYPE)data.type)
            {
                case TableData.TableObjectPool.eTYPE.Prefab_Root_Normal:
                {
                    if(this.m_dicObjectPool.ContainsKey(nKey) == true) continue;

                    ObjectPool objectPool = new ObjectPool(data.path, data.poolCount, transRootGobj);
                    this.m_dicObjectPool.Add(nKey, objectPool);
                }
                break;

                case TableData.TableObjectPool.eTYPE.Prefab_Root_UI:
                {
                    if(this.m_dicObjectPool.ContainsKey(nKey) == true) continue;

                    ObjectPool objectPool = new ObjectPool(data.path, data.poolCount, transRootUI);
                    this.m_dicObjectPool.Add(nKey, objectPool);
                }
                break;

                case TableData.TableObjectPool.eTYPE.FX_Root_Normal:
                {
                    if(this.m_dicFx.ContainsKey(nKey) == true) continue;

                    ObjectPool objectPool = new ObjectPool(data.path, data.poolCount, transRootGobj);
                    this.m_dicFx.Add(nKey, objectPool);
                }
                break;

                case TableData.TableObjectPool.eTYPE.FX_Root_UI:
                {
                    if(this.m_dicFx.ContainsKey(nKey) == true) continue;

                    ObjectPool objectPool = new ObjectPool(data.path, data.poolCount, transRootUI);
                    this.m_dicFx.Add(nKey, objectPool);
                }
                break;
            }
        }
    }

    public T GetPoolObjectComponent<T>(TableData.TableObjectPool.eID eID) where T : class
    {
        uint tableID = (uint)eID;
        if(this.m_dicObjectPool.ContainsKey(tableID) == false)
        {
            //Debug.Log(eID);
            return null;
        }

		return this.m_dicObjectPool[tableID].GetObjectComponent<T>();
    }

    public GameObject GetPoolObject(TableData.TableObjectPool.eID eID)
    {
        uint tableID = (uint)eID;
        if(this.m_dicObjectPool.ContainsKey(tableID) == false) return null;

        return this.m_dicObjectPool[tableID].GetObejct();
    }

    private void inactiveAllObjects()
    {
        Dictionary<uint, ObjectPool>.Enumerator enumOP = this.m_dicObjectPool.GetEnumerator();
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
        uint tableID = (uint)eID;
        if(this.m_dicObjectPool.ContainsKey(tableID) == false) return;

        this.m_dicObjectPool[tableID].InactiveAllObjects();
    }

    #region Effect
    public void PlayEffect(uint resID, Vector3 vecPos, UnityAction funcOnFinish = null)
    {
        if(this.m_dicFx.ContainsKey(resID) == false) return;

        //재생
        this.m_dicFx[resID].GetObjectComponent<BaseFx>().Play(vecPos, funcOnFinish);
    }

    public void PlayEffect(TableData.TableObjectPool.eID eEffectID, Vector3 vecPos, UnityAction funcOnFinish = null)
    {
        this.PlayEffect((uint)eEffectID, vecPos, funcOnFinish);
    }

    public void PlayItemDropEffect(int nItemID, Vector3 vecPos)
    {
        uint resKey = (uint)TableData.TableObjectPool.eID.Effect_ItemDrop;
        if(this.m_dicFx.ContainsKey(resKey) == false) return;

        //재생
        this.m_dicFx[resKey].GetObjectComponent<Fx_Particle_Item>().SetItem(nItemID, vecPos);
    }

    public void PlayCountEffectByUlong(stDamage damage, Vector3 vecPos)
    {
        uint resKey = (uint)TableData.TableObjectPool.eID.Effect_Count;
        if(this.m_dicObjectPool.ContainsKey(resKey) == false) return;

        //재생
        this.m_dicObjectPool[resKey].GetObjectComponent<Fx_Animation_Count>().Init(damage, vecPos);
    }

    public void PlayCountEffect_Damage(stDamage damage, Vector3 vecPos)
    {
        uint resKey = (uint)TableData.TableObjectPool.eID.Effect_Count_Damage;
        if(this.m_dicObjectPool.ContainsKey(resKey) == false) return;

        //재생
        this.m_dicObjectPool[resKey].GetObjectComponent<DamageNumber>().Spawn(vecPos, damage.Value);
    }

    public void PlayCountEffect_Heal(stDamage damage, Vector3 vecPos)
    {
        uint resKey = (uint)TableData.TableObjectPool.eID.Effect_Count_Heal;
        if(this.m_dicObjectPool.ContainsKey(resKey) == false) return;

        //재생
        this.m_dicObjectPool[resKey].GetObjectComponent<DamageNumber>().Spawn(vecPos + new Vector3(0, 0.5f), damage.Value);
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