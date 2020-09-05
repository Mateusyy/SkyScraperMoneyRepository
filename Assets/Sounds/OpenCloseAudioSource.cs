using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseAudioSource : MonoBehaviour
{
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if(SettingsGame.instance.isSound == true)
        {
            _audio.Play();
        }
    }
}
