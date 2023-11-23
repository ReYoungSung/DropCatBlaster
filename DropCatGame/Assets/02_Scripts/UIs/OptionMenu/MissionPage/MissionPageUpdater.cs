using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionPageUpdater : MonoBehaviour
{
    [SerializeField] private Sprite[] missionImages = null;
    [SerializeField] private Image missionSlotImage = null;
    [SerializeField] private GameObject missionText_Goal = null;
    private TextMeshProUGUI missionText_Goal_TextMesh = null;
    [SerializeField] private TextMeshProUGUI dayText = null;
    private int numberOfCatHouses = 0;
    private int objectiveCache = 0;
    private int locking = 0;
    private int totalCatHouseCount = 0;
    public int TotalCatHouseCount { get { return totalCatHouseCount; } set {  totalCatHouseCount = value; } }
    private ObjectSpawnManager objectSpawnManager = null;
    private TimeAttack timeAttackManager = null;

    private void Awake()
    {
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        timeAttackManager = GameObject.Find("[SceneEventManager]").GetComponent<TimeAttack>();
        missionText_Goal_TextMesh = missionText_Goal.GetComponent<TextMeshProUGUI>();
        dayText = dayText.gameObject.GetComponent<TextMeshProUGUI>();
        missionSlotImage = missionSlotImage.gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        if (objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            dayText.text = "Day " + (SceneManager.GetActiveScene().buildIndex-2).ToString();
            missionSlotImage.sprite = missionImages[0];
        }
        else if (objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            dayText.text = "Midnight";
            missionSlotImage.sprite = missionImages[1];
        }
    }

        private void FixedUpdate()
    {
        if(objectSpawnManager.CatHouseCount != 0 && locking == 0)
        {
            totalCatHouseCount = objectSpawnManager.CatHouseCount; //최대 cathouse 갯수
            locking = 1;
        }
    }

    private void Update()
    {
        numberOfCatHouses = objectSpawnManager.CatHouseCount;
        if(objectiveCache <= numberOfCatHouses && numberOfCatHouses != objectSpawnManager.GET_LEVELCOMPLETEFLAG)
        {
            objectiveCache = numberOfCatHouses;
        }

        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            if (numberOfCatHouses == objectSpawnManager.GET_LEVELCOMPLETEFLAG)
            {
                missionText_Goal_TextMesh.text = "목표 : " + objectiveCache.ToString();
            }
            else
            {
                missionText_Goal_TextMesh.text = "남은 고양이 수 : " + numberOfCatHouses.ToString();
            }
        }
        else if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            if(0 < objectSpawnManager.StartDelay)
            {
                missionText_Goal_TextMesh.text = "남은 시간 : " + timeAttackManager.SelectCountdown.ToString();
            }
            else
            {
                missionText_Goal_TextMesh.text = "Times Up";
            }
        }
    }
}
