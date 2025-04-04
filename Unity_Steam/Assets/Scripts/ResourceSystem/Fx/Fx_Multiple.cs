using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_Multiple : BaseFx
{
    [SerializeField] private Animation m_anim = null;
    [SerializeField] private AnimationClip m_animClip = null;
    [SerializeField] private ParticleSystem m_particle = null;

    protected override IEnumerator coPlay()
    {
        this.m_anim.Play();
        this.m_particle.Play();

        yield return Utility_Time.YieldInstructionCache.WaitForSeconds(this.m_animClip.length);

        this.gameObject.SetActive(false);
    }
}
