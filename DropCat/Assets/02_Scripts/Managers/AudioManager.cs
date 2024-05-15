using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
        public string name;
        public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;
    [SerializeField] bool loopBGM = true;
    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer;

    [SerializeField] private AudioSource loopingSource = null;
    private bool sFXConditionalPlaying = false;

    private Sound currentSFX, currentBGM;
    public Sound GetCurrentSFX { get { return currentSFX; } }
    public Sound GetCurrentBGM { get { return currentBGM; } }

    private float volumeSFX = 128f;
    public float VolumeSFX { get { return volumeSFX; } set { volumeSFX = value; } }
    private float volumeBGM = 0.094f;
    private bool bGMPlaying = false;
    private bool bGMPaused = false;

    private void Start()
    {
        LeanTween.init(800);
        instance = this;
    }

    private void Update()
    {
        if (!bgmPlayer.isPlaying && bGMPlaying)
        {
            if (SaveData.Current.optionSaveProfile.bgmStatus == GameOptionFlags.BGMStatus.BGMOn)
            {
                if (loopBGM)
                {
                    bgmPlayer.Play();
                }
            }
        }
        if (SaveData.Current.optionSaveProfile.bgmStatus == GameOptionFlags.BGMStatus.BGMOff)
        {
            PauseBGM();
        }
        if(!bGMPlaying && !bGMPaused)
        {
            if (SaveData.Current.optionSaveProfile.bgmStatus == GameOptionFlags.BGMStatus.BGMOn)
            {
                ResumeBGM();
            }
        }
    }

    public void PlayBGM(string bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                currentBGM = bgm[i];
                bgmPlayer.Play();
                bGMPlaying = true;
            }
        }
    }

    public void ResumeBGM()
    {
        bgmPlayer.UnPause();
        bGMPlaying = true;
        bGMPaused = false;
    }

    public void PauseBGM()
    {
        if(bgmPlayer!= null)
        {
            bgmPlayer.Pause();
            bGMPlaying = false;
            bGMPaused = true;
        }
    }

    public void StopBGM()
    {
        if (bgmPlayer != null)
        {
            bgmPlayer.Stop();
            bGMPlaying = false;
        }
        currentBGM = null;
    }

    public void StopBGMGradually()
    {
        if (bgmPlayer != null)
        {
            float vol = volumeBGM;
            if(0 < vol)
            {
                LeanTween.value(vol, 0, 0.01f).setEaseInCirc().setIgnoreTimeScale(true);
                bgmPlayer.volume = vol;
            }
        }
    }

    public void PlaySFX(string sfxName)
    {
        if (SaveData.Current.optionSaveProfile.sfxStatus == GameOptionFlags.SFXStatus.SFXOn)
        {
            foreach (Sound playerSfx in sfx)
            {
                if (sfxName == playerSfx.name)
                {
                    for (int i = 0; i < sfxPlayer.Length; i++)
                    {
                        if (!sfxPlayer[i].isPlaying)
                        {
                            sfxPlayer[i].clip = playerSfx.clip;
                            currentSFX = playerSfx;
                            sfxPlayer[i].Play();
                            return;
                        }
                    }
                    return;
                }
            }
            return;
        }
    }

    public void PlaySFX(string sfxName, float volume)
    {
        if (SaveData.Current.optionSaveProfile.sfxStatus == GameOptionFlags.SFXStatus.SFXOn)
        {
            foreach (Sound playerSfx in sfx)
            {
                if (sfxName == playerSfx.name)
                {
                    for (int i = 0; i < sfxPlayer.Length; i++)
                    {
                        if (!sfxPlayer[i].isPlaying)
                        {
                            sfxPlayer[i].clip = playerSfx.clip;
                            currentSFX = playerSfx;
                            sfxPlayer[i].volume = volume;
                            sfxPlayer[i].Play();
                            return;
                        }
                    }
                    return;
                }
            }
            return;
        }
    }

    public void PlaySFXLoop(string sfxName)
    {
        if (SaveData.Current.optionSaveProfile.sfxStatus == GameOptionFlags.SFXStatus.SFXOn)
        {
            foreach (Sound playerSfx in sfx)
            {
                if (sfxName == playerSfx.name)
                {
                    if (!loopingSource.isPlaying)
                    {
                        loopingSource.clip = playerSfx.clip;
                        loopingSource.loop = true;
                        loopingSource.Play();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            return;
        }
    }

    public void StopLoopingSFX()
    {
        if(loopingSource != null)
            loopingSource.Stop();
    }

    public void PlayAudioInCondition(string sfxName, bool condition)
    {
        if (condition)
        {
            foreach (Sound playerSfx in sfx)
            {
                if (sfxName == playerSfx.name)
                {
                    for (int i = 0; i < sfxPlayer.Length; i++)
                    {
                        if (!sfxPlayer[i].isPlaying)
                        {
                            sfxPlayer[i].clip = playerSfx.clip;
                            break;
                        }
                    }
                    return;
                }
            }
            if (!loopingSource.isPlaying)
            {
                loopingSource.Play(0);
            }
        }
        else
            loopingSource.Stop();
    }

    public void SetVolumeSFX(float Volume)
    {
        foreach(AudioSource audioSource in sfxPlayer)
        {
            audioSource.volume = Volume;
        }
    }
}
