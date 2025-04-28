using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFx : MonoBehaviour
{
    [SerializeField] protected bool m_isLoop = false;
    [SerializeField] protected bool m_isPreset = false;
    protected UnityAction m_funcOnFinish = null;

    private void Start()
    {
        if(this.m_isPreset == true) this.Play();
    }

    public void Play()
    {
        this.gameObject.SetActive(true);

        StartCoroutine("coPlay");
    }

    virtual public void Play(Vector3 vecPos, UnityAction funcOnFinish = null)
    {
        this.transform.position = vecPos;
        if(funcOnFinish != null) this.m_funcOnFinish = funcOnFinish;

        this.Play();
    }

    abstract protected IEnumerator coPlay();

    public void Stop()
    {
        StopAllCoroutines();

        if(this.m_funcOnFinish != null)
        {
            this.m_funcOnFinish.Invoke();
            this.m_funcOnFinish = null;
        }

        this.gameObject.SetActive(false);
    }
}
