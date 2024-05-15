using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //public string[] saveFiles; ���� ����
    private string saveLevelName = "LevelData";
    private string saveOptionName = "OptionData";
    public void OnSaveLevel()
    {
        SerializationManager.Save(saveLevelName, SaveData.Current);
    }

    public void OnSaveOption()
    {
        SerializationManager.Save(saveOptionName, SaveData.Current);
    }

    private void Awake()
    {
        OnLoadOption();
        OnLoadLevel();
    }

    public void OnLoadLevel()
    {
        SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + saveLevelName + ".save");
    }

    public void OnLoadOption()
    {
        SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + saveOptionName + ".save");
    }

    private void GetLoadFile()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }
        //saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
    }
}
