using GameOptionFlags;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameOptionFlags
{
    public enum BGMStatus
    {
        BGMOn = 0, BGMOff = 1
    }

    public enum SFXStatus
    {
        SFXOn = 0, SFXOff = 1
    }

    public enum VibrationStatus
    {
        VibrationOn = 0, VibrationOff = 1
    }
}

public class OptionsPageManager : MonoBehaviour
{
    private Button toggleBGMBtn = null;
    private Button toggleSFXBtn = null;
    private Button toggleVibrationBtn = null;

    private BGMStatus bgmStatus;
    private SFXStatus sfxStatus;
    private VibrationStatus vibrationStatus;

    private SaveManager saveManager = null;

    public BGMStatus GetBGMStatus { get { return bgmStatus; } }
    public SFXStatus GetSFXStatus { get { return sfxStatus; } }
    public VibrationStatus GetVibrationStatus { get { return vibrationStatus; } }


    private void Awake()
    {
        bgmStatus = SaveData.Current.optionSaveProfile.bgmStatus;
        sfxStatus = SaveData.Current.optionSaveProfile.sfxStatus;
        vibrationStatus = SaveData.Current.optionSaveProfile.vibrationStatus;

        toggleBGMBtn = this.transform.GetChild(1).GetComponent<Button>();
        toggleSFXBtn = this.transform.GetChild(2).GetComponent<Button>();
        toggleVibrationBtn = this.transform.GetChild(3).GetComponent<Button>();

        toggleBGMBtn.gameObject.GetComponent<ButtonSwapControl>().
            SwapButtonImage((int)SaveData.Current.optionSaveProfile.bgmStatus);
        toggleSFXBtn.gameObject.GetComponent<ButtonSwapControl>().
            SwapButtonImage((int)SaveData.Current.optionSaveProfile.sfxStatus);
        toggleVibrationBtn.gameObject.GetComponent<ButtonSwapControl>().
            SwapButtonImage((int)SaveData.Current.optionSaveProfile.vibrationStatus);

        toggleBGMBtn.onClick.AddListener(ToggleBGM);
        toggleSFXBtn.onClick.AddListener(ToggleSFX);
        toggleVibrationBtn.onClick.AddListener(ToggleVibration);

        saveManager = this.GetComponent<SaveManager>();
    }

    private void ToggleBGM()
    {
        if (bgmStatus == BGMStatus.BGMOn)
        {
            bgmStatus = BGMStatus.BGMOff;
        }
        else
        {
            bgmStatus = BGMStatus.BGMOn;
        }
        toggleBGMBtn.gameObject.GetComponent<ButtonSwapControl>().
           SwapButtonImage((int)bgmStatus);
        SaveData.Current.optionSaveProfile.bgmStatus = this.bgmStatus;
    }

    private void ToggleSFX()
    {
        if (sfxStatus == SFXStatus.SFXOn)
        {
            sfxStatus = SFXStatus.SFXOff;
        }
        else
        {
            sfxStatus = SFXStatus.SFXOn;
        }
        toggleSFXBtn.gameObject.GetComponent<ButtonSwapControl>().
           SwapButtonImage((int)sfxStatus);
        SaveData.Current.optionSaveProfile.sfxStatus = this.sfxStatus;
    }

    private void ToggleVibration()
    {
        if (vibrationStatus == VibrationStatus.VibrationOn)
        {
            vibrationStatus = VibrationStatus.VibrationOff;
        }
        else
        {
            vibrationStatus = VibrationStatus.VibrationOn;
        }
        toggleVibrationBtn.gameObject.GetComponent<ButtonSwapControl>().
           SwapButtonImage((int)vibrationStatus);
        SaveData.Current.optionSaveProfile.vibrationStatus = this.vibrationStatus;
    }

    private void OnDisable()
    {
        SaveOptions();
    }

    private void OnDestroy()
    {
        SaveOptions();
    }

    private void OnApplicationQuit()
    {
        SaveOptions();
    }

    public void SaveOptions()
    {
        if (saveManager != null)
            saveManager.OnSaveOption();
    }
}
