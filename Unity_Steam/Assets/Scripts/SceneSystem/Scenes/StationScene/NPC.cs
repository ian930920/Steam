using Febucci.UI;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Transform m_transTartgetUI = null;
    [SerializeField] private TypewriterByCharacter m_textDesc = null;

    private void Start()
    {
        StartCoroutine("coAnimDesc");
    }

    private IEnumerator coAnimDesc()
    {
        this.m_textDesc.ShowText("무엇을 도와드릴까요?");

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(8.0f);

        StartCoroutine("coAnimDesc");
    }

    private void OnMouseDown()
    {
        if(UIManager.Instance.PopupSystem.IsAnyPopupOpen == true) return;

        SceneManager.Instance.GetCurrScene<StationScene>().HUD.OpenRouteSelect();
    }
}