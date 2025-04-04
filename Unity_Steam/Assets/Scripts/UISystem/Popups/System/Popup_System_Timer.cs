using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Popup_System_Timer : Popup_System
{
    private static readonly float TIME = 1.5f;

    public override BasePopup OpenPopup(int nOreder, UnityAction funcClose)
    {
        base.OpenPopup(nOreder, funcClose);

        ProjectManager.Instance.ObjectPool.PlayEffectSound(BaseSound.eID.Error);

        StopAllCoroutines();
        StartCoroutine("coTimer");

        return this;
    }

    private IEnumerator coTimer()
    {
        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(TIME);

        base.closePopup();
    }
}
