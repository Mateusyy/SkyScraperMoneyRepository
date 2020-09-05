using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static void Initialize()
    {
        PlayerManager.Create();
        SettingsGame.Create();
    }

    public static void Delete()
    {

    }

    public static void Verify()
    {
        if (!BinarySerializer.FileExists(typeof(PlayerManager).Name))
        {
            PlayerManager.Create();
        }
        else
        {
            PlayerManager.instance.ReloadData();
        }

        if (!BinarySerializer.FileExists(typeof(SettingsGame).Name))
        {
            SettingsGame.Create();
        }
    }

    public static void ReloadData()
    {
        Delete();
        Initialize();
    }
}
