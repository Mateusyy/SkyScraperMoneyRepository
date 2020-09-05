using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsGame
{
    public bool isFirstTimeStatus { get; set; }
    public int localizedLanguage { get; private set; }
    public bool isSound { get; set; }

    public static SettingsGame _instance;

    public static SettingsGame instance
    {
        get { return _instance ?? (_instance = BinarySerializer.Load<SettingsGame>(typeof(SettingsGame).Name)); }
    }

    public static void Create()
    {
        if (!BinarySerializer.FileExists(typeof(SettingsGame).Name))
        {
            _instance = (SettingsGame)System.Activator.CreateInstance(type: typeof(SettingsGame), nonPublic: true);
        }
    }

    protected SettingsGame()
    {
        isFirstTimeStatus = true;
        localizedLanguage = -1;
        isSound = true;
    }

    public void FirstTimeSetter(bool value)
    {
        isFirstTimeStatus = value;
        Save();
    }

    public void SetLocalizedLanguage(int localizedLanguage)
    {
        this.localizedLanguage = localizedLanguage;
        Save();
    }

    public void SetIsSound(bool value)
    {
        isSound = value;
        Save();
    }

    protected void Save()
    {
        BinarySerializer.Save(typeof(SettingsGame).Name, this);
    }
}
