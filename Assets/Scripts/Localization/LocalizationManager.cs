using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    [System.Serializable]
    public class SupportedLanguage
    {
        public SystemLanguage language;
        public TextAsset jsonDatabase;
    }

    [SerializeField]
    public SupportedLanguage[] supportedLanguages;
    public Dictionary<string, string> textDatabase;
    [SerializeField]
    private bool matchSystemLanguage = true;
    [SerializeField]
    private SystemLanguage defaultLanguage = SystemLanguage.English;
    private SystemLanguage localizedLanguage
    {
        get { return supportedLanguages[SettingsGame.instance.localizedLanguage].language; }
    }
    public bool isLoaded { get; private set; }
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as LocalizationManager;
            DontDestroyOnLoad(gameObject);
            instance.Init();
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    protected void Init()
    {

    }

    private void Start()
    {
        
    }

    public void LoadDatabase()
    {
        textDatabase = JSONSerializer.FromJson<Dictionary<string, string>>(supportedLanguages[SettingsGame.instance.localizedLanguage].jsonDatabase.text);
        isLoaded = true;
    }

    public string StringForKey(string key)
    {
        string result = key;
        if (textDatabase.ContainsKey(key))
        {
            result = textDatabase[key];
        }
        else
        {
            Debug.LogError("key " + key + " not found");
        }
        return result;
    }
}
