using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Unity.Editor;
using GooglePlayGames;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DatabaseInit : MonoBehaviour
{
    public static DatabaseInit instance;
    public DataSnapshot datasnapshot;
    public DataSnapshot datasnapshotUser;
    public PlayerDataRoot players;

    private bool googlePlayIsAuthenticated = false;
    public DatabaseReference reference;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as DatabaseInit;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    /*private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://skyscraper-money.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task<bool> UpdateLeaderboard(int score)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null)
            {
                PlayerData player = new PlayerData(user.UserId, user.DisplayName, score);
                string json = JsonUtility.ToJson(player);

                await reference.Child("users").Child(user.UserId.ToString()).SetRawJsonValueAsync(json);

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> GetAllUsersAsync()
    {
        datasnapshot = await FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .GetValueAsync();

        return datasnapshot.Exists;
    }

    public async Task<bool> GetUser(string uid)
    {
        datasnapshotUser = await FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(uid)
            .GetValueAsync();

        return datasnapshotUser.Exists;
    }

    public void ParseDatasnapshot()
    {
        List<PlayerData> downloadedPlayers = ParseData(datasnapshot).ToList();
        var sortedPlayers = downloadedPlayers.OrderBy(score => score.score).ToList();
        sortedPlayers.Reverse();
        players = new PlayerDataRoot(sortedPlayers);
    }

    private List<PlayerData> ParseData(DataSnapshot result)
    {
        return result.Children
            .Select(record => PlayerData.CreateScoreFromRecord(record))
            .ToList();
    }

    public IEnumerator PutAndUpdateScore(int score)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser firebaseUser = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;

        if (firebaseUser != null)
        {
            var getUserAsyncTask = DatabaseInit.instance.GetUser(firebaseUser.UserId);
            yield return new WaitUntil(() => getUserAsyncTask.IsCompleted);

            if (getUserAsyncTask.Result)
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(DatabaseInit.instance.datasnapshotUser.GetRawJsonValue());
                playerData.score += score;
                RestClient.Put("https://skyscraper-money.firebaseio.com/users/" + firebaseUser.UserId + ".json", playerData);
            }
            else
            {
                PlayerData playerData = new PlayerData(firebaseUser.UserId, firebaseUser.DisplayName, score);
                RestClient.Put("https://skyscraper-money.firebaseio.com/users/" + firebaseUser.UserId + ".json", playerData);
            }
        }
    }
    public static void PostLeaderboard(int score)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null)
            {
                PlayerData player = new PlayerData(user.UserId, user.DisplayName, score);

                RestClient.Put("https://skyscraper-money.firebaseio.com/users/" + player.uid + ".json", player);

                return;
            }
            else
            {
                Debug.LogError("Authorized user is equal null.");
                return;
            }
        }
        else
        {
            Debug.LogError("Google Play Platform is not authorized.");
            return;
        }
    }*/
}
