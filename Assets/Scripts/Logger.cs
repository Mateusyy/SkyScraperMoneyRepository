using UnityEngine;
using UnityEditor;
using System.IO;

public class Logger
{
    public static void WriteLog(string log)
    {
        string path = "Assets/Resources/logs.txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(log);
        writer.Close();
    }
}
