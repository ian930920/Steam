using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textDesc = null;
    [SerializeField] private bool m_isPreset = false;

    public void Active(string strDesc, Vector3 vecPos)
    {
        this.m_textDesc.text = strDesc;

        this.transform.position = vecPos;
        this.gameObject.SetActive(true);

        if(this.m_isPreset == true) return;

        StartCoroutine("coInactive");
    }

    private IEnumerator coInactive()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(2);

        this.gameObject.SetActive(false);
    }
}