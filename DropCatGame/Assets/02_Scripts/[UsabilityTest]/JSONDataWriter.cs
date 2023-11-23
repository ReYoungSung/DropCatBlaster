using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class JSONDataFormat<T>
{
    public List<T> data = new List<T>();
}

public class JSONDataWriter : MonoBehaviour
{
    [SerializeField] GameObject player;
    private JSONDataFormat<Vector2> characterPosition = new JSONDataFormat<Vector2>();
    private JSONDataFormat<float> capturedTime = new JSONDataFormat<float>();

    private int flag = 0;
    private bool isWriting = true;

    [SerializeField] private GameObject levelCompleteHolder;
    [SerializeField] private GameObject gameOverHolder;
    private LevelCompleteEventManager levelCompleteEvent;
    private GameOverEventManager gameOver;

    private void Awake()
    {
        levelCompleteEvent = levelCompleteHolder.GetComponent<LevelCompleteEventManager>();
        gameOver = gameOverHolder.GetComponent<GameOverEventManager>();
        levelCompleteEvent.onLevelCompleteTriggerEnter += FinishDataWriting;
        gameOver.onGameOverTriggerEnter += FinishDataWriting;
        CreateUserDataDirectory();
    }

    private void Start()
    {
        characterPosition = new JSONDataFormat<Vector2>();
        capturedTime = new JSONDataFormat<float>();
        isWriting = true;
    }

    private void Update()
    {
        if(isWriting)
        {
            if (flag % 4 == 0)
            {
                characterPosition.data.Add(player.transform.position);
                capturedTime.data.Add(Time.timeSinceLevelLoad);
            }
            ++flag;
            Debug.Log("Character Position is logged");
        }
    }

    private void CreateUserDataDirectory()
    {
        string directory = Path.Combine(Application.dataPath + "/USER_DATA");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private void FinishDataWriting()
    {
        isWriting = false;
        string characterPos = JsonUtility.ToJson(characterPosition);
        string characterT = JsonUtility.ToJson(capturedTime);

        string JSONPath = Path.Combine(Application.dataPath + "/USER_DATA/level_" + (SceneManager.GetActiveScene().buildIndex - 2).ToString()
            + "_Trial_" + GameInfoLogger.currentTrial.ToString() + ".json");

        File.WriteAllText(JSONPath, characterPos);
    }
}
