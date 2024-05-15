using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private static SaveData current;
    public static SaveData Current
    {
        get
        {
            if(current == null)
            {
                current = new SaveData();
            }
            return current; 
        }
        set
        {
            current = value;
        }
    }

    public PlayerSaveProfile playerProfile = new PlayerSaveProfile();
    public OptionSaveProfile optionSaveProfile = new OptionSaveProfile();
}
