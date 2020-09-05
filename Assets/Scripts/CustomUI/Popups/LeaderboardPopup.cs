using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardPopup : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private CanvasGroup noSignInPanel;
    [SerializeField]
    private CanvasGroup signInPanel;
    [SerializeField]
    private LeaderoardPanel leaderoardPanelPrefab;
    [SerializeField]
    private RectTransform scrollViewContent;

    public List<LeaderoardPanel> slotsOfLeaderboard = new List<LeaderoardPanel>();
    private Coroutine coroutine;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Show()
    {
        //ShowCorrectPanel();
        if (coroutine == null)
        {
            //coroutine = StartCoroutine(GenerateSlotForLeaderboardWithCoroutine());
        }
    }

    public void Hide()
    {
        DestroySlotsOfLeaderboard();
        anim.SetTrigger("Hide");
    }

    public void ShowCorrectPanel()
    {
        if (CheckFirebaseAuthorization())
        {
            SetOffPanel(noSignInPanel);
            SetOnPanel(signInPanel);
        }
        else
        {
            SetOffPanel(signInPanel);
            SetOnPanel(noSignInPanel);
            anim.SetTrigger("Show");
        }
    }

    private bool CheckFirebaseAuthorization()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if(user != null)
        {
            //FirebaseInit.infoText.text += user.DisplayName + "authorized!";
            return true;
        }

        //FirebaseInit.infoText.text += "somethingWentWeryWrong";
        return false;
    }

    private void SetOnPanel(CanvasGroup panel)
    {
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.alpha = 1f;
    }

    private void SetOffPanel(CanvasGroup panel)
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.alpha = 0f;
    }

    /*private IEnumerator GenerateSlotForLeaderboardWithCoroutine()
    {
        var getAllUsersAsyncTask = DatabaseInit.instance.GetAllUsersAsync();
        yield return new WaitUntil(() => getAllUsersAsyncTask.IsCompleted);

        if (getAllUsersAsyncTask.Result)
        {
            anim.SetTrigger("Show");
            SetOffPanel(noSignInPanel);
            SetOnPanel(signInPanel);

            //DatabaseInit.instance.ParseDatasnapshot();
            for (int i = 0; i < DatabaseInit.instance.players.playerData.Count; i++)
            {
                PlayerData user = DatabaseInit.instance.players.playerData[i];
                LeaderoardPanel leaderoardPanel = Instantiate(leaderoardPanelPrefab, scrollViewContent);
                slotsOfLeaderboard.Add(leaderoardPanel);
                leaderoardPanel.Initialize(user, i);
            }
        }
        else
        {
            anim.SetTrigger("Show");
            SetOffPanel(signInPanel);
            SetOnPanel(noSignInPanel);
            Debug.Log("Powinienes zaczekać");
        }

        coroutine = null;
    }*/

    private void DestroySlotsOfLeaderboard()
    {
        if(slotsOfLeaderboard != null && slotsOfLeaderboard.Count > 0)
        {
            foreach (var slot in slotsOfLeaderboard)
            {
                Destroy(slot.gameObject);
            }
            slotsOfLeaderboard.Clear();
        }
    }
}
