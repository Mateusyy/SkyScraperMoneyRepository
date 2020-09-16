using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    //public Text infoText;
    public string authCode = string.Empty;
    public Firebase.Auth.FirebaseUser user;

    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //Firebase Analitics
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });

        //Google Play Service Signin
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            .RequestIdToken()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowData();
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Remote config ready");
    }

    public void FetchFirebase()
    {
        FetchDataAsync();
    }

    public void ShowData()
    {
        Debug.Log("TestRemoteConfigValue: " + FirebaseRemoteConfig.GetValue("TestRemoteConfigValue").LongValue);
    }

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.FetchAsync(
            TimeSpan.Zero);

        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).",
                                       info.FetchTime));
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }

    public void SignIn()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated != true)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    string playerUserName = PlayGamesPlatform.Instance.localUser.userName;
                    //infoText.text += "GooglePlatformSigned: " + playerUserName + "\n";

                    authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                    if (!string.IsNullOrEmpty(authCode))
                    {
                        //infoText.text += "auth _1\n";
                        //FirebaseAuthenticate();
                    }
                    else
                    {
                        //infoText.text += "auth is nullOrEmpty_1\n";
                    }
                }
                else
                {
                    //infoText.text += ":((\n";
                }
            });
        }
        else
        {

        }
    }

    public static void RegisterEvent(string name, string parameter, int valueOfParameter)
    {
        FirebaseAnalytics.LogEvent(name, parameter, valueOfParameter);
    }

    private void FirebaseAuthenticate()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignIn was canceled.");
                //infoText.color = Color.red;
                //infoText.text += "SignIn was canceled.\n";
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignIn throw exception: " + task.Exception.Message + "\n");
                //infoText.color = Color.red;
                //infoText.text += "\nSignIn throw exception: " + task.Exception.Message;
                return;
            }

            if (task.IsCompleted)
            {
                user = task.Result;
                //infoText.text += "isCompleted";
                //infoText.text += "\n" + user.DisplayName + " " + user.UserId;
            }
        });
    }
}
