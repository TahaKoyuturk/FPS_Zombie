using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #endregion

    #region Variables

    public Sound[] sounds;
    public AudioSource source;

    #endregion

    #region Custom Methods

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, x=>x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound Not Found!");
        }
        else
        {
            source.clip = s.clip;
            source.Play();
           
        }
    }

    #endregion
}
