using UnityEngine;
using UnityEditor;


public class PlayerPrefsEditorUtility : MonoBehaviour
{
#if(UNITY_EDITOR)
    [MenuItem("PlayerPrefs/Delete All")]
    static void DeletePlayerPrefs()
    {
        SecurityPlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs deleted");
    }

    [MenuItem("PlayerPrefs/Save All")]
    static void SavePlayerPrefs()
    {
        SecurityPlayerPrefs.Save();
        Debug.Log("PlayerPrefs saved");
    }
#endif

    //여기부터 내가 쓰는거
#if(UNITY_EDITOR)
    [MenuItem("PlayerPrefs/Cheat")]
    static void Cheat()
    {
    }
#endif
}
