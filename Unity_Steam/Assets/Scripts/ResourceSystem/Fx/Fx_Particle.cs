using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_Particle : BaseFx
{
    [SerializeField] private ParticleSystem m_particle = null;

    override protected IEnumerator coPlay()
    {
        this.m_particle.Play();

        if(base.m_isLoop == false)
        {
            if(base.m_isPreset == true) yield return Utility_Time.YieldInstructionCache.WaitForSeconds(this.m_particle.main.duration + this.m_particle.main.startLifetime.constantMax);
            else yield return Utility_Time.YieldInstructionCache.WaitForSeconds(1);

            base.Stop();
        }
    }
}