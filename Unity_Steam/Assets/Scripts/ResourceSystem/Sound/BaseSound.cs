using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BaseSound : MonoBehaviour
{
    public enum eID
    {
        Btn_Click = 10001,
        Popup_Open,
        Order,
        Error,
        ItemDrop,
        ItemDrop_Special,
        Reward,
        Buy,
        Upgrade,
        Success,
        Fail,
        NextPage,
        Typing,
        Buff,
        Atteck,
        Event,
        Mission,
        ItemBox_Open,
        Bboyrorong,

        BGM = 11001,
    }

    public enum eTYPE
    {
        BGM,
        Effect,
    }

    public readonly static float BGM_VOLUME = 0.4f;

    [SerializeField] private AudioSource m_audioSource = null;
    public bool IsPlaying { get; private set; } = false;

    public void Play(eID eSoundID, eTYPE eType, float fVolume)
    {
        this.Play(ResourceManager.Instance.GetAudioClip((uint)eSoundID), eType, fVolume);
    }

    public void Play(AudioClip clip, eTYPE eType, float fVolume)
    {
        if(this.IsPlaying == true) return;

        this.gameObject.SetActive(true);
        this.IsPlaying = true;

        this.m_audioSource.clip = clip;
        this.SetVolume(eType, fVolume);
        this.m_audioSource.loop = eType == eTYPE.BGM;
        this.m_audioSource.Play();
        if(eType == eTYPE.Effect) Invoke("inactiveGameObject", clip.length);
    }

    public void Stop()
    {
        this.m_audioSource.Stop();
        this.IsPlaying = false;
    }

    public void SetVolume(eTYPE eType, float fVolume)
    {
        switch(eType)
        {
            case eTYPE.BGM:
            {
                //TODO UserDataManager
                //this.m_audioSource.volume = UserDataManager.Instance.Setting.SoundBGM.Volume * fVolume;
            }
            break;
            case eTYPE.Effect:
            {
                //TODO UserDataManager
                //this.m_audioSource.volume = UserDataManager.Instance.Setting.SoundEffect.Volume * fVolume;
            }
            break;
        }
    }

    private void inactiveGameObject()
    {
        this.gameObject.SetActive(false);
        this.IsPlaying = false;
    }
}