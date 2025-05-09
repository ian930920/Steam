using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class BaseButton : MonoBehaviour
{
    public enum eADD_LISTENER_TYPE
    {
        Set,
        Add
    }

    private Button m_btn = null;

    [SerializeField] protected UnityEvent m_eventOnClicked = null;

    protected Color BtnColor { set => this.m_btn.image.color = value; }

    public bool IsInit { get; private set; } = false;

    virtual public void InitButton()
    {
        if(this.IsInit == true) return;

        //버튼 이벤트 물려주기
        if(this.m_btn == null) this.m_btn = this.GetComponent<Button>();
        this.m_btn.onClick.RemoveAllListeners();
        this.m_btn.onClick.AddListener(this.OnClicked);

        //초기화 완료
        this.IsInit = true;
    }

    virtual public void OnClicked()
    {
        if(this.m_btn == null) return;

        if(this.m_eventOnClicked == null) return;

        //소리 재생
        ObjectPoolManager.Instance.PlayEffectSound(BaseSound.eID.Btn_Click);

        this.m_eventOnClicked.Invoke();
    }

    protected void changeBtnSprite(Sprite sprite)
    {
        this.m_btn.image.sprite = sprite;
        this.m_btn.image.SetNativeSize();
    }

    public void AddOnClickEvent(eADD_LISTENER_TYPE eAddType, UnityAction action)
    {
        if(eAddType == eADD_LISTENER_TYPE.Set) this.m_btn.onClick.RemoveAllListeners();

        this.m_eventOnClicked.AddListener(action);
    }
}