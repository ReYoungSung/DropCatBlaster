using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using CharacterBehaviour.Attack;

public class ObjectSpawnManager : MonoBehaviour
{
    private SwitchPosition switchPosition = null;
    [SerializeField] private float spawningInterval = 0f;

    [SerializeField] private GameObject[] enemyArr = null;

    private bool spawningInProgress = false;
    public bool SpawningInProgress { get { return spawningInProgress; } set { spawningInProgress = value; } }

    public UnityAction onSpawningTriggerEnter;
    public UnityAction onSpawningTriggerExit;

    private Coroutine enemySpawningRoutine = null;
    private EnemyType spawningEnemy = EnemyType.None;

    private SpawningData spawningData = null;
    private Stack rowStack = new Stack();

    private int catHouseCount = 0;
    public int CatHouseCount { get { return catHouseCount; } set {  catHouseCount = value; } }
    private int spawnedCatHouse = 0;
    public int SpawnedCatHouse { get { return spawnedCatHouse; } }
    private GameObject motherSpaceship = null;
    private float spawningPositionY = 0;

    private const int LEVELCOMPLETEFLAG = 9876;
    public int GET_LEVELCOMPLETEFLAG { get { return LEVELCOMPLETEFLAG; } }
    [SerializeField] private LevelTemplate levelData = null;
    public LevelTemplate LevelData { get { return levelData; } }
    private AudioManager audioManager = null;
    private bool firstRow = true;
    [SerializeField] private float startDelay = 0.0f;

    private GameObject gamePlayCanvas;
    private GameObject houseDurability;

    private bool levelFinished = false;
    public bool LevelFinished { get { return levelFinished; } }
    [SerializeField] private ModeName modeName = ModeName.CatHouseFalling;
    public ModeName GetModeName { get { return modeName; } }
    public float StartDelay { get { return startDelay; } }

    public enum ModeName
    {
        CatHouseFalling,
        DjClimbing,
        Boss
    }

    public void SpawningTriggerEnter()
    {
        if(onSpawningTriggerEnter != null)
        {
            if(!spawningInProgress)
            {
                onSpawningTriggerEnter();
                spawningInProgress = true;
            }
        }
    }

    public void SpawningTriggerExit()
    {
        if(onSpawningTriggerExit != null)
        {
            if(spawningInProgress)
            {
                onSpawningTriggerExit();
                spawningInProgress = false;
            }
        }
    }

    private void Awake()
    {
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();

        if(modeName == ModeName.CatHouseFalling)
        {
            motherSpaceship = GameObject.Find("[Enemy] CatSpaceship_Normal");
        }
        else if(modeName == ModeName.DjClimbing)
        {
            motherSpaceship = GameObject.Find("[Enemy] CatSpaceship_Laser");
        }
        spawningPositionY = motherSpaceship.transform.position.y - 30;

        gamePlayCanvas = GameObject.Find("[UI] GamePlayCanvas");
        houseDurability = gamePlayCanvas.transform.GetChild(0).gameObject;

        switchPosition = this.GetComponent<SwitchPosition>();
        onSpawningTriggerEnter += StartSpawnRoutine;
        onSpawningTriggerExit += StopSpawnRoutine;
        /*
        if (0 < levelData.levelNumber)
            spawningData = new SpawningData(levelData.levelNumber);
        else
            spawningData = new SpawningData();
        */
        spawningData = new SpawningData(levelData.levelName); 
    }

    public void SpawnSingleRow(ref List<GameObject> spawnedObj)
    {
        SpawnRow((Row)rowStack.Pop());
        GameObject catHouseObj = GameObject.FindGameObjectWithTag("ENEMY_CatHouse");
        spawnedObj.Add(catHouseObj);
    }

    private void Start()
    {
        spawningData.StackColumns(ref rowStack);
 
        catHouseCount = spawningData.CatHouse;
        if(modeName == ModeName.DjClimbing)
        {
            houseDurability.transform.GetChild(2).gameObject.SetActive(false);
            houseDurability.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        levelFinished = false;
        if(catHouseCount == 1)
        {
            audioManager.PlaySFX("EnemyAlarm");
        }
        if(modeName == ModeName.CatHouseFalling)
        {
            if(catHouseCount <= 0 && 0 < spawnedCatHouse)
            {
                levelFinished = true;
            }
        }
        spawningPositionY = motherSpaceship.transform.position.y - 30;
    }

    private void StartSpawnRoutine()
    {
        if (enemySpawningRoutine == null)
        {
            enemySpawningRoutine = StartCoroutine(SpawnRoutine(spawningInterval));
        }
        else
            return;
    }

    private void StopSpawnRoutine()
    {
        if(enemySpawningRoutine != null)
            StopCoroutine(enemySpawningRoutine);
        enemySpawningRoutine = null;
    }

    private IEnumerator SpawnRoutine(float rowInterval)
    {
        foreach (Row row in rowStack)
        {
            if(firstRow == true)
            {
                yield return new WaitForSeconds(startDelay);
                firstRow = false;
            }
            SpawnRow(row);
            yield return new WaitForSeconds(rowInterval);
        }
    }

    private void SpawnRow(Row r)
    {
        for (int i = 0; i < 8; i++)
        {
            if (CellIsNotEmpty(r.Data[i]))
            {
                SpawnObject(enemyArr[Convert.ToInt32(spawningEnemy)], i);
                ++spawnedCatHouse;
            }
        }
    }
    
    private bool CellIsNotEmpty(string cell)
    {
        if (cell != "0")
        {
            IdentifyEnemyType(cell, out spawningEnemy);
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject SpawnObject(int spawningObjIndex, int columnIndex)
    {
        Vector2 spawningPosition = switchPosition.SwitchColumnX(columnIndex,spawningPositionY);
        return Instantiate(enemyArr[spawningObjIndex], spawningPosition, Quaternion.identity);
    }

    public void SpawnObject(GameObject spawningObj, int columnIndex)
    {
        Vector2 spawningPosition = switchPosition.SwitchColumnX(columnIndex,spawningPositionY);
        Instantiate(spawningObj, spawningPosition, Quaternion.identity);
    }

    public GameObject SpawnObject(int spawningObjIndex, Vector2 position)
    {
        Vector2 spawningPosition = position;        
        return Instantiate(enemyArr[spawningObjIndex], spawningPosition, Quaternion.identity);
    }

    private void IdentifyEnemyType(string cellData, out EnemyType spawningEnem)
    {
        string[] individualEnemies = ParseRowData(cellData);
        foreach (string enemy in individualEnemies)
        {
            if (enemy != null)
            {
                if (enemy == "C")
                {
                    spawningEnem = EnemyType.CatHouse;
                    return;
                }
                else if (enemy == "B")
                {
                    spawningEnem = EnemyType.BombCat;
                    return;
                }
                else if (enemy == "D")
                {
                    spawningEnem = EnemyType.Drone;
                    return;
                }
                else if(enemy == "0")
                {
                    spawningEnem = EnemyType.None;
                    Debug.Log("Enemy Slot Empty");
                    return;
                }
                else
                {
                    spawningEnem = EnemyType.None;
                    Debug.Log("Unidentified Enemy");
                    return;
                }
            }
            else
            {
                spawningEnem = EnemyType.None;
                return;
            }
        }
        spawningEnem = EnemyType.None;

    }

    private string[] ParseRowData(string rawSpawningD)
    {
        string[] spawningEnemies = rawSpawningD.Split(',');
        return spawningEnemies;
    }
}
