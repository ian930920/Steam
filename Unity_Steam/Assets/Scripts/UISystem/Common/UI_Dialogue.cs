using Febucci.UI;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] private TypewriterByCharacter m_textDesc = null;
    [SerializeField] private bool m_isPreset = false;

    public void Active(string strDesc, Vector3 vecPos)
    {
        this.transform.position = vecPos;
        this.gameObject.SetActive(true);

        this.m_textDesc.ShowText(strDesc);

        if(this.m_isPreset == true) return;

        StartCoroutine("coInactive");
    }

    private IEnumerator coInactive()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(2);

        this.gameObject.SetActive(false);
    }
}