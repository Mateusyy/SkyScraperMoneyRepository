using Firebase;
using Firebase.Analytics;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseInit : MonoBehaviour
{
    //public Text infoText;
    public string authCode = string.Empty;
    public Firebase.Auth.FirebaseUser user;

    private void Start()
    {
        ///infoText.text = "START\n";
        //infoText.color = Color.black;

        //Firebase Analitics
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            if (task.IsCompleted)
            {
                //infoText.text += "Analitics is on\n";
            }
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
            authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            if (!string.IsNullOrEmpty(authCode))
            {
                //infoText.text += "auth _2\n";
                //FirebaseAuthenticate();
            }
            else
            {
                //infoText.text += "auth is nullOrEmpty_2\n";
            }
        }
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
