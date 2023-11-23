using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Spine.Unity;

public class LevelCompleteMenuManager : PauseManager
{
    [SerializeField] private Slider starSlider = null;
    [SerializeField] private TMPro.TextMeshProUGUI[] texts = null;
    [SerializeField] private Image fullStarCheckMark = null;
    private Coroutine menuCoroutine = null;
    private LevelComplete levelComplete;
    private MissionPageUpdater missionPageUpdater;
    private int playerScore = 0;
    private int fullScore = 0;
    int maxComboCount = 0;
    private bool hasCompletedDisplay = false;
    private int finalStarRecord = 1;
    LTDescr leanTweenObj = null;
    //[SerializeField] private GameObject starEffectsObj = null;
    //private SkeletonGraphic starSkeletonGraphic = null;

    private float oneStarAnimationValue = 200f;
    private float starFillAnimationTime = 2f;
    private Transform playerObj = null;
    private PlayerBasicAttackLogger playerBasicAttackLogger = null;

    public bool HasCompletedDisplay { get { return hasCompletedDisplay; } }
    private _PlayerInputActions PlayerInputActions = null;
    private InputAction menuSkipInput = null;

    public int FinalStarRecord { get { return finalStarRecord; } }

    private ObjectSpawnManager objectSpawnManager;
    private GameObject objectiveImage;
    private GameObject objectiveText;
    private TimeAttack timeAttack;

    private GameObject levelCompleteMenu;

    private TMPro.TextMeshProUGUI ObjectiveText;
    private TMPro.TextMeshProUGUI MaxComboText;
    private TMPro.TextMeshProUGUI TotalText;
    private TMPro.TextMeshProUGUI Objective;
    private TMPro.TextMeshProUGUI MaxCombo;
    private TMPro.TextMeshProUGUI Total;

