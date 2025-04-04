using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : BaseManager<ResourceManager>
{
    public enum eATLAS_ID
    {
        UI = 20001,
        Ingame
    }

    public enum eRES_ID
    {
    }

    #region Resource path
    //리소스 경로 관련
    static readonly public string PATH_ANI_BUTTON = "Animations/UI/Button/Ani_Button";
    static readonly public string PATH_ANI_POPUP = "Animations/UI/Popup/Popup";
    static readonly public string PATH_ANI_AUDIOSORCE = "Prefabs/Audio";
    #endregion
    
    //버튼 애니
    public RuntimeAnimatorController Animator_Button { get; private set; } = null;
    public RuntimeAnimatorController Animator_Popup { get; private set; } = null;

    //Key : resID, value : Sound
    private Dictionary<int, AudioClip> m_dicSound = new Dictionary<int, AudioClip>();

    //Key : resID, value : atlas
    private Dictionary<int, SpriteAtlas> m_dicAtlas = new Dictionary<int, SpriteAtlas>();

    //Key : resID, Value : Material
    private Dictionary<int, Material> m_dicMaterial = new Dictionary<int, Material>();

    //Key : ePOPUP_ID, Value : Gameobject
    private Dictionary<int, GameObject> m_dicPopup = new Dictionary<int, GameObject>();

    protected override void init()
    {
        //버튼 애니 미리 로드
        this.Animator_Button = Resources.Load<RuntimeAnimatorController>(PATH_ANI_BUTTON);
        this.Animator_Popup = Resources.Load<RuntimeAnimatorController>(PATH_ANI_POPUP);
    }

    public override void ResetManager()
    {
        //딕셔너리들 초기화
        this.m_dicSound.Clear();
        this.m_dicAtlas.Clear();
        this.m_dicPopup.Clear();
    }

    public void LoadResByTable()
    {
        TableData.TableData_Resource data = null;
        Dictionary<int, TableData.TableData_Resource>.Enumerator enumData = ProjectManager.Instance.Table.Resource.GetEnumerator();
        while(enumData.MoveNext())
        {
            data = enumData.Current.Value;
            switch((TableData.TableResource.eTYPE)data.type)
            {
                case TableData.TableResource.eTYPE.Atlas:
                {
                    if(this.m_dicAtlas.ContainsKey(data.tableID) == true) continue;

                    this.m_dicAtlas.Add(data.tableID, Resources.Load<SpriteAtlas>(data.path));
                }
                break;
                case TableData.TableResource.eTYPE.Sound:
                {
                    if(this.m_dicSound.ContainsKey(data.tableID) == true) continue;

                    this.m_dicSound.Add(data.tableID, Resources.Load<AudioClip>(data.path));
                }
                break;
                case TableData.TableResource.eTYPE.Popup:
                {
                    if(this.m_dicPopup.ContainsKey(data.tableID) == true) continue;

                    this.m_dicPopup.Add(data.tableID, Resources.Load<GameObject>(data.path));
                }
                break;
                case TableData.TableResource.eTYPE.Material:
                {
                    if(this.m_dicMaterial.ContainsKey(data.tableID) == true) continue;

                    this.m_dicMaterial.Add(data.tableID, Resources.Load<Material>(data.path));
                }
                break;
            }
        }
    }

    public GameObject InstantiatePopup(int nPopupID, Transform transParent)
    {
        if(this.m_dicPopup.ContainsKey(nPopupID) == false) return null;

        return Instantiate(this.m_dicPopup[nPopupID], transParent);
    }

    public Sprite GetSprite(int nResID, string strName)
    {
        if(this.m_dicAtlas.ContainsKey(nResID) == false)
        {
            ProjectManager.Instance.Log($"없는 아틀라스 {nResID}");
            return null;
        }

        try
        {
            return this.m_dicAtlas[nResID].GetSprite(strName);
        }
        catch
        {
            ProjectManager.Instance.Log($"아틀라스 {nResID}에 없는 이미지 {strName}");
            return null;
        }
    }

    public Sprite GetSpriteByAtlas(eATLAS_ID eAtlas, string strName)
    {
        return this.GetSprite((int)eAtlas, strName);
    }

    #region Material
    public Material GetMaterial(int nResKey)
    {
        if(this.m_dicMaterial.ContainsKey(nResKey) == false)
        {
            //Debug.Log(nResKey);
            return null;
        }

        return this.m_dicMaterial[nResKey];
    }
    #endregion

    public AudioClip GetAudioClip(int nResKey)
    {
        if(this.m_dicSound.ContainsKey(nResKey) == false)
        {
            //ProjectManager.Instance.LogWarning($"FxObject 중 {nResKey}는 존재하지 않습니다");
            return null;
        }

        return this.m_dicSound[nResKey];
    }

    public GameObject Instantiate(string strPath)
    {
        return GameObject.Instantiate(Resources.Load<GameObject>(strPath));
    }

    public T InstantiateAndGetComponent<T>(string strPath) where T : class
    {
        return GameObject.Instantiate(Resources.Load<GameObject>(strPath)).GetComponent<T>();
    }

    public T InstantiateAndGetComponent<T>(string strPath, Transform transParent) where T : class
    {
        return GameObject.Instantiate(Resources.Load<GameObject>(strPath), transParent).GetComponent<T>();
    }
}