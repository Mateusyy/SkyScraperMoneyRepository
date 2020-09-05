using System;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string uid;
    public string name;
    public long score;

    public PlayerData()
    {
        uid = string.Empty;
        name = string.Empty;
        score = 0;
    }

    public PlayerData(string uid, string name, long score)
    {
        this.uid = uid;
        this.name = name;
        this.score = score;
    }

    public PlayerData(DataSnapshot record)
    {
        if (record.Child("uid").Exists)
        {
            this.uid = record.Child("uid").Value.ToString();
        }
        if (record.Child("name").Exists)
        {
            this.name = record.Child("name").Value.ToString();
        }
        long score;
        if (Int64.TryParse(record.Child("score").Value.ToString(), out score))
        {
            this.score = score;
        }
        else
        {
            this.score = 0;
        }
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", name, score, uid);
    }

    public static PlayerData CreateScoreFromRecord(DataSnapshot record)
    {
        if (record == null)
            return null;

        return new PlayerData(record);
    }
}

[Serializable]
public class PlayerDataRoot
{
    public List<PlayerData> playerData = new List<PlayerData>();

    public PlayerDataRoot(List<PlayerData> playerData)
    {
        this.playerData = playerData;
    }
}
