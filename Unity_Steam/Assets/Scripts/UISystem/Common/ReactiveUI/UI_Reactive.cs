using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_Reactive : MonoBehaviour
{
    [SerializeField] private RectTransform m_rectTrans = null;
    private Transform m_transTarget = null;
    public bool IsActive { get => this.gameObject.activeSelf; }

    public void ActiveUI(Transform transTarget, UI_ReactiveGroup.eMODE eMode = UI_ReactiveGroup.eMODE.Default)
    {
        if(this.gameObject.activeSelf == true) ProjectManager.Instance.Log("UI_Reactive 이미 활성화 중인디?");

        this.gameObject.SetActive(true);

        if(transTarget != null)
        {
            this.m_transTarget = transTarget;

            StartCoroutine("coTarget");
        }

        //TODO
        //this.transform.SetParent(UIManager.Instance.HUD?.ReactiveGroup.GetTransformByMode(eMode));
    }

    virtual public void InactiveUI()
    {
        if(this.IsActive == false) return;

        //초기화
        StopAllCoroutines();
        this.m_transTarget = null;
        this.gameObject.SetActive(false);
    }

    private IEnumerator coTarget()
    {   
        while(true)
        {
            this.m_rectTrans.position = Camera.main.WorldToScreenPoint(this.m_transTarget.position);

            yield return null;
        }
    }
}
