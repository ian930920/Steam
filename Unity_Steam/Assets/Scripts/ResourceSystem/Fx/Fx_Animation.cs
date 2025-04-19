using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_Animation : BaseFx
{
    [SerializeField] private Animation m_anim = null;
    [SerializeField] private AnimationClip m_animClip = null;

    override protected IEnumerator coPlay()
    {
        this.m_anim.Play();

        if(base.m_isLoop == false)
        {
            yield return Utility_Time.YieldInstructionCache.WaitForSeconds(this.m_animClip.length);

            base.Stop();
        }
    }
}