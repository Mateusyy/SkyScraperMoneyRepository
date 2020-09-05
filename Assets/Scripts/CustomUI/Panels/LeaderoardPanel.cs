using GooglePlayGames;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderoardPanel : MonoBehaviour
{
    [SerializeField]
    private Image background;

    [SerializeField]
    private Text numberText;
    private int number;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text nameText;
    private string _name;

    [SerializeField]
    private Text pointsText;
    private string points;

    public void Initialize(PlayerData user, int index)
    {
        this.number = index + 1;
#if UNITY_ANDROID
        //this.icon.sprite = Sprite.Create(PlayGamesPlatform.Instance.localUser.image,
        //    new Rect(0.0f, 0.0f, PlayGamesPlatform.Instance.localUser.image.width, PlayGamesPlatform.Instance.localUser.image.height),
        //    new Vector2(0.5f, 0.5f), 100.0f);
#endif
        this._name = user.name;
        this.points = user.score.ToString();

        numberText.text = number.ToString();
        nameText.text = name.ToString();
        pointsText.text = points;

        Firebase.Auth.FirebaseUser firebaseUser = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        if (firebaseUser != null)
        {
            if (user.uid.Equals(firebaseUser.UserId))
            {
                //current user and change colour of background
                //var currentUserInLeaderboardColor = new Color(255, 0, 0);
                //currentUserInLeaderboardColor.a = 0.72f;
                background.color = new Color32(255, 0, 0, 72);
            }
        }
    }
}