    private void Awake()
    {
        InitializeCachingReference();
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        objectiveImage = GameObject.Find("ObjectiveImage");
        objectiveText = GameObject.Find("ObjectiveText");
        timeAttack = this.GetComponent<TimeAttack>();

        playerObj = GameObject.Find("[Player]").transform; 
        playerBasicAttackLogger = playerObj.GetChild(0).GetComponent<PlayerBasicAttackLogger>(); 

        levelCompleteMenu = GameObject.Find("LevelCompleteMenu");

        texts = new TMPro.TextMeshProUGUI[6];
        texts[0] = GameObject.Find("ObjectiveText").GetComponent<TMPro.TextMeshProUGUI>();
        texts[1] = GameObject.Find("MaxComboText").GetComponent<TMPro.TextMeshProUGUI>();
        texts[2] = GameObject.Find("TotalText").GetComponent<TMPro.TextMeshProUGUI>();
        texts[3] = GameObject.Find("Objective").GetComponent<TMPro.TextMeshProUGUI>();
        texts[4] = GameObject.Find("MaxCombo").GetComponent<TMPro.TextMeshProUGUI>();
        texts[5] = GameObject.Find("Total").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void InitializeCachingReference()
    {
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions; 
        menuSkipInput = PlayerInputActions.Touch.TouchAnyWhereToContinue; 
        starSlider = GameObject.Find("StarSlider").GetComponent<Slider>();
        missionPageUpdater = GameObject.Find("[PauseMenu]").GetComponent<MissionPageUpdater>(); 
        levelComplete = this.GetComponent<LevelComplete>(); 
        //starSkeletonGraphic = starEffectsObj.GetComponent<SkeletonGraphic>(); 
    }

    public void InitializeScore(float playerScore, int comboCount)
    {
        this.playerScore = (int)playerScore; 
        this.maxComboCount = comboCount; 
        
        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            this.fullScore = (int)timeAttack.TimeLimit *10/2;
        }
        else if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            if(missionPageUpdater.TotalCatHouseCount > 50)
            {
                this.fullScore = missionPageUpdater.TotalCatHouseCount*13;
            }
            else
            {
                this.fullScore = missionPageUpdater.TotalCatHouseCount*10;
            }
        }
        starSlider.maxValue = oneStarAnimationValue * 3; 
        starSlider.value = 0;  
        texts[1].text = "0"; 
        texts[2].text = "0"; 
        AllocateStars(); 
    }

    public void LevelCompleteMenuRoutine()
    {
        if (menuCoroutine == null)
        {
            menuCoroutine = StartCoroutine(ComboFillUpRoutine()); 
        }
    }

    private IEnumerator ComboFillUpRoutine()
    {
        InputAction inputAction = menuSkipInput; 
        while (!inputAction.triggered)
        {   
            if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
            {
                if (textHasNotFilledUp(texts[1].text, maxComboCount)) 
                {
                    int comboBuff = Convert.ToInt32(texts[1].text); 
                    texts[1].text = (comboBuff + 1).ToString(); 
                }
                yield return null; 
                if (inputAction.triggered || !textHasNotFilledUp(texts[1].text, maxComboCount))
                {
                    texts[0].text = missionPageUpdater.TotalCatHouseCount.ToString(); //최대 cathouse 개수
                    texts[1].text = maxComboCount.ToString(); 
                    yield return StartCoroutine(ScoreFillUpRoutine()); 
                }
            }
            else if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
            {
                texts[3].text = "제한 시간"; 
                texts[4].text = "남은 시간"; 
                objectiveImage.SetActive(false); 
                //objectiveText.transform.Translate(-170f,0f,0f); 

                if (textHasNotFilledUp(texts[1].text, maxComboCount))
                { 
                    texts[1].text = timeAttack.SelectCountdown.ToString(); 
                } 
                yield return null;  
                if (inputAction.triggered || !textHasNotFilledUp(texts[1].text, maxComboCount)) 
                { 
                    texts[0].text = timeAttack.TimeLimit.ToString(); 
                    texts[1].text = timeAttack.SelectCountdown.ToString(); 
                    yield return StartCoroutine(ScoreFillUpRoutine()); 
                } 
            }
        }
    }

    private IEnumerator ScoreFillUpRoutine()
    {
        InputAction inputAction = menuSkipInput;
        while (!inputAction.triggered)
        {
            if (textHasNotFilledUp(texts[2].text, playerScore))
            {
                int scoreBuff = Convert.ToInt32(texts[2].text);
                texts[2].text = (scoreBuff + 1).ToString();
            }
            yield return null;
            if (inputAction.triggered || !textHasNotFilledUp(texts[2].text, playerScore))
            {
                texts[2].text = playerScore.ToString();
                yield return StartCoroutine(StarFillUpRoutine());
            }
        }
    }

    private bool textHasNotFilledUp(string text, int comparison)
    {
        return Convert.ToInt32(text) < comparison;
    }

    private IEnumerator StarFillUpRoutine()
    {
        InputAction inputAction = menuSkipInput;
        while (!inputAction.triggered)
        {
            starSlider.value = oneStarAnimationValue * finalStarRecord;
            yield return null;
            if (inputAction.triggered || starSlider.value == oneStarAnimationValue * finalStarRecord)
            {
                yield return StartCoroutine(WaitForTriggerActivation());
            }
        }
    }

    private bool StarHasNotFilledUp()
    {
        return starSlider.value < oneStarAnimationValue * finalStarRecord;
    }

    private IEnumerator WaitForTriggerActivation()
    {
        InputAction inputAction = menuSkipInput;
        while (!inputAction.triggered)
        {
            yield return null;

            if (inputAction.triggered)
            {
                levelComplete.LoadNextLevel();
            }
        }
    }

    private void AllocateStars()
    {
        int starDurablity = levelComplete.BaseHouseDruability.MaxDurability/5*4;
        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            if(levelComplete.BaseHouseDruability.Durability >= starDurablity)
            {
                finalStarRecord += 1;
            }
            if(fullScore <= playerScore) 
            {
                finalStarRecord += 1;
            }
        }
        else if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            if(fullScore <= playerScore)
            {
                finalStarRecord += 2;
            }
            else if(fullScore/2 <= playerScore) 
            {
                finalStarRecord += 1;
            }
        }
    }
}
