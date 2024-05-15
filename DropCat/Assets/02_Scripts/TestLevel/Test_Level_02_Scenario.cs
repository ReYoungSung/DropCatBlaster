using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Test_Level_02_Scenario : MonoBehaviour
{
    private Chapter chapterLevel_01 = null;
    private _PlayerInputActions playerInput = null;

    [SerializeField] private GameObject virtualCamera = null;
    private AudioManager audioManager = null;
    private Timer timer = null;

    private ObjectSpawnManager objectSpawnManager = null;
    private GameOver gameOverChapter = null;
    [SerializeField] private Transform player = null;
    [SerializeField] private Transform sky = null;
    [SerializeField] private GameObject houseUI = null;

    private void Awake()
    {
        chapterLevel_01 = new Chapter();
        playerInput = new _PlayerInputActions();

        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        timer = this.GetComponent<Timer>();
        gameOverChapter = this.GetComponent<GameOver>();
    }

    private void Start()
    {
        audioManager.PlayBGM("Combat");
        LoadSequence_Test_Level_01();
        //LoadSequence_Level01();
        playerInput.UIs.ContinueDialogue.performed += ctx => chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
        houseUI.SetActive(true);
        GameOverEventManager.current.onGameOverTriggerEnter += gameOverChapter.GetGameOverChapter.StartChapter;
    }

    private void Update()
    {
        Debug.Log(chapterLevel_01.GetCurrentCount);
        if (chapterLevel_01.CurrentStatus == Chapter.ChapterStatus.isLoading)
        {
            chapterLevel_01.StartChapter();
        }
    }

    private void LoadSequence_Test_Level_01()
    {
        Sequence SequenceLevel01 = new Sequence("Test_Level_02_Scenario");
        Beat level01Beat = new Beat();
        level01Beat.AddEventsToBeatEnter(
                   delegate
                   {
                       audioManager.PlayBGM("Combat");
                   }
                   );
        level01Beat.AddEventsToBeatRepeat(objectSpawnManager.onSpawningTriggerEnter);
        SequenceLevel01.AddBeatToCurrent(level01Beat);
        chapterLevel_01.AddSequenceToCurrent(SequenceLevel01);
    }

    /*
    private void Load_PopUpDialogue(int loop)
    {
        for (int i = 0; i < loop; i++)
        {
            PopUpMessage beat_01 = new PopUpMessage();
            string dialogue = tutorialDialogue.Dequeue();
            if (i < 1)
            {
                beat_01.AddEventsToBeatEnter(popUpMessageBehaviour.PopsUp);
                beat_01.AddEventsToBeat(audioManager.PlaySFX, "DialoguePopsUp");
            }
            beat_01.AddEventsToBeat(popUpMessageBehaviour.SwitchContents, dialogue);
            sequence_01.AddBeatToCurrent(beat_01);
        }
    }

    private void Load_Tutorial(int loop)
    {
        for (int i = 0; i < loop; i++)
        {
            PopUpMessage beat_01 = new PopUpMessage();
            IntervalBreak intervalBreak = new IntervalBreak();
            string dialogue = tutorialDialogue.Dequeue();
            if (i == 4)
            {
                beat_01.AddEventsToBeat(delegate { catHousesForTutorial[2] = objectSpawnManager.SpawnObject(1, 6); });
                beat_01.AddEventsToBeat(SetCameraTargetToCatHouse);
            }
            else if(i == 9)
            {
                beat_01.AddEventsToBeat(SetCameraTargetToPlayer);
            }
            else if(i == 10)
            {
                beat_01.AddEventsToBeat(delegate { catHousesForTutorial[0] = objectSpawnManager.SpawnObject(1, 1); });
            }
            else if (i == 11)
            {
                beat_01.AddEventsToBeat(delegate { catHousesForTutorial[1] = objectSpawnManager.SpawnObject(1, new Vector2(39f, 59f)); });
                beat_01.AddEventsToBeat(delegate { DisableFalling(ref catHousesForTutorial[1]); });
                beat_01.AddEventsToBeat(delegate { timer_2ND.StartTicking(intervalBreakTime); });
            }
            beat_01.AddEventsToBeat(popUpMessageBehaviour.SwitchContents, dialogue);
            sequence_01.AddBeatToCurrent(beat_01);
        }
    }
    */

    private void OnEnable()
    {
        playerInput.Enable();
    }
}
