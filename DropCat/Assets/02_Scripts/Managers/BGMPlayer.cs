using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private AudioManager audiomanager;
    [InspectorButton("OnPlayButtonClick", ButtonWidth = 300f)]
    public bool PlayCurrentBGM;
    [InspectorButton("OnPauseButtonClick", ButtonWidth = 300f)]
    public bool Pause;

    [SerializeField] private string currentBGM;

    private void Awake()
    {
        audiomanager = this.GetComponent<AudioManager>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        audiomanager.PlayBGM(currentBGM);
    }

    // 상황 별 배경음악 재생 코드 여기

    //

    private void OnPlayButtonClick()
    {
        audiomanager.PlayBGM(currentBGM);
    }

    private void OnPauseButtonClick()
    {
        audiomanager.PauseBGM();
    }
}
