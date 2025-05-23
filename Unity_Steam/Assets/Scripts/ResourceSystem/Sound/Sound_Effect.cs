using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Effect : MonoBehaviour
{
    [SerializeField] private BaseSound.eID m_eSoundID = BaseSound.eID.Btn_Click;
    
    public void PlaySound()
    {
        ObjectPoolManager.Instance.PlayEffectSound(this.m_eSoundID);
    }
}