using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(SimpleLongPressableEventTrigger))]
public class SimpleLongPressableEventButton : MonoBehaviour
{
    public enum ButtonType { None, State }
    public ButtonType buttonType = ButtonType.None;

    [Space(10)]
    [SerializeField] private UnityEvent m_eventOnClicked = null;
    [SerializeField] private UnityEvent m_eventOnLongPress = null;
    [SerializeField] private UnityEvent m_eventOnLongPressEnd = null;


    // 리프레시 관련
    [HideInInspector][SerializeField] public Image m_imgButton = null;
    [HideInInspector][SerializeField] public Color m_colorActive = new Color(0.227451f, 0.627451f, 1);
    [HideInInspector][SerializeField] public Color m_colorInactive = new Color(0.6705883f, 0.6705883f, 0.6705883f);

    [HideInInspector][SerializeField] public Text m_textText = null;
    [HideInInspector][SerializeField] public Color m_colorTextActive = Color.white;
    [HideInInspector][SerializeField] public Color m_colorTextInactive = Color.white;

    [Space(10)]
    [Header("────────────────────────────────────────────────")]
    [Tooltip("롱프레스가 시작되기 위해 필요한시간 (시간 이전에 Up하면 Click)")]
    //[SerializeField] 
    private float m_fLongPressDelay = 0.3f;
    public float LongPressDealy => this.m_fLongPressDelay;
    [Tooltip("롱프레스 중 함수를 호출하는 시간 간격")]
    private float m_fLongPressInterval = 0.1f;
    public float LongPressInterval => this.m_fLongPressInterval;

    private UIManager.eUI_BUTTON_STATE m_eState = UIManager.eUI_BUTTON_STATE.Active;
    public UIManager.eUI_BUTTON_STATE ButtonState
    {
        get => this.m_eState;
        set
        {
            this.m_eState = value;

            switch (this.m_eState)
            {
                case UIManager.eUI_BUTTON_STATE.Active:
                    {
                        this.m_imgButton.color = this.m_colorActive;
                        this.m_textText.color = this.m_colorTextActive;
                    }
                    break;
                case UIManager.eUI_BUTTON_STATE.Inactive:
                    {
                        this.m_imgButton.color = this.m_colorInactive;
                        this.m_textText.color = this.m_colorTextInactive;
                    }
                    break;
            }

            bool bActive = this.m_eState == UIManager.eUI_BUTTON_STATE.Active;
        }
    }

    virtual public void OnClicked()
    {
        this.m_eventOnClicked.Invoke();
    }

    // ex) 클라상 레벨업을 진행함
    virtual public void OnLongPress()
    {
        this.m_eventOnLongPress.Invoke();
    }

    // ex) 클라상 레벨업 진행했던 것을 한꺼번에 서버로 전송함
    virtual public void OnLongPressEnd()
    {
        this.m_eventOnLongPressEnd.Invoke();
    }

    public void SetGameObjectActive(bool bValue)
    {
        this.m_eventOnLongPressEnd.Invoke();

        this.gameObject.SetActive(bValue);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SimpleLongPressableEventButton))]
public class SimpleLongPressableEventButtonEditor : Editor
{
    SimpleLongPressableEventButton selected = null;

    private void OnEnable()
    {
        if (Selection.objects.Length != 0)
        {
            selected = (Selection.objects[0] as GameObject).GetComponent<SimpleLongPressableEventButton>();
        }
        else
        {
            selected = null;
        }
    }

    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("────────────────────────────────────────────────", EditorStyles.boldLabel);
        //EditorGUILayout.LabelField("기본 롱프레스 버튼 설정", EditorStyles.boldLabel);

        base.OnInspectorGUI();

        if (selected.buttonType == SimpleLongPressableEventButton.ButtonType.None) return;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("────────────────────────────────────────────────", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("상태 롱프레스 버튼 설정", EditorStyles.boldLabel);

        selected.m_imgButton = EditorGUILayout.ObjectField("버튼 이미지", selected.m_imgButton, typeof(Image), true) as Image;
        //EditorUIManager.ObjectConnectionHelp(selected.gameObject, ref selected.m_imgButton, "버튼 이미지");
        selected.m_colorActive = EditorGUILayout.ColorField("버튼 이미지 활성화 색", selected.m_colorActive);
        selected.m_colorInactive = EditorGUILayout.ColorField("버튼 이미지 비활성화 색", selected.m_colorInactive);
        EditorGUILayout.Space(5);
        selected.m_textText = EditorGUILayout.ObjectField("버튼 텍스트", selected.m_textText, typeof(Text), true) as Text;
        //EditorUIManager.ObjectConnectionHelp(selected.gameObject, ref selected.m_textText, "버튼 텍스트");
        selected.m_colorTextActive = EditorGUILayout.ColorField("버튼 텍스트 활성화 색", selected.m_colorTextActive);
        selected.m_colorTextInactive = EditorGUILayout.ColorField("버튼 텍스트 비활성화 색", selected.m_colorTextInactive);
        EditorGUILayout.Space(5);
        //selected.UiEffect = EditorGUILayout.ObjectField("레벨업 가능 이펙트", selected.UiEffect, typeof(UI_CanvasGroup), true) as UI_CanvasGroup;
        //EditorUIManager.ObjectConnectionHelp(selected.gameObject, ref selected.UiEffect, "UI 이펙트");

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("");
        if (GUILayout.Button("저장 버튼"))
        {
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif