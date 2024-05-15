using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Android;

public class SpawningData
{
    protected string jsonText = null;
    private int catHouse = 0;
    public int CatHouse { get { return catHouse; } set { catHouse = value; } }

    public SpawningData(int levelNum)
    {
        /*
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        */
        string confirmedDirectoryPath = SetDirectoryPath(levelNum);
        BetterStreamingAssets.Initialize();
        jsonText = ReadJSONIntoText(confirmedDirectoryPath);
    }

    public SpawningData(string levelName)
    {
        /*
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        */
        string confirmedDirectoryPath = SetDirectoryPath(levelName);
        BetterStreamingAssets.Initialize();
        jsonText = ReadJSONIntoText(confirmedDirectoryPath);
    }

    public virtual string SetDirectoryPath(int levelNum)
    {
        string confirmedDirPath = null;
        if (IsLessThanTen(levelNum))
            confirmedDirPath = Convert.ToString(levelNum) + ".json";
        else
            confirmedDirPath = Convert.ToString(levelNum) + ".json";
        return confirmedDirPath;
    }

    public virtual string SetDirectoryPath(string levelN)
    {
        return levelN;
    }

    public string ReadJSONIntoText(string JSONPath)
    {
        TextAsset JSONTextFile = Resources.Load<TextAsset>(JSONPath);
        if(JSONTextFile != null)
        {
            return JSONTextFile.ToString();
        }
        return null;
    }

    public void StackColumns(ref Stack rowS)
    {
        JsonData data = JsonMapper.ToObject(jsonText);
        int maxRow = data["C_00"].Count;
        int maxColumnNumber = data.Keys.Count;
        for (int rowIndex = 0; rowIndex < maxRow; rowIndex++)
        {
            Row row = new Row(maxColumnNumber);
            for (int columnIndex = 0; columnIndex < maxColumnNumber; columnIndex++)
            {
                string colInd = string.Format("{0}{1}", "C_0", columnIndex.ToString());
                string rowInd = rowIndex.ToString();
                string val = VerifySpawningValue(data[colInd][rowInd]);
                AllocateCelltoRowData(val, ref row, columnIndex);
            }
            rowS.Push(row);
        }
    }

    private string VerifySpawningValue(JsonData cellData)
    {
        string valStr = null;
        if (CellIsOccupied(cellData))
        {
            valStr = cellData.ToString();
            ++catHouse;
        }
        else
        {
            valStr = "0";
        }
        return valStr;
    }

    private bool CellIsOccupied(JsonData valStr)
    {
        return valStr != null;
    }

    private void AllocateCelltoRowData(string val, ref Row row, int columnI)
    {
        row.SetRowData(columnI, val);
    }

    public bool IsLessThanTen(int levelNum)
    {
        return levelNum < 10;
    }
}

public class Row
{
    private string[] rowData = null;
    public string[] Data { get { return rowData; } }

    public Row(int maxColumnNumber)
    {
        rowData = new string[maxColumnNumber];
    }

    public string[] GetRowData()
    {
        return rowData;
    }
    public void SetRowData(string[] val)
    {
        rowData = val;
    }
    public void SetRowData(int index, string val)
    {
        rowData[index] = val;
    }
}
