using UnityEngine;

public abstract class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Init(GameObject gobjManager)
    {
        T t = gobjManager.AddComponent<T>();
        (t as BaseManager<T>).init();
        return t;
    }

    abstract protected void init();
    virtual public void ResetManager() { }
}