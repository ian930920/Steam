using System;
using UnityEngine;

/// <summary>
/// //Singleton Ojbect is only Managers
/// </summary>
/// <typeparam name="T">Manager</typeparam>
public abstract class BaseSingleton<T> : MonoBehaviour where T : BaseSingleton<T>
{
    //TODO 프로젝트에 Tag 추가 : "Manager" "Scene"

    static private string s_strGameObjectName = "Manager";

    static readonly private Lazy<T> s_instance = new Lazy<T>(()=>
    {
        T instance = FindAnyObjectByType(typeof(T)) as T;
        if(instance == null)
        {
            var gobj = GameObject.FindGameObjectWithTag(s_strGameObjectName);
            if(gobj == null)
            {
                gobj = new GameObject(s_strGameObjectName);
                gobj.tag = s_strGameObjectName;
            }
            instance = gobj.AddComponent<T>();
        }

        //아직 파괴 안되게 세팅 안됐다면 세팅
        if(instance.gameObject.scene.name != "DontDestroyOnLoad") DontDestroyOnLoad(instance.gameObject);

        //초기화 안했으면 초기화
        if(instance.IsInitialized == false) instance.Initialize();

        return instance;
    });

    static public T Instance { get { return s_instance.Value; } }
    public bool IsInitialized { get; protected set; } = false;

    abstract public void Initialize();
    /* 상속 받을 때
    public override void Initialize()
    {
        //필수
        if(base.IsInitialized == true) return;

        //... 초기화
    }
    */

    virtual public void ResetManager() { }
}