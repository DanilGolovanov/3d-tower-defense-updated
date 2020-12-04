using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

//script to handle main menu, options and character select functions & events

public class MenuHandler : MonoBehaviour
{
    //reference to UI elements
    public AudioMixer masterAudio;
    public Resolution[] resolutions;
    public Dropdown resolution;
    public Toggle fullscreenToggle;
    public Slider musicSlider;
    public Slider sFXSlider;

    //Populate screen resolutions dropbox with available options
    //Load Playerprefs
    private void Start()
    {
        //generate screen resolutions to populate options with
        resolutions = Screen.resolutions; 
        //clear dropbox
        resolution.ClearOptions();
        //create new list called options
        List<string> options = new List<string>();
        //start new index
        int currentResolutionsIndex = 0;
        //add each resolution option into the dropbox field
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionsIndex = i;
            }
        }
        resolution.AddOptions(options);

        resolution.value = currentResolutionsIndex;

        resolution.RefreshShownValue();
        //load player preferences if the playerpref file has value, if not use defaults
        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }
        else
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }

        if (!PlayerPrefs.HasKey("quality"))
        {
            PlayerPrefs.SetInt("quality", 5);//dont have magic numbers
            QualitySettings.SetQualityLevel(5);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }

        LoadPlayerPrefs();
    }
    /// <summary>
    /// method to change volume group from audiomixer
    /// this is for music
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolume(float volume)
    {
        masterAudio.SetFloat("volume", volume);
    }
    /// <summary>
    /// method to change volume group from audiomixer
    /// this is for sound effects
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSfxVolume(float volume)
    {
        masterAudio.SetFloat("sfxvolume", volume);
    }
    /// <summary>
    /// method to mute audio, this mutes the master audio mixer group
    /// </summary>
    /// <param name="isMuted"></param>
    public void ToggleMute(bool isMuted)
    {
        if (isMuted)
        {
            masterAudio.SetFloat("isMutedVolume", -80);
        }
        else
        {
            masterAudio.SetFloat("isMutedVolume", 0);
        }
    }
    /// <summary>
    /// method to set screen resolution to chosen field in dropdown
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    /// <summary>
    /// method to toggle fullscreen resolution on or off
    /// </summary>
    /// <param name="isFullScreen"></param>
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    /// <summary>
    /// method to change quality settings available in build settings
    /// </summary>
    /// <param name="qualityIndex"></param>
    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    /// <summary>
    /// method to load another scene
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    /// <summary>
    /// method to quit unity editor runtime mode
    /// or quit the application if not in editor
    /// </summary>
    public void ExitToDesktop()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    /// <summary>
    /// method to save the player preferences
    /// </summary>
    public void SavePlayerPrefs()
    {
        //save quality
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());

        //save fullscreen
        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        //save audio Sliders
        float musicVol;
        if (masterAudio.GetFloat("volume", out musicVol))
        {
            PlayerPrefs.SetFloat("volume", musicVol);
        }

        float SFXVol;
        if (masterAudio.GetFloat("sfxvolume", out SFXVol))
        {
            PlayerPrefs.SetFloat("sfxvolume", SFXVol);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// method to load from the player preferences
    /// </summary>
    public void LoadPlayerPrefs()
    {
        //Load Quality
        int quality = PlayerPrefs.GetInt("quality");
        resolution.value = quality;
        if (QualitySettings.GetQualityLevel() != quality)
        {
            Quality(quality);
        }

        //load fullscreen
        if (PlayerPrefs.GetInt("fullscreen") == 0)
        {
            fullscreenToggle.isOn = false;
        }
        else
        {
            fullscreenToggle.isOn = true;
        }
                
        //load audio Sliders
        float musicVol = PlayerPrefs.GetFloat("volume");
        musicSlider.value = musicVol;
        masterAudio.SetFloat("volume", musicVol);
        float SFXVol = PlayerPrefs.GetFloat("sfxvolume");
        sFXSlider.value = SFXVol;
        masterAudio.SetFloat("sfxvolume", SFXVol);
    }
}
