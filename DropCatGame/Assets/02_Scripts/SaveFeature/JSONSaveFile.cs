using System.IO;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class JSONSaveFileFormat
{
    public int currentLevel = 3;
}

public class JSONSaveFile : MonoBehaviour
{
    public JSONSaveFileFormat saveFile;
    [SerializeField] private LevelCompleteEventManager levelCompleteEvent;

    public static JSONSaveFile instance;

    private void Awake()
    {
        saveFile = new JSONSaveFileFormat();
        CreateUserDataDirectory();
        if(levelCompleteEvent != null)
            levelCompleteEvent.onLevelCompleteTriggerEnter += UpdateData;
        ReadData();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        Debug.Log(saveFile.currentLevel);
    }

    private void CreateUserDataDirectory()
    {
        string directory = Path.Combine(Application.dataPath + "/StreamingAssets" + "/PlayerData");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private void UpdateData()
    {
        if(1 < saveFile.currentLevel)
        {
            string playerData = JsonUtility.ToJson(saveFile);

            string JSONPath = Path.Combine(Application.dataPath + "/StreamingAssets" + "/PlayerData/SaveFile.json");
            File.WriteAllText(JSONPath, playerData);
        }
    }

    public void ReadData()
    {
        string dataCache = File.ReadAllText(Application.dataPath + "/StreamingAssets" + "/PlayerData/SaveFile.json");
        JsonData data = JsonMapper.ToObject(dataCache);
        Debug.Log((int)data["currentLevel"]);
        saveFile.currentLevel = (int)data["currentLevel"];
    }
}