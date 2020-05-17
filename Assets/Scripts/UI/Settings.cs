using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public SettingsData data;

    Resolution[] resolutions;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectsSlider;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;

    public AudioMixer audioMixer;

    //Set start settings
    private void Start()
    {

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        //Construct Resolution dropdown
        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;

        //Load data
        if (!loadData())
        {
            saveData();
        }
        setData(data);
    }
    

    private bool loadData()
    {
        string destination = Application.persistentDataPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        data = (SettingsData)bf.Deserialize(file);
        file.Close();
        Debug.Log("Settings Loaded");
        return true;
    }

    //save data to file
    public void saveData()
    {
        data.MasterVolume = MasterSlider.value;
        data.MusicVolume = MusicSlider.value;
        data.EffectsVolume = EffectsSlider.value;
        data.QualityLevel = qualityDropdown.value;
        data.ResolutionLevel = resolutionDropdown.value;
        data.Fullscreen = fullscreenToggle.isOn;

        string destination = Application.persistentDataPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Settings Saved");
        return;
    }

    //set data values
    public void setData(SettingsData settingsData)
    {
        data = settingsData;

        SetMasterVolume(data.MasterVolume);
        MasterSlider.value = data.MasterVolume;
        SetEffectsVolume(data.EffectsVolume);
        EffectsSlider.value = data.EffectsVolume;
        SetMusicVolume(data.MusicVolume);
        MusicSlider.value = data.MusicVolume;

        SetQuality(data.QualityLevel);
        qualityDropdown.value = data.QualityLevel;

        SetResolution(data.ResolutionLevel);
        resolutionDropdown.value = data.ResolutionLevel;

        SetFullscreen(data.Fullscreen);
        fullscreenToggle.isOn = data.Fullscreen;
        Debug.Log("Settings Set");
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool toggle)
    {
        
        Screen.fullScreen = toggle;
    }
}
