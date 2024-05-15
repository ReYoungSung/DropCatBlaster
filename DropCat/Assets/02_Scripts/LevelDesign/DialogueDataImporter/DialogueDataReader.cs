using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public class DialogueDataReader : SpawningData
{
    public DialogueDataReader(int levelNum)
        : base(levelNum)
    {
        string confirmedDirectoryPath = SetDirectoryPath(levelNum);
        jsonText = ReadJSONIntoText(confirmedDirectoryPath);
    }

    public DialogueDataReader(string levelName)
        : base(levelName)
    {
    }

    public void EnqueueDialogues(string chapter, ref Queue<string> queue)
    {
        if(jsonText != null)
        {
            JsonData data = JsonMapper.ToObject(jsonText);
            int maxRow = data[chapter].Count;
            for (int i = 0; i < maxRow; i++)
            {
                queue.Enqueue(data[chapter][i.ToString()].ToString());
            }
        }
    }

    public void AddDialogues(string chapter, ref List<string> list)
    {
        if(jsonText != null)
        {
            JsonData data = JsonMapper.ToObject(jsonText);
            int maxRow = data[chapter].Count;
            for (int i = 0; i < maxRow; i++)
            {
                list.Add(data[chapter][i.ToString()].ToString());
            }
        }
    }
}
