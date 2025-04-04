using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class SimpleLongPressableEventTrigger : EventTrigger
{
    [SerializeField] private ILongPressButton m_Button = null;

    private bool isPressed = false;

    private readonly static float WAIT = 0.5f;
    private readonly static float INTERVAL_MIN = 0.2f;
    private readonly static float INTERVAL_MAX = 0.1f;
    private readonly static float INTERVAL_MINUS = 0.02f;

    public void InitTrigger(ILongPressButton btn)
    {
        this.m_Button = btn;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(this.isPressed) return;

        StartCoroutine("LongPress");
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        //if(this.isPressed) this.m_Button.OnLongPressEnd();

        this.isPressed = false;
        StopCoroutine("LongPress");
    }

    private IEnumerator LongPress()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(WAIT);
        this.isPressed = true;

        float fInterval = INTERVAL_MIN;
        while(this.isPressed)
        {
            //ProjectManager.Instance.Log("LongPress");
            this.m_Button.OnLongPress();
            
            yield return Utility_Time.YieldInstructionCache.WaitForSeconds(fInterval);

            if(fInterval <= INTERVAL_MAX) continue;

            fInterval -= INTERVAL_MINUS;
        }

        yield return Utility_Time.YieldInstructionCache.WaitForEndOfFrame;
    }

    private void OnDisable()
    {
        this.isPressed = false;
    }
}

public interface ILongPressButton
{
    UnityEngine.Events.UnityEvent IEventOnLongPress { get; }
    //UnityEngine.Events.UnityEvent IEventOnLongPressEnd { get; }
    //public void OnLongPress();
    //public void OnLongPressEnd();
    // ex) 클라상 레벨업을 진행함
    virtual public void OnLongPress()
    {
        if(this.IEventOnLongPress != null) this.IEventOnLongPress.Invoke();
    }

    /*

    // ex) 클라상 레벨업 진행했던 것을 한꺼번에 서버로 전송함
    virtual public void OnLongPressEnd()
    {
        if(this.IEventOnLongPressEnd != null) this.IEventOnLongPressEnd.Invoke();
    }
    */
}

#if UNITY_EDITOR
[CustomEditor(typeof(SimpleLongPressableEventTrigger)), CanEditMultipleObjects]
public class SimpleShortPressableButtonEditor : Editor
{
    //bool m_menuOpen = false;

    override public void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("이벤트 트리거 전용 스크립트입니다.");
        EditorGUILayout.LabelField("SimpleLongPressableEventTrigger가");
        EditorGUILayout.LabelField("한 오브젝트안에 존재해야 작동합니다.");

/*
        //if (m_menuOpen)
        //{
        //    if (GUILayout.Button("메뉴 닫기")) m_menuOpen = true;
        //}
        //else
        //{
        //    if (GUILayout.Button("메뉴 열기")) m_menuOpen = false;
        //}

        if((Selection.objects[0] as GameObject).GetComponent<SimpleLongPressableEventButton>() == null)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("현재 SimpleLongPressableEventTrigger가",EditorStyles.boldLabel);
            EditorGUILayout.LabelField("없습니다. 추가하시려면 버튼을 눌러주세요.", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("컴포넌트 추가하기 --->", EditorStyles.boldLabel);
            if (GUILayout.Button("추가"))
            {
                (Selection.objects[0] as GameObject).AddComponent<SimpleLongPressableEventButton>();
                EditorUtility_Time.SetDirty(Selection.objects[0]);
            }
            EditorGUILayout.EndHorizontal();
        }
*/
    }
}
#endif
