using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySerializer 
{
    public static T Load<T>(string fileName) where T: class
    {
        Debug.Log(Application.persistentDataPath);
        string path = PathForFilename(fileName);
        if (BinarySerializer.PathExists(path))
        {
            try
            {
                using(Stream stream = File.OpenRead(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as T;
                }
            }
            catch(Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
        return default(T);
    }

    public static void Save<T>(string filename, T data) where T: class
    {
        string path = PathForFilename(filename);
        using (Stream stream = File.OpenWrite(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
    }

    private static bool PathExists(string filepath)
    {
        return File.Exists(filepath);
    }

    public static bool FileExists(string filename)
    {
        return PathExists(PathForFilename(filename));
    }

    public static void DeleteFile(string filename)
    {
        string filepath = PathForFilename(filename);
        if (PathExists(filepath))
        {
            File.Delete(filepath);
        }
    }

    private static string PathForFilename(string filename)
    {
        string path = filename;
        path = Path.Combine(Application.persistentDataPath, filename);
        return path;
    }
}
