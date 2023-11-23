using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    private VideoManager videoManager;
    private VideoPlayer videoPlayer;
    public static VideoPlayerManager instance;

    private VideoFile fileActive;
    // clipActive를 이 코드 바깥에서 Setting하는것은 권장되지 않음  
    public VideoFile GetFileActive { get { return fileActive; } }

    bool isLoopingFile = false;

    private void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoManager = this.transform.parent.gameObject.GetComponent<VideoManager>();
    }

    private void Start()
    {
        instance = this;
        videoPlayer.loopPointReached += ClearActiveFile;
    }

    private void Update()
    {
        if(SaveData.Current.optionSaveProfile.sfxStatus == GameOptionFlags.SFXStatus.SFXOff)
        {
            videoPlayer.SetDirectAudioVolume(0, 0f);
        }
        if(SaveData.Current.optionSaveProfile.sfxStatus == GameOptionFlags.SFXStatus.SFXOn)
        {
            videoPlayer.SetDirectAudioVolume(0, 100f);
        }

    }

    private void ClearActiveFile(UnityEngine.Video.VideoPlayer vp)
    {
        if (!isLoopingFile)
        {
            if (!videoPlayer.isPlaying)
            {
                fileActive = null;
            }
        }
    }

    public void ClearActiveFile()
    {
        if(videoPlayer != null)
        {
            videoPlayer.Stop();
            fileActive = null;
            videoPlayer.clip = null;
            videoPlayer.targetTexture.Release();
        }
    }

    public bool NoFileActive()
    {
        return fileActive == null;
    }

    public bool FinishedPlaying(int fileIndex)
    {
        if (fileActive == null)
            return true;
        else
            return fileActive.clip != videoManager.GetVideoFileList[fileIndex].clip;
    }

    public void PlayVideo(string customName, bool canInterrupt = false, bool loop = false)
    {
        foreach (VideoFile file in videoManager.GetVideoFileList)
        {
            if (file.customName == customName)
            {
                if (videoPlayer.isPlaying)
                {
                    if (canInterrupt)
                    {
                        videoPlayer.Stop();
                        videoPlayer.clip = file.clip;
                        if (videoPlayer.clip != null && videoPlayer.isPrepared)
                        {
                            videoPlayer.Play();
                            if (loop)
                                LoopCurrentVideo();
                            fileActive = file;
                        }
                        return;
                    }
                    else
                    {
                        Debug.Log("VideoClip " + videoPlayer.clip + "is already playing");
                        Debug.Log("Scene info :" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                        Debug.Log("Turn the 'canInterrupt' parameter on to interrupt the playing clip ");
                        return;
                    }
                }
                else
                {
                    videoPlayer.clip = file.clip;
                    if (videoPlayer.clip != null && videoPlayer.isPrepared)
                    {
                        videoPlayer.Play();
                        if (loop)
                            LoopCurrentVideo();
                        fileActive = file;
                    }
                    return;
                }
            }
            else
            {
                Debug.Log("Current Video File Custom Name Doesen't Exist");
                return;
            }
        }
    }

    public void PlayVideo(int fileIndex, bool canInterrupt = false, bool loop = false)
    {
        if (fileIndex < videoManager.GetVideoFileList.Length)
        {
            if (videoPlayer.isPlaying)
            {
                if (canInterrupt)
                {
                    if (videoPlayer.clip == fileActive.clip)
                    {
                        videoPlayer.Stop();
                        videoPlayer.clip = videoManager.GetVideoFileList[fileIndex].clip;
                        if (videoPlayer.clip != null)
                        {
                            videoPlayer.Play();
                            if (loop)
                                LoopCurrentVideo();
                            fileActive = videoManager.GetVideoFileList[fileIndex];
                        }
                        return;
                    }
                    else
                    {
                        Debug.Log("VideoClip " + videoPlayer.clip + "is already playing");
                        Debug.Log("Scene info :" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                        Debug.Log("Turn the 'canInterrupt' parameter on to interrupt the playing clip ");
                        return;
                    }
                }
            }
            else
            {
                videoPlayer.clip = videoManager.GetVideoFileList[fileIndex].clip;
                if (videoPlayer.clip != null)
                {
                    videoPlayer.Play();
                    if (loop)
                        LoopCurrentVideo();
                    fileActive = videoManager.GetVideoFileList[fileIndex];
                }
                return;
            }
        }
        else
        {
            Debug.Log("FileIndex went out of range");
            return;
        }
    }

    public void LoopCurrentVideo()
    {
        if(!videoPlayer.isLooping)
        {
            videoPlayer.isLooping = true;
            isLoopingFile = true;
        }
    }

    public double GetCurrentVideoTime()
    {
        return videoPlayer.time;
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
    }
}
