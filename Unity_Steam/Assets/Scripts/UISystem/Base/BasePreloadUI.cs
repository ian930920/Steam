using UnityEngine;
using UnityEngine.UI;

public abstract class BasePreloadUI<T> : MonoBehaviour where T : BasePreloadUI<T>
{
    [SerializeField] private CanvasScaler m_canvasScaler = null;
    public CanvasScaler CanvasScaler => this.m_canvasScaler;

    public static T Init(Transform trasParent, string strPath)
    {
		GameObject gobj = Instantiate(Resources.Load<GameObject>(strPath));
        gobj.transform.SetParent(trasParent);
        T t = gobj.GetComponent<T>();
        t.init();
        return t;
    }

    virtual protected void init() { }

    public void UpdateCanvasScaler()
    {
		UIManager.Instance.SetUIScaler(this.m_canvasScaler);
    }
}