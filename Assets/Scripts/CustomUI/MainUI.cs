using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainUI : MonoBehaviour
{
    public RectTransform bottomPanel;
    [SerializeField]
    private Animator cashTextTopButtonAnimator;

    public static MainUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as MainUI;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void SetHeightAndPosYForRectTransform(RectTransform rectTransform, float height)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, height / 2);
    }

    public void PlayTopCashAnim()
    {
        cashTextTopButtonAnimator.SetTrigger("GetMoney");
    }
}
