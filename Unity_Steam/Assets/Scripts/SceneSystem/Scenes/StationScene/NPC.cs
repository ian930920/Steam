using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Transform m_transTartgetUI = null;

    private static readonly string[] STR_DESC =
    {
        "의미심장한 대사1",
        "은하철도 구구구..",
        "기차가..어둠을...",
        "... ... ...",
    };

    private void Start()
    {
        //TODO 말풍선
        StartCoroutine("coDialogue");
    }

    private IEnumerator coDialogue()
    {
        ObjectPoolManager.Instance.ActiveDialogue(STR_DESC[Random.Range(0, STR_DESC.Length)], Camera.main.WorldToScreenPoint(this.m_transTartgetUI.position));

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(5);

        StartCoroutine("coDialogue");
    }

    private void OnMouseDown()
    {
        if(UIManager.Instance.PopupSystem.IsAnyPopupOpen == true) return;

        SceneManager.Instance.GetCurrScene<StationScene>().HUD.OpenRouteSelect();
    }
}