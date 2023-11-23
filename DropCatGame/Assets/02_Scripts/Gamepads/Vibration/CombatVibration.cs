using UnityEngine;
using Lofelt.NiceVibrations;

public class CombatVibration : MonoBehaviour
{
    public void EnemyDestruction()
    {
        if(SaveData.Current.optionSaveProfile.vibrationStatus == GameOptionFlags.VibrationStatus.VibrationOn)
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }
    }

    public void EnemyPunch()
    {
        if (SaveData.Current.optionSaveProfile.vibrationStatus == GameOptionFlags.VibrationStatus.VibrationOn)
        { 
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
    }
}
