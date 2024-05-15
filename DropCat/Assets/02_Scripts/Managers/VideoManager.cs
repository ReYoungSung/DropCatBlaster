using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class VideoFile
{
    public string customName;
    public VideoClip clip;

    [Header("The Clip Format MUST be WebM")]
    [Header("Video Infos")]
    public Vector2 resolution;
    [Tooltip("Clip Length In Seconds")]
    public float length;
    public ulong frameCount;
    [TextArea]
    public string path;
}

public class VideoManager : MonoBehaviour
{
    [InspectorButton("RefreshVideoFileArray", ButtonWidth = 300f)]
    public bool RefreshVideoList;
    [SerializeField] private VideoFile[] videoFileList;
    public VideoFile[] GetVideoFileList { get { return videoFileList; } }
    [Space(20)]
    [SerializeField] private VideoPlayerManager[] videoPlayerManagerList;
    public VideoPlayerManager[] VideoPlayerManagerList { get { return videoPlayerManagerList; } }
    [Space(20)]
    [SerializeField] private RenderTexture[] videoLayerTextures;
    [SerializeField] private GameObject[] videoLayerObjs = null;

    public void InitializeVideoLayerObjs(GameObject levelLoaderObj, bool isInMenuScene)
    {
        videoLayerObjs = new GameObject[2];
        videoLayerObjs[0] = levelLoaderObj.transform.GetChild(0).GetChild(0).gameObject;
        videoLayerObjs[1] = levelLoaderObj.transform.GetChild(0).GetChild(1).gameObject;
    }

    public void RefreshVideoFileArray()
    {
        foreach (VideoFile file in videoFileList)
            if (file.clip != null)
            {
                file.resolution = new Vector2(file.clip.width, file.clip.height);
                file.length = Mathf.Floor((float)file.clip.length * 100f) * 0.01f;
                file.frameCount = file.clip.frameCount;
                file.path = file.clip.originalPath;
            }
    }

    public void ResetVideoLayers()
    {
        foreach(RenderTexture videoLayer in videoLayerTextures)
        {
            videoLayer.Release();
        }
        RefreshVideoFileArray();
    }

    public VideoPlayerManager GetVideoPlayerManager(int layerIndex)
    {
        return videoPlayerManagerList[layerIndex];
    }

    public void ToggleVideoLayerGameObject(int index, bool toggleVal)
    {
        if(VideoLayerGameObjectEnabled(index) != toggleVal)
        {
            videoLayerObjs[index].SetActive(toggleVal);
        }
    }

    public bool VideoLayerGameObjectEnabled(int index)
    {
        return videoLayerObjs[index].activeSelf;
    }

    public void ToggleVideoLayer(int index, bool toggleVal)
    {
        if(VideoLayerEnabled(index) != toggleVal)
            videoLayerObjs[index].GetComponent<RawImage>().enabled = toggleVal;
    }

    public bool VideoLayerEnabled(int index)
    {
        return videoLayerObjs[index].GetComponent<RawImage>().enabled;
    }
}
