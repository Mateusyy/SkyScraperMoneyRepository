using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance { get; private set; }
    private AudioSource backgroundAudio;

    [SerializeField]
    private Sprite soundIsOn;
    [SerializeField]
    private Sprite soundIsOff;
    [SerializeField]
    private Button soundButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as BackgroundMusic;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Start()
    {
        backgroundAudio = GetComponent<AudioSource>();

        while (SettingsGame.instance == null)
        {
            yield return null;
        }

        backgroundAudio.Play();
        if (SettingsGame.instance.isSound == false)
        {
            backgroundAudio.mute = true;
            soundButton.image.sprite = soundIsOff;
        }
        else
        {
            soundButton.image.sprite = soundIsOn;
        }
    }

    public void OnChangeSoundButtonPressed()
    {
        //change on false
        if(SettingsGame.instance.isSound != false)
        {
            backgroundAudio.mute = true;
            soundButton.image.sprite = soundIsOff;
            SettingsGame.instance.SetIsSound(false);
            return;
        }

        //change on true
        if (SettingsGame.instance.isSound != true)
        {
            backgroundAudio.mute = false;
            soundButton.image.sprite = soundIsOn;
            SettingsGame.instance.SetIsSound(true);
            return;
        }
    }
}
