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
    private Animator m_animator = null;

    [SerializeField] protected UnityEvent m_eventOnClicked = null;

    protected Color BtnColor { set => this.m_btn.image.color = value; }

    public bool IsInit { get; private set; } = false;

    virtual public void InitButton()
    {
        if(this.IsInit == true) return;

        //버튼 트랜지션 모드 변경
        if(this.m_btn == null) this.m_btn = this.GetComponent<Button>();
        this.m_btn.transition = Selectable.Transition.Animation; 

        //버튼 애니 물려주기
        if(this.m_animator == null) this.m_animator = this.GetComponent<Animator>();
        this.m_animator.runtimeAnimatorController = ProjectManager.Instance.Resource.Animator_Button;

        //버튼 이벤트 물려주기
        //이벤트 물려주기
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
        ProjectManager.Instance.ObjectPool.PlayEffectSound(BaseSound.eID.Btn_Click);

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