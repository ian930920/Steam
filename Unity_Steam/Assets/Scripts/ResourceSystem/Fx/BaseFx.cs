using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFx : MonoBehaviour
{
    [SerializeField] protected bool m_isLoop = false;
    [SerializeField] protected bool m_isPreset = false;

    private void Start()
    {
        if(this.m_isPreset == true) this.Play();
    }

    public void Play()
    {
        this.gameObject.SetActive(true);

        StartCoroutine("coPlay");
    }

    virtual public void Play(Vector3 vecPos)
    {
        this.transform.position = vecPos;

        this.Play();
    }

    abstract protected IEnumerator coPlay();

    public void Stop()
    {
        StopAllCoroutines();

        this.gameObject.SetActive(false);
    }
}
